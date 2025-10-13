using UnityEngine;

/// <summary>
/// A LaneController manages it's lane, including note spawning and hit registration.
/// Multi instance (non singleton)
/// </summary>
public class LaneController : MonoBehaviour
{
    public int laneIndex; // technically an ID
    public GameObject notePrefab; // prefab (inspector) [hook]
    public Transform spawnPoint; // note spawn coords (inspector)
    public Transform hitZone; // note hit coords (GM)
    public float noteSpeed = 5f; // the speed at which notes move because accesibility.
    // yk they never talk about MY accesibility needs of not giving a fuck about this

    /// <summary>
    /// Start() is called ONCE on object enable.
    /// It is called BEFORE any Update() calls
    /// It is called AFTER the Awake() call
    /// Fun fact, has an overload for enumeration!
    /// </summary>
    private void Start()
    {
        // ill give you a description in the comment this time but going forward its gonna look like: OnKeyPress() -> OnLaneKeyPressed [subscription]
        InputManager.Instance.OnLaneKeyPressed += OnKeyPress; // subscribes OnKeyPress() (method in this script) to OnLaneKeyPressed Event
    }

    /// <summary>
    /// OnDestroy() is called when the object is removed from a scene, including when its scene is unloaded (important for later).
    /// </summary>
    private void OnDestroy()
    {
        InputManager.Instance.OnLaneKeyPressed -= OnKeyPress; // unsubs (see Start())
    }

    /// <summary>
    /// SpawnNote(), spawns a note!
    /// Spawns note from prefab assigned in inspector.
    /// </summary>
    public void SpawnNote()
    {
        GameObject noteObj = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity, transform); // instantiate note prefab
        noteObj.GetComponent<Note>().Initialize(this, noteSpeed); // this grabs the script set in the prefab and tells it some info to keep track of
    }

    /// <summary>
    /// OnKeyPress(int) is called whenever a key is pressed BY THE INPUTMANAGER USING EVENTS.
    /// See the InputManager for more info!
    /// </summary>
    /// <param name="lane">The lane that got proced</param>
    private void OnKeyPress(int lane)
    {
        if (lane != laneIndex) return; // fuck off if its not the lane we care about [checkCond]

        // Detect closest note in hit zone
        foreach (Transform child in transform) // for every note
        {
            Note note = child.GetComponent<Note>(); // grab script reference [grabRef]
            if (note != null && note.IsInHitZone(hitZone)) // if the note is not nothing (it happens) and the note thinks its in the hitzone
            {
                note.Hit(); // tell the note it hit
                break; // dont need to check the rest, semantically (and design wise) it is impossible for two notes to be in the same place.
            }
        }
    }
}
