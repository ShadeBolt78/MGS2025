using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Menu References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private BaseMenu activeMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        var input = InputManager.Instance;

        input.OnPausePressed += TogglePause;
        input.OnNavigate += HandleNavigate;
        input.OnSubmit += HandleSubmit;
        input.OnCancel += HandleCancel;
    }

    private void OnDisable()
    {
        if (InputManager.Instance == null) return;
        var input = InputManager.Instance;

        input.OnPausePressed -= TogglePause;
        input.OnNavigate -= HandleNavigate;
        input.OnSubmit -= HandleSubmit;
        input.OnCancel -= HandleCancel;
    }

    private void TogglePause()
    {
        if (activeMenu == null)
        {
            OpenMenu(pauseMenu);
        }
        else
        {
            CloseMenu();
        }
    }

    private void OpenMenu(GameObject menuPrefab)
    {
        if (activeMenu != null) CloseMenu();

        activeMenu = menuPrefab.GetComponent<BaseMenu>();
        if (activeMenu == null)
        {
            Debug.LogError($"Menu prefab {menuPrefab.name} is missing a BaseMenu component.");
            return;
        }

        activeMenu.Open();
        InputManager.Instance.EnableUI();
    }

    public void CloseMenu()
    {
        if (activeMenu == null) return;

        activeMenu.Close();
        activeMenu = null;
        InputManager.Instance.EnableGameplay();
    }

    private void HandleNavigate(Vector2 direction)
    {
        activeMenu?.HandleNavigate(direction);
    }

    private void HandleSubmit()
    {
        activeMenu?.HandleSubmit();
    }

    private void HandleCancel()
    {
        if (activeMenu != null)
        {
            activeMenu.HandleCancel();
        }
        else
        {
            TogglePause(); // fallback — close menu or open pause
        }
    }
}
