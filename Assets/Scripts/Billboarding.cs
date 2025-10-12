    using UnityEngine;

    public class Billboard : MonoBehaviour
    {
        private Transform mainCameraTransform;

        void Start()
        {
            mainCameraTransform = Camera.main.transform; // Get reference to the main camera
        }


        /// <summary>
        /// LateUpdate() will be called AFTER all update calls.
        /// </summary>
        void LateUpdate()
        {
            // Option 1: Full billboarding (object rotates to face camera completely)
            //transform.LookAt(mainCameraTransform);

            // Option 2: Y-axis only billboarding (object stays upright, only rotates horizontally)
            // use this if you want the object to remain upright.
            Vector3 lookAtCamera = mainCameraTransform.position - transform.position;
            lookAtCamera.y = 0; // Lock Y-axis rotation
            transform.rotation = Quaternion.LookRotation(lookAtCamera);
        }
    }