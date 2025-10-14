using UnityEngine;

public class TapNote : NoteBase
{
    public override void OnKeyPressed()
    {
        Destroy(gameObject);
        // add more functionality later, like scoring, etc
    }
}