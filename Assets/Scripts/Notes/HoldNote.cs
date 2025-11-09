using System.Drawing;
using Unity.Properties;
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
    private float totalTime; // the amount of time of the note in seconds
    private float noteLength; // the physical length of the note (dependant on speed)

    private Transform pivotTransform; // reference to the pivot's transform for proper stretching

    public void Start()
    {
        pivotTransform = transform.Find("Note Pivot");

        totalTime = data.parameters["endTime"] - data.time;
        noteLength = totalTime * speed;

        pivotTransform.localScale += Vector3.right * noteLength - Vector3.right; // stretching the note to represent its length
    }

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

        if (hasStarted && isHolding)
        {
            if (InputManager.Instance.IsLaneHeld(lane.laneIndex))
            {
                holdTimer += Time.deltaTime;

                pivotTransform.localScale = new Vector3(noteLength - ((holdTimer) / totalTime) * speed, 1f, 1f); // shrinks it when it is being held
                transform.position += Vector3.right * speed * Time.deltaTime; // keeps the front of the note in place

                if (holdTimer >= (data.parameters["endTime"] - data.time))
                {
                    Destroy(gameObject);
                    // successful note completion (might want to add something to NoteBase for this)
                }
            }

        }
        else if (hasStarted && !isHolding)
        {
            Destroy(gameObject);
            // hold released early, do any penalties before the destroy statement (same for success)
            // doesnt really do much because there is no "let go of button" checking
        }
    }
}