using UnityEngine;

public class DeadNote : NoteBase
{
    public override void OnKeyPressed()
    {
        AnimationManager.Missed(this.lane);
        Health.TakeDamage(); // take damage when the note is hit
        Destroy(gameObject);
    }

    public override void Miss()
    {
        Health.Regen(); // regen health or add score when missed
        UltimateSystem.IncrementUltimate(); //increments the ults progression bar
        Destroy(gameObject);
    }
}
