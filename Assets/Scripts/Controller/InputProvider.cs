using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    public bool Clicked { get; private set; }

    public void Update()
    {
        Clicked = Mouse.current.leftButton.wasPressedThisFrame;
    }
}
