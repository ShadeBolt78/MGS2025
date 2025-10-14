using UnityEngine;

/// <summary>
/// Rotates the object so it always faces the main camera.
/// Optionally locks specific rotation axes.
/// Can be attatched to any 2d object with a sprite.
/// This is a simple script im not docing it fuck you.
/// </summary>
[ExecuteAlways]
public class Billboard2D : MonoBehaviour
{
    private Transform mainCam;

    [Header("Freeze Rotation Axes")]
    public bool freezeX = false;
    public bool freezeY = false;
    public bool freezeZ = false;

    [Header("Tweaks")]
    public bool sweetCrispXTweak = false;

    private void Start()
    {
        if (Camera.main != null)
            mainCam = Camera.main.transform;
        else
            Debug.LogWarning("Billboard2D: No main camera found in scene");
    }
    
    /// <summary>
    /// LateUpdate() is called after every Update() in all objects to be called this frame.
    /// </summary>
    private void LateUpdate()
    {
        // Always face the camera
        Vector3 directionToCamera = mainCam.transform.forward * -1f; // opposite of camera forward
        Quaternion lookRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
        Vector3 euler = lookRotation.eulerAngles; // AAAHHH EULERS AAAA

        // Apply axis freezing
        Vector3 currentEuler = transform.rotation.eulerAngles;
        if (freezeX) euler.x = currentEuler.x;
        if (sweetCrispXTweak) euler.x += 90; 
        // this is here because sometimes depending on parenting the direction calcs make a 2d object face
        // the camera not "2d head on" but rather "3d head on" because theres actually no such thing as 2d LMAO
        if (freezeY) euler.y = currentEuler.y;
        if (freezeZ) euler.z = currentEuler.z;

        transform.rotation = Quaternion.Euler(euler);
    }
}
