using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

      // ============================================================
      // STEP 1: CAMERA SETTINGS
      // ============================================================

      // Particle effect that plays when the camera zooms out
      // because the player's speed increases.
      [SerializeField] ParticleSystem speedupParticleSystem;

      // Minimum allowed Camera Field of View.
      [SerializeField] float minFOV = 20f;

      // Maximum allowed Camera Field of View.
      [SerializeField] float maxFOV = 120f;

      // How long the camera should take to change its FOV.
      // 1f = 1 second.
      [SerializeField] float zoomCameraDuraion = 1f;

      // Controls how much the speed change affects the FOV.
      //
      // Example:
      // speedAmount = 2
      // zoomSpeedModifier = 5
      //
      // FOV change = 2 × 5 = 10
      [SerializeField] float zoomSpeedModifier = 5f;

      // Store a reference to the Cinemachine Camera.
      CinemachineCamera myCinemachineCamera;


      void Awake()
      {
            // Get the CinemachineCamera component attached
            // to this GameObject.
            //
            // GetComponent<T>() means:
            // "Find component T on this same GameObject."
            myCinemachineCamera = GetComponent<CinemachineCamera>();
      }

      // ============================================================
      // STEP 2: START CAMERA FOV CHANGE
      // ============================================================

      // Public method:
      // Other scripts can call this method to change the camera FOV.
      //
      // speedAmount tells us how much the speed changed.
      public void ChangeCameraFOV(float speedAmount)
      {
            // Stop any previous FOV changing Coroutine.
            //
            // Why?
            // If another FOV change is already running,
            // we don't want multiple Coroutines changing the FOV
            // at the same time.
            StopAllCoroutines();

            // Start a new Coroutine to smoothly change the FOV.
            StartCoroutine(ChangeFOVRoutine(speedAmount));

            // If speedAmount is positive,
            // it means the speed increased.
            if (speedAmount > 0)
            {
                  // Play the speed-up particle effect.
                  speedupParticleSystem.Play();
            }
      }

      // ============================================================
      // STEP 3: SMOOTHLY CHANGE THE CAMERA FOV
      // ============================================================
      IEnumerator ChangeFOVRoutine(float speedAmount)
      {
            // Get the camera's current Field of View.
            //
            // Example:
            // Current FOV = 60
            float currentFOV = myCinemachineCamera.Lens.FieldOfView;

            // Calculate the desired FOV.
            //
            // speedAmount × zoomSpeedModifier
            // gives us how much the FOV should change.
            //
            // Mathf.Clamp() keeps the final FOV
            // between minFOV and maxFOV.
            //
            // Example:
            // currentFOV = 60
            // speedAmount = 2
            // modifier = 5
            //
            // 60 + (2 × 5)
            // = 70
            //
            // targetFOV = 70
            float targetFOV = Mathf.Clamp(currentFOV + speedAmount * zoomSpeedModifier, minFOV, maxFOV);

            // Keep track of how much time has passed.
            float elapsedTime = 0f;


            // ========================================================
            // STEP 4: SMOOTH TRANSITION
            // ========================================================

            // Continue changing the FOV
            // until the desired duration is completed.
            while (elapsedTime < zoomCameraDuraion)
            {
                  // Convert elapsed time into a value between 0 and 1.
                  //
                  // Example:
                  // elapsedTime = 0.5
                  // duration = 1
                  //
                  // t = 0.5
                  //
                  // t tells Lerp how far we are in the transition.
                  float t = elapsedTime / zoomCameraDuraion;

                  // Increase elapsed time every frame.
                  elapsedTime += Time.deltaTime;


                  // LERP CONCEPT:
                  //
                  // Mathf.Lerp(start, end, t)
                  //
                  // smoothly moves a value from start to end.
                  //
                  // currentFOV → targetFOV
                  // t = 0 → currentFOV
                  // t = 1 → targetFOV
                  //
                  // This creates a smooth camera zoom.
                  myCinemachineCamera.Lens.FieldOfView = Mathf.Lerp(currentFOV, targetFOV, t);

                  // Wait for the next frame.
                  //
                  // This allows the Coroutine to continue
                  // instead of finishing immediately.
                  yield return null;
            }

            // After the loop finishes,
            // make sure the FOV is exactly the target value.
            myCinemachineCamera.Lens.FieldOfView = targetFOV;


      }

}
