using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A BeatmapData contains information about a beatmap!
/// For those who don't know JSON:
/// 1. Learn it! https://www.json.org/json-en.html
/// 2. JSON is a *FORMAT*, not a language. There are multiple frameworks that read the json format and translates that shit directly
/// into a pre set class that is api readable. This is one such class.
/// </summary>
[Serializable] // Serialization is a bit of a more advanced concept. basically, a serialized object is marked by unity to be in a format it can edit.
// theres way more, but for now, mark things as serializable if you need to edit them in the inspector and they don't pop up automatically like certain vars.
public class BeatmapData
{
    public string songName; // the name of the song [metadata]
    public string songPath; // directory path to the audio. Should be in Assets/StreamingAssets but that could change
    public float bpm; // the predetermined BPM of the song. Sound Design should be providing these to you.
    public string notesCsv; // Notes and timings [encoded(csv)]

    [NonSerialized] public List<NoteData> notes = new(); // raw note data [decodeTarget]

    [Serializable]
    public class NoteData // this is a helper class that just couples an int and float
    {
        public int lane; // the lane the note goes to
        public float time; // the time the note should be spawned
    }
    /// <summary>
    /// Parses the CSV-like string into structured note data.
    /// Expected format: "lane,time" per line.
    /// </summary>
    public void ParseCsv()
    {
        notes.Clear(); // wipe the list if its been used or data carried over for whatever reason
        if (string.IsNullOrEmpty(notesCsv)) // if theres nothing to decode, fuck out of here
            return;

        string[] lines = notesCsv.Split('\n', StringSplitOptions.RemoveEmptyEntries); // split by a delimiter \n and dont track anything empty
        foreach (string line in lines) // for every encoded note
        {
            string[] parts = line.Trim().Split(','); // get rid of any leading/trailing whitespace and split by delimiter ,
            if (parts.Length != 2) continue; // format checking

            // if you can't read this line I would just work on something else.
            if (int.TryParse(parts[0], out int lane) && float.TryParse(parts[1], out float time))
                notes.Add(new NoteData { lane = lane, time = time });
        }

        notes.Sort((a, b) => a.time.CompareTo(b.time)); // sorts notes based on time to appear so single iteration is possible
        // beatmaps should already be formatted as such but you just never know.
    }
}
