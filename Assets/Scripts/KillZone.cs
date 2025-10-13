using UnityEngine;

public class KillZone : MonoBehaviour
{
    /// <summary>
    /// OnTriggerEnter will fire whenever an object with a Collider has another collider enter it.
    /// This will only fire if the isTrigger flag is set to true on THIS object.
    /// </summary>
    /// <param name="other">The other collider that entered this one</param>
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Note>().Miss();
        //Destroy(other.gameObject); // destroy note as it enters.
    }
}