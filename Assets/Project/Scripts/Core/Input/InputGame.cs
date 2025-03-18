using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputGame : MonoBehaviour
{
    private Dictionary<int, Touch> _activeTouches = new Dictionary<int, Touch>();
    private InputManager _intputManager;

    public void Init(InputManager inputManager)
    {
        _intputManager = inputManager;
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegan(touch);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    OnTouchMoved(touch);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnTouchEnded(touch);
                    break;
            }
        }

        if(Input.touchCount > 0)
        {
            _intputManager.InputHandler.MoveCamera(Input.touches);
            _intputManager.InputHandler.MoveItems(Input.touches);
        }
    }

    private void OnTouchBegan(Touch touch)
    {
        _activeTouches[touch.fingerId] = touch;
        _intputManager.InputHandler.TouchBegun(touch);
    }

    private void OnTouchMoved(Touch touch)
    {
        if (_activeTouches.ContainsKey(touch.fingerId))
        {
            _activeTouches[touch.fingerId] = touch;
        }
    }

    private void OnTouchEnded(Touch touch)
    {
        if (_activeTouches.ContainsKey(touch.fingerId))
        {
            _activeTouches.Remove(touch.fingerId);
            _intputManager.InputHandler.TouchEnd(touch);
        }
    }
}
