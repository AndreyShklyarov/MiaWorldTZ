using UnityEngine;
using Zenject;

public class GameEntryPoint : MonoBehaviour
{
    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Application.targetFrameRate = 60;
        _inputManager.Init();
    }
}
