using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct ItemGetter {
    public Entity entity;
    public Entity target;
    public float dirX;
    public float posX;
    public float agility;
}

public class ItemStorageSystem : JobComponentSystem {
    private EntityCommandBufferSystem _system = null;
    protected override void OnCreate() {
        _system = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    private EntityQuery _query;
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var collectCmdBuf = _system.CreateCommandBuffer().ToConcurrent();
        var entityCount = _query.CalculateEntityCount();
        var itemGetters = new NativeArray<ItemGetter>(entityCount, Allocator.TempJob);
        var searchEntitiesJboHandle = Entities
            .WithName("ItemStorageSystem_CollectItemGetter")
            .WithStoreEntityQueryInField(ref _query)
            .ForEach((Entity entity, int entityInQueryIndex, in ReactiveComponent reactive, in VelocityComponent velocity, in Translation pos, in PropertyComponent  property) => {
                itemGetters[entityInQueryIndex] = new ItemGetter() {
                    entity = entity,
                    target = reactive.target,
                    dirX = velocity.dir,
                    posX = pos.Value.x,
                    agility = property.agility
                };
            })
            .Schedule(inputDependencies);
        _system.AddJobHandleForProducer(searchEntitiesJboHandle);

        var deltaTime = Time.DeltaTime;
        var cmdBuf = _system.CreateCommandBuffer().ToConcurrent();
        var jobHandle = Entities
            .WithName("ItemStorageSystem")
            .WithoutBurst()
            .ForEach((Entity entity, int entityInQueryIndex, ref ItemStorageComponent item, in BoundComponent bound) => {
                for (int i = 0; i < itemGetters.Length; ++i) {
                    var itemGetter = itemGetters[i];
                    if (itemGetter.target != entity) {
                        continue;
                    }

                    var at = bound.posX - itemGetter.posX;
                    var checkDir = at * itemGetter.dirX;
                    if (0.0f > checkDir) { // different direction
                        continue;
                    }
                    if (math.abs(at) > bound.sizeX) { // too long distance
                        continue;
                    }

                    if (0 == item.used) {
                        cmdBuf.AddComponent<HoldingComponent>(entityInQueryIndex, itemGetter.entity);
                        item.used = 1;
                    }

                    item.gettingTime -= itemGetter.agility * deltaTime;
                    if (0.0f < item.gettingTime) {
                        continue;
                    }

                    Debug.Log($"{itemGetter.entity} = Get Item {item.index}");
                    cmdBuf.RemoveComponent<HoldingComponent>(entityInQueryIndex, itemGetter.entity);
                    cmdBuf.RemoveComponent<ReactiveComponent>(entityInQueryIndex, itemGetter.entity);

                    cmdBuf.DestroyEntity(entityInQueryIndex, entity);
                    break;
                }
            })
            .WithDeallocateOnJobCompletion(itemGetters)
            .Schedule(searchEntitiesJboHandle);
        _system.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}