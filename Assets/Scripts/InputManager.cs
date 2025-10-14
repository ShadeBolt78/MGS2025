using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// if you cant read this without comments maybe leave this one alone
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Input Settings")]
    [Tooltip("Input Action Asset containing the 'Gameplay' action map.")]
    [SerializeField] private InputActionAsset inputAsset;

    private InputActionMap gameplayMap;

    // Stores all lane actions: lane index → InputAction
    private readonly Dictionary<int, InputAction> laneActions = new();

    // Lane press/release events
    public event Action<int> OnLanePressed;
    public event Action<int> OnLaneReleased;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGameplayMap();
    }

    private void OnEnable() => gameplayMap?.Enable();
    private void OnDisable() => gameplayMap?.Disable();

    private void InitializeGameplayMap()
    {
        gameplayMap = inputAsset.FindActionMap("Gameplay", throwIfNotFound: true);
        laneActions.Clear();

        // Auto-detect any actions named "Lane0", "Lane1", etc.
        int i = 0;
        while (true)
        {
            var action = gameplayMap.FindAction($"Lane{i}");
            if (action == null) break;

            int laneIndex = i; // Capture variable for closure
            laneActions[laneIndex] = action;

            // Subscribe events
            action.performed += ctx => OnLanePressed?.Invoke(laneIndex);
            action.canceled += ctx => OnLaneReleased?.Invoke(laneIndex);

            i++;
        }

        Debug.Log($"ControlsManager initialized with {laneActions.Count} lanes.");
    }

    /// <summary>
    /// Returns the InputAction for a given lane index.
    /// </summary>
    public InputAction GetLaneAction(int laneIndex)
    {
        return laneActions.TryGetValue(laneIndex, out var action) ? action : null;
    }

    /// <summary>
    /// Returns true if the lane key is currently being held.
    /// </summary>
    public bool IsLaneHeld(int laneIndex)
    {
        var action = GetLaneAction(laneIndex);
        return action != null && action.ReadValue<float>() > 0.5f;
    }

    /// <summary>
    /// Start a runtime rebind for a specific lane.
    /// </summary>
    public void StartRebind(int laneIndex, Action onComplete = null)
    {
        var action = GetLaneAction(laneIndex);
        if (action == null)
        {
            Debug.LogWarning($"No action found for lane {laneIndex}");
            return;
        }

        action.Disable();

        var rebind = action.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>") // optional
            .OnComplete(ctx =>
            {
                ctx.Dispose();
                action.Enable();
                onComplete?.Invoke();
                Debug.Log($"Rebound Lane {laneIndex} to {action.bindings[0].effectivePath}");
            });

        rebind.Start();
    }
}
