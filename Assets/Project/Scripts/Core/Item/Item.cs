using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Item : ItemBase
{
    [SerializeField] private LayerMask _standLayer;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private StandBase _stand;
    private Tween _dropDownTween;

    public override void StartDrag()
    {
        _dropDownTween?.Kill(false);
        if (_stand != null)
        {
            _stand.RemoveItem(this);
            _stand = null;
        }
    }

    public override void Drag(Vector2 newPosition)
    { 
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPosition.x, newPosition.y, Camera.main.nearClipPlane));
        transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
    }

    public override void Drop()
    {
        float rayDistance = 5f;
        Vector2 origin = transform.position;
        origin.y -= _collider.bounds.extents.y;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, _standLayer);

        if (hit.collider != null)
        {
            StandBase stand = hit.collider.GetComponent<StandBase>();
            float targetY = hit.point.y;

            if (Mathf.Abs(origin.y - targetY) < 0.01f)
            {
                _stand = stand;
                _stand.AddItem(this);
                return;
            }

            DropDown(targetY + _collider.bounds.extents.y, () => { _stand = stand; _stand.AddItem(this); });
        }
    }

    public override void SetLayer(int layer)
    {
        _collider.layerOverridePriority = layer;
        _spriteRenderer.sortingOrder = layer;
    }

    private void DropDown(float posY, UnityAction callBack)
    {
        _dropDownTween = transform.DOMoveY(posY, (transform.position.y - posY)/5).OnComplete(() => callBack?.Invoke());
    }
}
