using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class SpriteMeshProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public Mesh mesh = null;
    public Material material = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.RemoveComponent<LocalToWorld>(entity);
        dstManager.RemoveComponent<Rotation>(entity);
        dstManager.RemoveComponent<Translation>(entity);

        dstManager.AddSharedComponentData(entity, new SpriteMeshComponent() {
            mesh = mesh,
            material = material
        });
    }
}