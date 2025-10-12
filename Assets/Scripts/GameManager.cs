using UnityEngine;

/// <summary>
/// "That is my power. The Almighty." - Yhwach
/// The Game Manager does an assortment of things, mainly variable tracking and hooking.
/// 
/// YES! I even documented my code!
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // singleton reference. There can only be one.

    [Header("Beat Settings")]
    public float bpm = 120f; // the BPM to a given song, for the MVP, its 120 default
    private float secondsPerBeat; // assigned in Awake()
    private float timer; // per instance timer. "but what about Time.time" shut up.

    [Header("Lanes")]
    public LaneController[] lanes; // Lane hooks (inspector)

    /// <summary>
    /// Awake() is a Monobehavior method, it is run before the first frame after object load and all Start() methods.
    /// </summary>
    private void Awake()
    {
        Instance = this; // Assign singleton reference
        secondsPerBeat = 60f / bpm; // Seconds in each beat is just the bpm converted to seconds reciprocal.
    }

    /// <summary>
    /// Update() is called every frame. 
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime; // delta time is is tracked by the engine, so it is unaffected by frame rate
        // delta time returns the time between the last frame and the current
        if (timer >= secondsPerBeat) // procs every mathematical beat time
        {
            timer -= secondsPerBeat;
            // everything above allows the timer to track interim time values between beats
            foreach (var lane in lanes)
            {
                lane.SpawnNote(); // fire a note on beat time.
            }
        }
    }
}
