using UnityEngine;

/// <summary>
/// Singleton exposing GameObjects in the `Base` scene to the Game and Editor Managers,
/// reponsible for loading Note prefabs and making those available as well
/// </summary>
public class BaseManager : MonoBehaviour
{
    public static BaseManager Instance { get; private set; }

    [Header("Beatmap")]
    public string beatmapFileName = "fuckoff.json"; // A file name in /StreamingAssets/Beatmaps
    public BeatmapData beatmap; // the beatmap to run

    [Header("Hooks")]
    public LanePrefabController[] lanes; // Lane hooks (inspector)
    public AudioSource audioSource; // audio hook (inspector)

    private void Awake()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "Beatmaps", beatmapFileName); // makes a nice readble path to the beatmap
        beatmap = BeatmapData.Load(path);
        if (beatmap == null)
        {
            Debug.LogError("Failed to load beatmap. Abort!");
            enabled = false;
            return; // basically just tell it to break to avoid any loops
        }
        Instance = this;
    }
}
