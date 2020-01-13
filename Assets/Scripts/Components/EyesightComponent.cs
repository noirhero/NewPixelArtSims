using System;
using Unity.Entities;

[Serializable]
public struct EyesightComponent : IComponentData {
    public Entity target;
    public float thinkingTime;
    public int type;
}