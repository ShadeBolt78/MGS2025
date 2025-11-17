using UnityEngine;

/// <summary>
/// A NoteBase is the abstraction of basic functionality of a hittable object that spawns at a given time in a lane.
/// If you need an explanation on abstraction, you are far better off googling it, however I will answer any questions.
/// </summary>
public abstract class NoteBase : MonoBehaviour
{
    protected float speed; // the speed at which the note moves (assigned on instantiation)
    protected LaneController lane; // the lane it's assigned (assigned on instantiation)
    protected BeatmapData.NoteData data; // any other args to be passed to children of the appropriate type (instantiation)
    public Sprite sprNote;

    /// <summary>
    /// Initialize(LaneController, float) is suprisingly not a Mono method and is literally a workaround because
    /// Mono's can't have constructors. This is called in LaneController on note instantiation.
    /// </summary>
    /// <param name="lane">The lane the note belongs to.</param>
    /// <param name="speed">The speed the note moves at down the lane.</param>
    public virtual void Initialize(LaneController lane, float speed, BeatmapData.NoteData data)
    {
        this.lane = lane; // setters
        this.speed = speed;
        this.data = data;
       // sprNote = Resources.Load<Sprite>("Assets/Art/DEV/drum-placeholder.png");
       // gameObject.GetComponent<SpriteRenderer>().sprite = sprNote;
    }

    /// <summary>
    /// Update() is called every frame. 
    /// </summary>
    protected virtual void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime; // translate its ass down the lane.
    }

    /// <summary>
    /// IsInHitZone(Transform) determines whether the note is within the confines of a hitzone.
    /// </summary>
    /// <param name="hitZone">The hitzone the note will possibly be in.</param>
    /// <returns>Boolean whether the note is in the hitzone or not.</returns>
    public virtual bool IsInHitZone(Transform hitZone)
    {   
        // calc the distance to the hitzone.
        return Mathf.Abs(transform.position.x - hitZone.position.x) < 0.5f; // basic timing window
    }

    public virtual void Miss()
    {
        AnimationManager.Missed(this.lane);
        Health.TakeDamage();
        Destroy(gameObject);
    }
    public abstract void OnKeyPressed();
}
