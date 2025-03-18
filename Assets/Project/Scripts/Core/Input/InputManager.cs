using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputGame _inputGame;
    [SerializeField] private InputHandler _inputHandler;

    public InputGame InputGame => _inputGame;
    public InputHandler InputHandler => _inputHandler;

    public void Init()
    {
        _inputGame.Init(this);
        _inputHandler.Init(this);
    }
}
