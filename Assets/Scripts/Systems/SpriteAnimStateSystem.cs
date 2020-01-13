using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class SpriteAnimStateSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var setHashJobHandle = Entities
            .WithName("SpriteAnimStateSystem_SetHash")
            .WithoutBurst()
            .ForEach((ref SpriteAnimStateComponent state, in VelocityComponent velocity) => {
                state.hash = AnimStatePreset.GetHash((0.0f < math.abs(velocity.value)) ? "Walk" : "Idle");
            })
            .Schedule(inputDependencies);
        
        var deltaTime = Time.DeltaTime;
        return Entities
            .WithName("SpriteAnimStateSystem_AccumTime")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref SpriteAnimStateComponent state, in PropertyComponent property) => {
                if (state.oldHash != state.hash) {
                    state.oldHash = state.hash;
                    state.accumTime = 0.0f;
                }
                else {
                    state.accumTime += property.agility * deltaTime;
                }
            })
            .Schedule(setHashJobHandle);
    }
}