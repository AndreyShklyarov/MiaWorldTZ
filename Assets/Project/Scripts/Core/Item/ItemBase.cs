using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public abstract void SetLayer(int layer);
    public abstract void StartDrag();
    public abstract void Drag(Vector2 delta);
    public abstract void Drop();
}
