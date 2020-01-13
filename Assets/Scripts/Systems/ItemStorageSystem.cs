using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public struct ItemGetter {
    public Entity entity;
    public float dirX;
    public float posX;
}

public class ItemStorageSystem : JobComponentSystem {
    private EntityCommandBufferSystem _system = null;
    protected override void OnCreate() {
        _system = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    private EntityQuery _query;
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var entityCount = _query.CalculateEntityCount();
        var itemGetters = new NativeArray<ItemGetter>(entityCount, Allocator.TempJob);

        return inputDependencies;
    }
}