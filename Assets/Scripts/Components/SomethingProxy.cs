using System.ComponentModel;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class SomethingProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public enum Type {
        None,
        Wall,
        Item
    };
    public Type type = Type.None;
    public float thinkingTime = 0.0f;

    [Header("Item Storage Properties")]
    public float gettingTime = 0.0f;
    public int itemIndex = 0;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (Type.None == type) {
            dstManager.DestroyEntity(entity);
            return;
        }

        var boxCollider = GetComponent<BoxCollider2D>();
        if (null == boxCollider) {
            Debug.LogError("Set box collider 2d, now!!!!!");
            dstManager.DestroyEntity(entity);
            return;
        }

        dstManager.RemoveComponent<LocalToWorld>(entity);
        dstManager.RemoveComponent<Rotation>(entity);
        dstManager.RemoveComponent<Translation>(entity);

        dstManager.AddComponentData(entity, new SomthingComponent() {
            type = (int) type,
            thinkingTime = thinkingTime
        });
        dstManager.AddComponentData(entity, new BoundComponent() {
            posX = transform.position.x + boxCollider.offset.x,
            sizeX = boxCollider.size.x
        });
    }
}