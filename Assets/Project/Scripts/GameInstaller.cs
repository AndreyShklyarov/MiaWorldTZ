using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private InputManager _inputManager;

    public override void InstallBindings()
    {
        Container.Bind<CameraController>().FromInstance(_cameraController).AsSingle();
        Container.Bind<InputManager>().FromInstance(_inputManager).AsSingle();
    }
}
