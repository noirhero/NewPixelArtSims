using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct SpriteTransformComponent : IComponentData {
    public float4x4 Value;
}