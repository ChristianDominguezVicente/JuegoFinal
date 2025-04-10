using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputManager")]
public class InputManagerSO : ScriptableObject
{
    Controls misControles;
    public event Action OnSaltar;
    public event Action<Vector2> OnMover;
    public event Action OnInteract;
    private void OnEnable()
    {
        misControles = new Controls();
        misControles.Gameplay.Enable();
        misControles.Gameplay.Saltar.started += Saltar;
        misControles.Gameplay.Mover.performed += Mover;
        misControles.Gameplay.Mover.canceled += Mover;
        misControles.Gameplay.Interact.started += Interact;
    }

    private void Mover(InputAction.CallbackContext ctx)
    {
        OnMover?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void Saltar(InputAction.CallbackContext ctx)
    {
        OnSaltar?.Invoke();
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }
}
