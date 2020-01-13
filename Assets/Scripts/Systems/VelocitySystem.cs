using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class VelocitySystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var deltaTime = Time.DeltaTime;

        return Entities
            .WithName("VelocitySystem")
            .WithEntityQueryOptions(EntityQueryOptions.FilterWriteGroup)
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref Translation pos, in VelocityComponent velocity, in PropertyComponent property) => {
                pos.Value.x += velocity.value * property.agility * deltaTime;
            })
            .Schedule(inputDependencies);
    }
}