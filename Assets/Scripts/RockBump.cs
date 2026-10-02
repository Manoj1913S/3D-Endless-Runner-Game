using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Android; //import this 

public class RockBump : MonoBehaviour
{
    // Particle effect played at the exact point where the rock is hit.
    [SerializeField] ParticleSystem rockCollisionParticleSystem;

    // Sound played when the rock is hit.
    [SerializeField] AudioSource rockCollisionAudio;

    // Controls how strong the camera shake can be.
    [SerializeField] float shakeModifier = 10f;

    // Minimum time required between two rock collisions.
    // Example:
    // 1 second cooldown means another collision
    // cannot trigger the effect immediately.
    [SerializeField] float rockCollisionCooldownTimer = 1f;

    // Reference to CinemachineImpulseSource.
    // This component creates an impulse/shake signal
    // that the Cinemachine camera can react to.
    CinemachineImpulseSource cinemachineImpulseSource;

    // Keeps track of how much time has passed since
    // the last collision effect.
    //
    // Starting at 1 means the rock can trigger immediately
    // because cooldown is already considered completed.
    float collisionCooldownTimeTracker = 1f;


    void Awake()
    {
        // GetComponent<T>() searches for the specified component
        // on the SAME GameObject where this script is attached.
        // Example:
        // Rock GameObject
        //      ↓
        // RockBump script
        //      ↓
        // CinemachineImpulseSource component
        // Store that component reference so we can use it later.
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

    }

    void Update()
    {
        // Time.deltaTime = time passed since the previous frame.
        // Adding deltaTime makes the timer work based on real time,
        // not on the number of frames.
        // Example:
        // After 0.5 seconds → tracker ≈ 0.5
        // After 1 second   → tracker ≈ 1
        collisionCooldownTimeTracker += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        // OnCollisionEnter() is automatically called by Unity
        // when this object physically collides with another object.
        // "other" contains collision information such as:
        // - The other GameObject
        // - Contact points
        // - Collision direction, etc.
        // Check whether the cooldown has finished.
        // Example:
        // tracker = 0.5
        // cooldown = 1
        // 0.5 < 1 → TRUE
        // return → stop this method immediately.
        // This prevents the rock from triggering
        // the effects repeatedly in a very short time.
        if (collisionCooldownTimeTracker < rockCollisionCooldownTimer) return;

        // Calculate and generate the camera shake.                   
        SceneImpulseWhenRockBump();



        // Pass the Collision information to this method.
        // WHY pass "other"?
        // Because the collision point is stored inside
        // the Collision object.
        //
        // RockBump needs that point to place the particle
        // exactly where the rock was hit.
        RockCollisionFX(other);

        // Reset the timer after a valid collision.
        //
        // Now:
        // tracker = 0
        //
        // Another collision must wait until
        // tracker reaches 1 second again.
        collisionCooldownTimeTracker = 0f;

    }

    void SceneImpulseWhenRockBump()
    {
        // Vector3.Distance() calculates the distance
        // between two positions.
        // Position A → Rock position
        // Position B → Main Camera position
        // Example:
        // Rock is 5 units away from camera
        // distance = 5

        //Rock ko position bhaneko ho yo transform.position le
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        // Calculate camera shake intensity based on distance.
        // Formula:
        // intensity = (1 / distance) × shakeModifier
        // IMPORTANT:
        // Smaller distance → stronger shake
        // Larger distance  → weaker shake
        // Example:
        // distance = 5
        // modifier = 10
        // intensity = (1 / 5) × 10
        //           = 2
        float shakeIntensity = (1f / distance) * shakeModifier;

        // Mathf.Min() returns the SMALLER of two values.
        //
        // Here we are limiting shakeIntensity to maximum 1.
        //
        // Example:
        // intensity = 2
        // Min(2, 1) → 1
        //
        // intensity = 0.5
        // Min(0.5, 1) → 0.5
        shakeIntensity = Mathf.Min(shakeIntensity, 1f);

        // Generate an impulse signal.
        // Cinemachine receives this impulse and
        // the configured camera reacts by shaking.
        cinemachineImpulseSource.GenerateImpulse();
    }

    void RockCollisionFX(Collision other)
    {
        // Collision.contacts contains the points where
        // the two objects collided.
        // [0] means we take the FIRST contact point.
        // Array/List indexing starts from 0.
        ContactPoint contactPoint = other.contacts[0];

        // contactPoint.point gives the exact WORLD position
        // where the collision happened.
        // Move the particle system to that position
        // before playing it.
        rockCollisionParticleSystem.transform.position = contactPoint.point;

        // Play the rock collision particle effect.
        rockCollisionParticleSystem.Play();

        // Play the collision sound.
        rockCollisionAudio.Play();
    }
}
