using UnityEngine;
using System.Linq;
using System;

public class EditorManager : MonoBehaviour
{
    public static EditorManager Instance { get; private set; }

    public string beatmapFileName;
    private BeatmapData beatmap;
    // How far into the beat map we are
    private float trackTime = 0;

    public LanePrefabController[] lanes;
    public AudioSource audioSource;

    private int lane = 0;
    private float? addingHoldNote;
    private string selectedNoteType = "Tap";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        // remove if not used/needed
        if (BaseManager.Instance != null)
        {
            beatmapFileName = BaseManager.Instance.beatmapFileName;
            beatmap = BaseManager.Instance.beatmap;
            lanes = BaseManager.Instance.lanes;
            audioSource = BaseManager.Instance.audioSource;
        }
        else
            Debug.LogError("BaseManager singleton has not been instantiated. Did you forget to load the Base scene?");
    }


    public void PlayTrack() { }
    public void PauseTrack() { }
    public void SetTrackTime(float playbackPercent)
    {
        trackTime = playbackPercent;
    }
    // add functions here!

    public void SelectLane(int index)
    {
        if (index < 0 || index > 4)
        {
            Debug.LogError($"Refusing to select non-existent lane with index {index}");
            return;
        }
        lane = index;
    }

    public void SelectNoteType(string noteType)
    {
        switch (noteType)
        {
            case "Tap":
            case "Hold":
            case "Dead":
                selectedNoteType = noteType;
                break;
            default:
                Debug.LogError($"Invalid note type: {noteType}");
                break;
        }
    }

    public void Edit()
    {
        var selected = lanes[lane];

        if (addingHoldNote is not null)
        {
            // If we are in the middle of adding a HoldNote, end and add the HoldNote
            addingHoldNote = null;
            selected.EndSpawnSpecial(ref beatmap);
            return;
        }
        try
        {
            // This logic will probably require improvement later
            beatmap.notes.Remove(selected.GetComponents<NoteBase>()
                    .Where(note => note.transform.position.x == selected.specialZone.position.x)
                    .First()
                    .Decay());
            beatmap.Save();
            return;
        }
        catch (Exception e) when (e is InvalidOperationException || e is NullReferenceException) // if this exception fired, theres no note
        {
            _ = e;
        }

        // If we're still running, neither of the two blocks succeeded, so we should add a note
        if (selectedNoteType == "Hold")
            addingHoldNote = trackTime;
        selected.BeginSpawnSpecial(selectedNoteType, ref beatmap);
    }

    public void Scroll(LanePrefabController.Side side)
    {
        var step = LanePrefabController.stepSize * (side == LanePrefabController.Side.Left ? -1 : 1);
        // Don't allow the timeline to go further left of where we've already started placing a hold
        // note
        if (addingHoldNote is not null && trackTime + step <= addingHoldNote)
        {
            Debug.Log("refusing to scroll past start of pending HoldNote");
            return;
        }

        trackTime += step;
        foreach (var lane in lanes)
        {
            lane.Scroll(side);
            if (lane.Offset != trackTime)
            {
                Debug.LogError(
                    $"EditorManager and LanePrefabController ({lane.laneIndex}) are out of sync\n" +
                    $"EditorManager is at {trackTime}, LanePrefabController is at {lane.Offset}\n"
                );
            }
        }
    }

    // For referencing in the inspector
    public void ScrollLeft()
    {
        Scroll(LanePrefabController.Side.Left);
    }
    public void ScrollRight()
    {
        Scroll(LanePrefabController.Side.Right);
    }
    public void Back()
    {
        BaseManager.MainMenu();
    }
}
