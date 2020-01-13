using System;
using Unity.Entities;

[Serializable]
public struct ReactiveComponent : IComponentData {
    public Entity target;
}