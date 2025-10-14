using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A hold note is held down! I will make a csv example here, later i if i remember
/// </summary>
public class HoldNote : NoteBase
{
    private bool isHolding = false; // there are ways to do this throught the input system that i am choosing not to do
    private bool hasStarted = false;
    private float holdTimer = 0f;

    public override void OnKeyPressed()
    {
        if (!hasStarted && IsInHitZone(lane.hitZone))
        {
            isHolding = true;
            hasStarted = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        // *** DO NOT *** unprotect base.Update(). FOR ANY REASON! UNLESS CLEARED WITH ME!

        if (isHolding)
        {
            if (InputManager.Instance.IsLaneHeld(lane.laneIndex))
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= (data.parameters["endTime"] - data.time))
                {
                    Destroy(gameObject);
                    // successful note completion (might want to add something to NoteBase for this)
                }
            }

        }
        else
        {
            Destroy(gameObject);
            // hold released early, do any penalties before the destroy statement (same for success)
        }
    }
}