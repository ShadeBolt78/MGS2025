using UnityEngine;

/// <summary>
/// A NoteBase is the abstraction of basic functionality of a hittable object that spawns at a given time in a lane.
/// If you need an explanation on abstraction, you are far better off googling it, however I will answer any questions.
/// </summary>
public abstract class NoteBase : MonoBehaviour
{
    protected LanePrefabController lane; // the lane it's assigned (assigned on instantiation)
    protected BeatmapData.NoteData data; // any other args to be passed to children of the appropriate type (instantiation)

    // Remove itself from the engine world, then return the data it holds
    public BeatmapData.NoteData Decay()
    {
        Destroy(this);
        return data;
    }


    public bool testMove = true;
    /// <summary>
    /// Initialize(LaneController, float) is suprisingly not a Mono method and is literally a workaround because
    /// Mono's can't have constructors. This is called in LaneController on note instantiation.
    /// </summary>
    /// <param name="lane">The lane the note belongs to.</param>
    public virtual void Initialize(LanePrefabController lane, BeatmapData.NoteData data)
    {
        this.lane = lane; // setters
        this.data = data;
    }

    /// <summary>
    /// Update() is called every frame. 
    /// </summary>
    protected virtual void Update()
    {
        if (testMove)
            lane.Scroll(LanePrefabController.Side.Left); // translate its ass down the lane.
    }

    /// <summary>
    /// IsInHitZone(Transform) determines whether the note is within the confines of a hitzone.
    /// </summary>
    /// <param name="hitZone">The hitzone the note will possibly be in.</param>
    /// <returns>Boolean whether the note is in the hitzone or not.</returns>
    public virtual bool IsInHitZone(Transform hitZone)
    {
        var timing = Mathf.Abs(transform.position.x - hitZone.position.x); // this will need to be reworked since speed dosen't exist anymore
                                                                           // you also could use some sort of curve instead of it being linear perhaps
        ScoreManager.Instance.AddScore(timing);
        return timing < 0.4f; // if timing smaller than largest timing window // actually maybe just adjust this
    }

    public virtual void Miss()
    {
        data.resolved = true;
        AnimationManager.Missed(lane);
        Health.TakeDamage();
        Destroy(gameObject);
    }
    public abstract void OnKeyPressed();

    // Used for when note is hit
    public void ResolveNote()
    {
        data.resolved = true;
    }
}
