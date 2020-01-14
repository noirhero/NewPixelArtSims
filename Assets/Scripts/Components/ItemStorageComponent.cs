using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct ItemStorageComponent : IComponentData {
    public int index;
    public float gettingTime;

    [Header("Don't touch!!!!!")]
    public int used;
}