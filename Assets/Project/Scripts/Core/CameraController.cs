using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _roomBG;
    [SerializeField] private float _dragSpeed = 0.5f;

    private float _leftX;
    private float _rightX;
    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void OnEnable()
    {
        _inputManager.InputHandler.OnCameraMove += MoveCamera;
    }

    private void Start()
    {
        float halfCamWidth = Camera.main.orthographicSize * Camera.main.aspect;
        _leftX = _roomBG.bounds.min.x + halfCamWidth;
        _rightX = _roomBG.bounds.max.x - halfCamWidth;
    }

    private void OnDisable()
    {
        _inputManager.InputHandler.OnCameraMove -= MoveCamera;
    }

    private void MoveCamera(List<Touch> touches)
    {
        if (touches.Count == 0) return;

        Vector2 delta = Vector2.zero;
        foreach (Touch touch in touches)
        {
            delta += touch.deltaPosition;
        }

        Vector2 viewportDelta = Camera.main.ScreenToViewportPoint(new Vector3(delta.x, delta.y, 0));

        Vector3 newPosition = Camera.main.transform.position - new Vector3(viewportDelta.x * _dragSpeed, 0, 0);

        newPosition.x = Mathf.Clamp(newPosition.x, _leftX, _rightX);

        Camera.main.transform.position = newPosition;
    }
}
