using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // ARRAY CONCEPT:
    // [] means this variable can store MULTIPLE values/objects of the same type.
    //
    // GameObject[] = an array that stores multiple GameObjects.
    //
    // Example:
    // obstaclePrefabs = [Rock, Tree, Barrel, Car]
    //
    // Instead of creating separate variables:
    // GameObject rock;
    // GameObject tree;
    // GameObject barrel;
    //
    // We can store all of them inside ONE array.
    [SerializeField] GameObject[] obstaclePrefabs;

    // Time between each obstacle spawn.
    // Example: 2f = wait 2 seconds before spawning the next obstacle.
    [SerializeField] float obstacleDelayTime = 2f;

     [SerializeField] float minObstacleDelaySpawnTime = 0.2f;
    // Transform that will become the parent of spawned obstacles.
    // This keeps the Hierarchy organized.
    [SerializeField] Transform obstacleMain;


    // Controls how far left/right the obstacle can randomly spawn.
    //
    // Example:
    // spawnnerWidth = 4
    //
    // Random X position will be between:
    // -4 and +4
    [SerializeField] float spawnnerWidth = 4f;


    // int obstacleSpawn = 0;

    void Start()
    {

        // Start the Coroutine when the game begins.
        // Coroutine = a method that can PAUSE and CONTINUE later.and provide delay smoothly
        StartCoroutine(SpawnObstacleDelayRoutine());
    }


   public  void DecreaseObstacleSpanTime(float amount)
    {
        obstacleDelayTime -= amount;
        if(obstacleDelayTime<= minObstacleDelaySpawnTime)
        {
            obstacleDelayTime = minObstacleDelaySpawnTime; 
        }
    }

    IEnumerator SpawnObstacleDelayRoutine()
    {
        // while(true) means:
        // Keep repeating this code FOREVER
        // until the Coroutine/GameObject is stopped.
        while (true)
        {
            // ARRAY + RANDOM CONCEPT:
            //
            // obstaclePrefabs.Length = total number of objects in the array.
            //
            // Example:
            // obstaclePrefabs = [Rock, Tree, Barrel]
            // Length = 3
            //
            // Random.Range(0, 3)
            // can give: 0, 1, or 2
            //
            // Then that random INDEX is used to get
            // one random obstacle from the array.
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];


            // below this code  Create a random spawn position.
            //
            // X = random position between -spawnnerWidth and +spawnnerWidth
            // Y = spawner's Y position
            // Z = spawner's Z position
            //
            // Example:
            // spawnnerWidth = 4
            // X can be: -3.2, 0.5, 2.8, etc.
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnnerWidth, spawnnerWidth), transform.position.y, transform.position.z);

            //Below this code  PAUSE the Coroutine for obstacleDelayTime seconds.
            //
            // Example:
            // obstacleDelayTime = 2
            // ↓
            // Wait 2 seconds
            // ↓
            // Continue to the next line
            yield return new WaitForSeconds(obstacleDelayTime);


            //Below this code  Instantiate = CREATE a copy of the selected prefab.
            //
            // obstaclePrefab → which object to spawn
            // spawnPosition  → where to spawn
            // Random.rotation → random rotation
            // obstacleMain   → make it a child of obstacleMain
            Instantiate(obstaclePrefab, spawnPosition, Random.rotation, obstacleMain);
            // obstacleSpawn++;
        }
    }



}
