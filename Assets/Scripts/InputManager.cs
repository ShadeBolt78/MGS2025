using UnityEngine;
using System;
using UnityEngine.InputSystem;

/// <summary>
/// The InputManager manages all controls, inputs, and input events.
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; } // [singleton]

    private RhythmControls controls; // RhythmControls input system file [editorGenerated]

    public event Action<int> OnLaneKeyPressed; // lane keypressed flag [event]

    private void Awake()
    {
        if (Instance != null && Instance != this) // there can only be one.
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        controls = new RhythmControls(); // instantiate
    }

    private void OnEnable()
    {
        controls.Enable();
        BindLaneInputs();
    }

    private void OnDisable()
    {
        controls.Disable();
        UnbindLaneInputs();
    }

    /// <summary>
    /// All this does is take the binding from the input system and calls each lanes OnKeyPress function when the bind is pressed.
    /// The indexing is done for security and to remove some headaches for me later.
    /// This is technically hardcoding, but it's fine because its keybinds.
    /// </summary>
    private void BindLaneInputs()
    {
        controls.Gameplay.Lane1.performed += ctx => OnLaneKeyPressed?.Invoke(0);
        controls.Gameplay.Lane2.performed += ctx => OnLaneKeyPressed?.Invoke(1);
        controls.Gameplay.Lane3.performed += ctx => OnLaneKeyPressed?.Invoke(2);
        controls.Gameplay.Lane4.performed += ctx => OnLaneKeyPressed?.Invoke(3);
        controls.Gameplay.Lane5.performed += ctx => OnLaneKeyPressed?.Invoke(4);
    }

    private void UnbindLaneInputs()
    {
        controls.Gameplay.Lane1.performed -= ctx => OnLaneKeyPressed?.Invoke(0);
        controls.Gameplay.Lane2.performed -= ctx => OnLaneKeyPressed?.Invoke(1);
        controls.Gameplay.Lane3.performed -= ctx => OnLaneKeyPressed?.Invoke(2);
        controls.Gameplay.Lane4.performed -= ctx => OnLaneKeyPressed?.Invoke(3);
        controls.Gameplay.Lane5.performed -= ctx => OnLaneKeyPressed?.Invoke(4);
    }
}
