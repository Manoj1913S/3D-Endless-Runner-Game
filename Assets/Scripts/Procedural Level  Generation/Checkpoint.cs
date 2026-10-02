using UnityEngine;

public class Checkpoint : MonoBehaviour
{
     // How much extra time the player gets after reaching the checkpoint
   [SerializeField] float checkPointTimeIncrease = 5f;

   // How much the obstacle spawn delay is reduced
    // Example: 2.0s delay -> 1.8s delay
   [SerializeField] float obstacleDecreaseTimeAmount = 0.2f;

   // Reference to GameOverManager so we can increase game time
   GameOverManager gameManagerTimeExtend;

    // Reference to ObstacleSpawner so we can make obstacles spawn faster
   ObstacleSpawner obstacleSpawner;

   // Constant string used to identify the Player
   const string playerString = "Player";



    void Start()
    {     // Find GameOverManager in the scene and store its reference
        gameManagerTimeExtend = FindFirstObjectByType<GameOverManager>();

         // Find ObstacleSpawner in the scene and store its reference
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        //only player crush then trigger it
         // Continue only if the object entering the trigger is the Player
        if(other.CompareTag(playerString))
        {
            //from gamemanager
             // Give the player extra time through GameOverManager 
           gameManagerTimeExtend.IncreaseTime(checkPointTimeIncrease);

            // Reduce obstacle spawn delay, making obstacles spawn faster
           obstacleSpawner.DecreaseObstacleSpanTime(obstacleDecreaseTimeAmount); 
        }
    }
}
