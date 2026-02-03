using UnityEngine;

public class TapNote : NoteBase
{
    public override void OnKeyPressed()
    {
        Health.Regen();
        UltimateSystem.IncrementUltimate(); //increments the ults progression bar
        Destroy(gameObject);
        // add more functionality later, like scoring, etc
    }
}