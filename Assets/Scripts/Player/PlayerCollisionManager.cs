using UnityEngine;

public class PlayerCollisionManager : MonoBehaviour
{
  // ============================================================
  // STEP 1: REFERENCES AND SETTINGS
  // ============================================================

  // Reference to the Animator component of the player.
  // We will use this to play the "Hit" animation.
  [SerializeField] Animator playerAnimator;

  // Minimum time required between two collision hits.
  // 1f = 1 second cooldown.
  [SerializeField] float collisionHitCooldown = 1f;

  // Amount by which the road/chunk speed should change after a hit.
  // -2 means decrease the speed by 2.
  [SerializeField] float adjustChangeMoveSpeedAmount = -2f;

  // CONSTANT CONCEPT:
  //
  // const means the value cannot be changed after it is created.
  //
  // "Hit" is the name of the Animator Trigger.
  const string hitString = "Hit";

  // Stores how much time has passed since the last collision.
  //
  // Start at 0.
  // Then Update() keeps increasing it.
  float coolDownCollisionTimer = 0f;

  // Reference to the RoadGenerator script.
  // We need this to change the road speed.
  RoadGenerator myRoadGenerator;

  void Start()
  {
    // Find the RoadGenerator component in the scene.
    //
    // FindFirstObjectByType<RoadGenerator>()
    // searches the scene and finds the first RoadGenerator.
    //
    // Then we store that reference in myRoadGenerator.
    myRoadGenerator = FindFirstObjectByType<RoadGenerator>();
  }

  void Update()
  {
    // Time.deltaTime = time passed since the previous frame.
    //
    // Example:
    // After 1 second → timer ≈ 1
    // After 2 seconds → timer ≈ 2
    //
    // This allows us to measure the cooldown time.
    coolDownCollisionTimer += Time.deltaTime;
  }

  // Called automatically by Unity when this object
  // collides with another collider.
  void OnCollisionEnter(Collision other)
  {

    // ========================================================
    // STEP 2: CHECK COLLISION COOLDOWN
    // ========================================================

    // If the cooldown time has NOT finished:
    // stop the method immediately.
    //
    // Example:
    // collisionHitCooldown = 1 second
    //
    // Timer = 0.5
    // 0.5 < 1 → TRUE
    // return → ignore this collision
    if (coolDownCollisionTimer < collisionHitCooldown) return;

    // ========================================================
    // STEP 3: REDUCE ROAD SPEED
    // ========================================================

    // Call the RoadGenerator method to change the speed.
    //
    // adjustChangeMoveSpeedAmount = -2
    //
    // So the road speed decreases by 2.
    myRoadGenerator.changeSpeedOfChunk(adjustChangeMoveSpeedAmount);


    // ========================================================
    // STEP 4: PLAY PLAYER HIT ANIMATION
    // ========================================================

    // Tell the Animator to activate the "Hit" trigger.
    //
    // "Hit" must match the Trigger name
    // inside the Animator Controller.
    playerAnimator.SetTrigger(hitString);


    // ========================================================
    // STEP 5: RESET THE COOLDOWN TIMER
    // ========================================================

    // Collision has now been processed.
    //
    // Reset timer to 0 so another collision
    // cannot immediately trigger the same logic.
    coolDownCollisionTimer = 0f;


  }
}
