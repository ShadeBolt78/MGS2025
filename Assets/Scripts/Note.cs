using UnityEngine;

/// <summary>
/// A Note is a little object that runs down a lane until it gets to a hitzone, at which point it gets killed or dies on its own.
/// </summary>
public class Note : MonoBehaviour
{
    private float speed; // the speed at which the note moves (assigned on instantiation)
    private LaneController lane; // the lane it's assigned (assigned on instantiation)

    /// <summary>
    /// Initialize(LaneController, float) is suprisingly not a Mono method and is literally a workaround because
    /// Mono's can't have constructors. This is called in LaneController on note instantiation.
    /// </summary>
    /// <param name="lane">The lane the note belongs to.</param>
    /// <param name="speed">The speed the note moves at down the lane.</param>
    public void Initialize(LaneController lane, float speed)
    {
        this.lane = lane; // setters
        this.speed = speed;
    }

    /// <summary>
    /// Update() is called every frame. 
    /// </summary>
    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime; // translate its ass down the lane.
    }

    /// <summary>
    /// IsInHitZone(Transform) determines whether the note is within the confines of a hitzone.
    /// </summary>
    /// <param name="hitZone">The hitzone the note will possibly be in.</param>
    /// <returns>Boolean whether the note is in the hitzone or not.</returns>
    public bool IsInHitZone(Transform hitZone)
    {
        float distance = Mathf.Abs(transform.position.x - hitZone.position.x); // calc the distance to the hitzone.
        return distance < 0.5f; // basic timing window
    }

    /// <summary>
    /// Hit() is called probably by LaneController that tells the note it got hit.
    /// </summary>
    public void Hit()
    {
        // Debug.Log($"Lane {lane.laneIndex}: Note hit!"); // a debug statement prints whatevers in it to the unity console.
        Destroy(gameObject); // it dieded :(
        // gameObject is a Monobehavior thing. It's the reference to the gameObject object the engine uses, all mono's have one.

        // add score, whatever else here when a note is hit. 
    }

    /// <summary>
    /// Miss () is called in KilLZone(), when the colliders detect a trigger collision
    /// Miss() is called here for organization.
    /// </summary>
    public void Miss()
    {
        Destroy(gameObject);
    }
}
