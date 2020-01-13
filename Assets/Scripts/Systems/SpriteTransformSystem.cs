using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SpriteTransformSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        return Entities
            .WithName("SpriteTransformSystem")
            .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
            .ForEach((ref SpriteTransformComponent transform, in Translation pos, in Rotation rot, in NonUniformScale scale, in VelocityComponent velocity) => {
                var spriteInverseRotation = (0.0f < velocity.dir) ? quaternion.RotateY(math.radians(-180.0f)) : quaternion.identity;
                transform.Value = float4x4.TRS(pos.Value, math.mul(spriteInverseRotation, rot.Value), scale.Value);
            })
            .Schedule(inputDependencies);
    }
}