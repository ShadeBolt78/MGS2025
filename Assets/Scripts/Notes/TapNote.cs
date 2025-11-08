using UnityEngine;

public class TapNote : NoteBase
{
    public override void OnKeyPressed()
    {
        Health.Regen();
        Destroy(gameObject);
        // add more functionality later, like scoring, etc
    }
}