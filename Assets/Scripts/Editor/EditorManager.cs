using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class EditorManager : MonoBehaviour
{
    EditorManager Instance;

    // stuff from GM because it'll be annoying to reference GameManager.Instance.whatever every time, only for public fields tho
    // they should all be there since base is loaded first, but if it causes issues these can be removed.
    // not exactly sure if all of these will be utilized but I thought it was better to have them for now jic
    private Transform leftSpawnZone;

    public string beatmapFileName;
    private BeatmapData beatmap;
    // How far into the beat map we are
    private float trackTime = 0;

    public LanePrefabController[] lanes;
    public AudioSource audioSource;
    public Dictionary<string, GameObject> notePrefabs;

    public GameObject tapNotePrefab;
    public GameObject holdNotePrefab;
    public GameObject deadNotePrefab;

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
        if (GameManager.Instance != null)
        {
            leftSpawnZone = GameManager.Instance.hitZone;
            beatmapFileName = GameManager.Instance.beatmapFileName;
            lanes = GameManager.Instance.lanes;
            audioSource = GameManager.Instance.audioSource;
            notePrefabs = new Dictionary<string, GameObject>(GameManager.Instance.notePrefabs);
            tapNotePrefab = GameManager.Instance.tapNotePrefab;
            holdNotePrefab = GameManager.Instance.holdNotePrefab;
            deadNotePrefab = GameManager.Instance.deadNotePrefab;
        }
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
        catch (InvalidOperationException e) // if this exception fired, theres no note
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
        var step = LanePrefabController.step * (side == LanePrefabController.Side.Left ? -1 : 1);
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
}
