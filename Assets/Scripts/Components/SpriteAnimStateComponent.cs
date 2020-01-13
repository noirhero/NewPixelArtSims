using System;
using Unity.Entities;

[Serializable]
public struct SpriteAnimStateComponent : IComponentData {
    public int oldHash;
    public int hash;
    public float accumTime;
}