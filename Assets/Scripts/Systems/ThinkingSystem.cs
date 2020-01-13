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

                switch ((SomethingProxy.Type) eyesight.type) {
                    case SomethingProxy.Type.Wall:
                        cmdBuf.AddComponent(entityInQueryIndex, entity, new WallComponent());
                        break;
                    case SomethingProxy.Type.Item:
                        cmdBuf.AddComponent(entityInQueryIndex, entity, new ItemStorageComponent());
                        break;
                    default:
                        return;
                }

                cmdBuf.RemoveComponent<EyesightComponent>(entityInQueryIndex, entity);
            })
            .Schedule(inputDependencies);
    }
}