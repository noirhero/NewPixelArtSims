using Unity.Entities;
using Unity.Jobs;

public class MovementSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var walkJobHandle = Entities
            .WithName("MovementSystem_Walk")
            .WithNone<EyesightComponent>()
            .ForEach((ref VelocityComponent velocity) => { velocity.value = velocity.dir; })
            .Schedule(inputDependencies);

        return Entities
            .WithName("MovementSystem_Stop")
            .WithAll<EyesightComponent>()
            .ForEach((ref VelocityComponent velocity) => { velocity.value = 0.0f; })
            .Schedule(walkJobHandle);
    }
}