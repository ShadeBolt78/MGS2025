using UnityEngine;

/// <summary>
/// A BeatmapPlayer handles the logic to run a beatmap.
/// TIS NOT A MONOBEHAVIOR SHOULD ONE LOOK CLOSELY
/// </summary>
public class BeatmapPlayer
{
    // listen, comments get to a point.
    private BeatmapData beatmap; // the beatmap to run (GM)
    private LaneController[] lanes; // lanes (GM)
    private float noteSpeed; // (GM)
    private float noteTravelDistance; // (GM)

    private int nextNoteIndex = 0; // what note we're gonna be on

    /// <summary>
    /// Good old constructor
    /// </summary>
    public BeatmapPlayer(BeatmapData beatmap, LaneController[] lanes, float noteSpeed)
    {
        this.beatmap = beatmap;
        this.lanes = lanes;
        this.noteSpeed = noteSpeed;

        foreach(var lane in lanes)
        {
            lane.noteSpeed = noteSpeed;
            this.noteTravelDistance = lane.hitZone.position.x - lane.spawnPoint.position.x;
        }
    }


    public void Update(float songTime)
    {
        float spawnLeadTime = noteTravelDistance / noteSpeed; // calc the time it takes for the note to cross the distance at its speed.

        // while there are still beats to be made AND the next beat time is less than the current song time plus the freshly calculated lead time
        while (nextNoteIndex < beatmap.notes.Count && beatmap.notes[nextNoteIndex].time <= songTime + spawnLeadTime)
        {
            var data = beatmap.notes[nextNoteIndex]; // grab note data
            if (data.lane >= 0 && data.lane < lanes.Length) // if the note is in a valid lane
            {
                lanes[data.lane].SpawnNote(); // spawn that shit
            }
            nextNoteIndex++; // move on to the next one
        }
    }
}
