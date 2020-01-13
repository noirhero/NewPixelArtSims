using Unity.Entities;
using Unity.Jobs;

public class WallSystem : JobComponentSystem {
    private EntityCommandBufferSystem _bufSystem = null;
    protected override void OnCreate() {
        _bufSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var cmdBuf = _bufSystem.CreateCommandBuffer().ToConcurrent();
        return Entities
            .WithName("WallSystem")
            .WithoutBurst()
            .WithEntityQueryOptions(EntityQueryOptions.FilterWriteGroup)
            .WithAll<WallComponent>()
            .ForEach((Entity entity, int entityInQueryIndex, ref VelocityComponent velocity) => {
                velocity.dir *= -1.0f;
                cmdBuf.RemoveComponent<WallComponent>(entityInQueryIndex, entity);
                cmdBuf.RemoveComponent<ReactiveComponent>(entityInQueryIndex, entity);
            })
            .Schedule(inputDependencies);
    }
}