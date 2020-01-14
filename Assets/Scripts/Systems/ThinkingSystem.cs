using Unity.Entities;
using Unity.Jobs;

public class ThinkingSystem : JobComponentSystem {
    private EntityCommandBufferSystem _bufSystem = null;
    protected override void OnCreate() {
        _bufSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var cmdBuf = _bufSystem.CreateCommandBuffer().ToConcurrent();
        var deltaTime = Time.DeltaTime;
        return Entities
            .WithName("ThinkingSystem")
            .WithoutBurst()
            .ForEach((Entity entity, int entityInQueryIndex, ref EyesightComponent eyesight, in PropertyComponent property) => {
                eyesight.thinkingTime -= property.intelligence * deltaTime;
                if (0.0f < eyesight.thinkingTime) {
                    return;
                }

                cmdBuf.AddComponent(entityInQueryIndex, entity, new ReactiveComponent() {
                    target = eyesight.target
                });

                if (SomethingProxy.Type.Wall == (SomethingProxy.Type) eyesight.type) {
                    cmdBuf.AddComponent(entityInQueryIndex, entity, new WallComponent());
                }

                cmdBuf.RemoveComponent<EyesightComponent>(entityInQueryIndex, entity);
            })
            .Schedule(inputDependencies);
    }
}