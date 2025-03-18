using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class InputHandler : MonoBehaviour
{
    public UnityAction<List<Touch>> OnCameraMove;

    [SerializeField] private LayerMask _itemLayer;

    private List<int> _cameraTouchesId = new();
    private Dictionary<int, ItemBase> _movebleItems = new();
    private InputManager _intputManager;

    public void Init(InputManager inputManager)
    {
        _intputManager = inputManager;
    }

    public void TouchBegun(Touch touch)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
        Vector2 rayOrigin = new Vector2(worldPosition.x, worldPosition.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, Vector2.zero, 0.1f, _itemLayer);

        if (hits.Length > 0)
        {
            RaycastHit2D bestHit = hits[0];
            int maxPriority = bestHit.collider.layerOverridePriority;

            for (int i = 1; i < hits.Length; i++)
            {
                int currentPriority = hits[i].collider.layerOverridePriority;
                if (currentPriority > maxPriority)
                {
                    bestHit = hits[i];
                    maxPriority = currentPriority;
                }
            }


            if (bestHit.collider.TryGetComponent<ItemBase>(out ItemBase item) && !_movebleItems.ContainsValue(item))
            {
                _movebleItems[touch.fingerId] = item;
                item.StartDrag();
                item.SetLayer(100);
            }
        }
        else
        {
            _cameraTouchesId.Add(touch.fingerId);
        }
        
    }

    public void TouchEnd(Touch touch)
    {
        if (_cameraTouchesId.Contains(touch.fingerId))
        {
            _cameraTouchesId.Remove(touch.fingerId);
        }

        if (_movebleItems.ContainsKey(touch.fingerId))
        {
            _movebleItems[touch.fingerId].Drop();
            _movebleItems.Remove(touch.fingerId);
        }
    }

    public void MoveCamera(Touch[] touches)
    {
        List<Touch> cameraTouches = new List<Touch>();
        for(int i = 0; i < touches.Length; i++)
        {
            if (_cameraTouchesId.Contains(touches[i].fingerId))
            {
                cameraTouches.Add(touches[i]);
            }
        }

        OnCameraMove?.Invoke(cameraTouches);
    }

    public void MoveItems(Touch[] touches)
    {
        for(int i = 0; i < touches.Length; i++)
        {
            if (_movebleItems.ContainsKey(touches[i].fingerId))
            {
                _movebleItems[touches[i].fingerId].Drag(touches[i].position);
            }
        }
    }
}
