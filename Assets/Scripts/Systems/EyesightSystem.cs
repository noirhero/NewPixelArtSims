using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public struct Searcher {
    public Entity entity;
    public float dirX;
    public float posX;
    public float search;
    public float targetAt;
}

public class EyesightSystem : JobComponentSystem {
    private EndSimulationEntityCommandBufferSystem _bufSystem;
    protected override void OnCreate() {
        _bufSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    private EntityQuery _query;
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var entityCount = _query.CalculateEntityCount();
        var searchers = new NativeArray<Searcher>(entityCount, Allocator.TempJob);
        var searchEntitiesJobHandle = Entities
            .WithName("EyesightSystem_CollectSearcher")
            .WithStoreEntityQueryInField(ref _query)
            .WithNone<EyesightComponent>()
            .ForEach((Entity entity, int entityInQueryIndex, in VelocityComponent velocity, in PropertyComponent property, in Translation pos) => {
                searchers[entityInQueryIndex] = new Searcher() {
                    entity = entity,
                    dirX = velocity.dir,
                    posX = pos.Value.x,
                    search = property.search,
                    targetAt = -1.0f
                };
            })
            .Schedule(inputDependencies);

        var cmdBuf = _bufSystem.CreateCommandBuffer().ToConcurrent();
        var jobHandle = Entities
            .WithName("EyesightSystem")
            .WithEntityQueryOptions(EntityQueryOptions.FilterWriteGroup)
            .WithoutBurst()
            .ForEach((Entity entity, int entityInQueryIndex, in SomthingComponent something, in BoundComponent bound) => {
                for (int i = 0; i < searchers.Length; ++i) {
                    var searcher = searchers[i];
                    if (searcher.entity == entity) {
                        continue;
                    }

                    var at = bound.posX - searcher.posX;
                    var checkDir = at * searcher.dirX;
                    if (0.0f > checkDir) { // different direction
                        continue;
                    }
                    else if (math.abs(at) > searcher.search) { // to long distance
                        continue;
                    }

                    if (0.0f <= searcher.targetAt && at > searcher.targetAt) {
                        cmdBuf.SetComponent(entityInQueryIndex, searcher.entity, new EyesightComponent() {
                            target = entity,
                            thinkingTime = something.thinkingTime,
                            type = something.type
                        });
                    }
                    else {
                        cmdBuf.AddComponent(entityInQueryIndex, searcher.entity, new EyesightComponent() {
                            target = entity,
                            thinkingTime = something.thinkingTime,
                            type = something.type
                        });
                    }
                }
            })
            .WithDeallocateOnJobCompletion(searchers)
            .Schedule(searchEntitiesJobHandle);

        _bufSystem.AddJobHandleForProducer(jobHandle);
        return jobHandle;
    }
}