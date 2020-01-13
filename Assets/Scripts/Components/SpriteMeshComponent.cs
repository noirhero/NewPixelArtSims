using System;
using Unity.Entities;
using UnityEngine;

public struct SpriteMeshComponent : ISharedComponentData, IEquatable<SpriteMeshComponent> {
    public Mesh mesh;
    public Material material;

    public bool Equals(SpriteMeshComponent other) {
        return other.mesh == mesh;
    }

    public override int GetHashCode() {
        return (null == mesh) ? 0 : mesh.GetHashCode();
    }
}