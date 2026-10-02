
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    // GameObject variable stores a reference to a prefab.
    // [SerializeField] makes a private field visible in the Unity Inspector.
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject strawBerryPrefab;
    [SerializeField] GameObject coinSpawnPrefab;


     // Spawn chance:
    // 0.3 = 30% chance to spawn Strawberry
    // 0.5 = 50% chance to spawn Coins
    [SerializeField] float strawBerrySpawnChance = 0.3f;
    [SerializeField] float coinSpawnChance = 0.5f;

    // Distance between each coin on the Z axis
    [SerializeField] float coinSeperationLength = 2f;

      // Array = one variable that can store multiple values of the same type.
    // Here we store the X position of each lane:
    //
    // Index       Value
    //   0        -3.83  → Left lane
    //   1         0.00  → Middle lane
    //   2         5.00  → Right lane
    // IMPORTANT:
    // Array indexing starts from 0, not 1.
    [SerializeField] float[] lanes = { -3.83f, 0f, 5f };

      // These variables store references to other scripts.
    // We don't create new RoadGenerator/ScoreManager here.
    // We receive their existing references through Init().
    RoadGenerator roadGeneratorReference;
    ScoreManager scoreManagerReferance;

      // List = dynamic collection.
    // Unlike an array, a List can add/remove elements during runtime.
    // This list stores LANE INDEXES, not lane positions.
    // Initially:
    // availableLanes = [0, 1, 2]
    // Meaning:
    // 0 = left lane is free
    // 1 = middle lane is free
    // 2 = right lane is free
    List<int> availableLanes = new List<int> { 0, 1, 2 };


    void Start()
    {
         // Unity automatically calls Start() when this object becomes active.
        // We decide what should be spawned in this chunk.
        SpawnWalls();
        SpawnStrawBerry();
        SpawnCoins();
    }


    // Init = Initialize.
    // This method receives references from another script.
    // A → RoadGenerator reference comes in
    // B → ScoreManager reference comes in
    // C → Store them in this Chunk
    // This is called Dependency Injection:
    // Instead of Chunk searching/creating these objects itself,
    // another script gives Chunk the references it needs.
    public void Init(RoadGenerator roadGeneratorDependent, ScoreManager scoreManagerDependent) //for chunk
    {
        // "this" means the current Chunk object.
        // Store the received RoadGenerator reference
        // inside this Chunk's roadGeneratorReference variable.
        this.roadGeneratorReference = roadGeneratorDependent;

          // Store the received ScoreManager reference.
        this.scoreManagerReferance =  scoreManagerDependent;
    }

    void SpawnWalls()
    {
       // Random.Range(int, int) returns a random INTEGER.
        // lanes.Length = 3
        // Random.Range(0, 3) can return:
        // 0, 1, or 2
        // IMPORTANT:
        // For integers, the maximum value is EXCLUDED.
        // So this randomly decides how many walls to spawn.
        // 0 → no wall
        // 1 → one wall
        // 2 → two walls
        // If you wanted 0,1,2,3:
        // Random.Range(0, 4)
        int wallToSpawn = Random.Range(0, lanes.Length); //Wall randomly generate variable sometimes single wall sometimes double and max triple


        //so randomly generate countionusly so loop provide garne
           // for loop:
        // int i = 0      → starting value
        // i < wallToSpawn → condition
        // i++            → increase i by 1
        // Example:
        // wallToSpawn = 2
        // i = 0 → spawn wall
        // i = 1 → spawn wall
        // i = 2 → stop because 2 < 2 is false
        for (int i = 0; i < wallToSpawn; i++)
        {
            // Count tells us how many elements are currently inside the List.
            // Example:
            // [0, 2] → Count = 2
            // If Count becomes 0, there is no free lane left.
            if (availableLanes.Count <= 0)
            {
                 // break immediately stops the loop.
                break;
            }

             // Call SelectLane() to choose one RANDOM FREE lane.
            // Example:
            // availableLanes = [0, 1, 2]
            // SelectLane() → returns 1
            // selectedLane = 1
            int selectedLane = SelectLane();

             // Vector3 stores 3D position:
            // X = lane position
            // Y = chunk's Y position
            // Z = chunk's Z position
            // lanes[selectedLane]:
            // If selectedLane = 1
            // lanes[1] = 0
            // So the wall will be placed at X = 0.
            Vector3 spawnWallPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);

             // Instantiate() creates a NEW copy of the prefab.
            //
            // Parameter 1 → prefab to create
            // Parameter 2 → position
            // Parameter 3 → rotation
            // Parameter 4 → parent
            // Quaternion.identity means NO ROTATION.
            // this.transform means:
            // Make the spawned wall a child of this Chunk.
            Instantiate(wallPrefab, spawnWallPosition, Quaternion.identity, this.transform); //Queternion.identity gives no rotation






        }

    }

    void SpawnStrawBerry()
    {
        //spawn using instantiate when game start so code ko through spawn garaune
         // Random.value returns a random FLOAT between 0 and 1.
        // Example:
        // Random.value = 0.7
        // 0.7 > 0.3 → TRUE
        // Therefore return → Strawberry does NOT spawn.
        // If:
        // Random.value = 0.2
        // 0.2 > 0.3 → FALSE
        // So the code continues → Strawberry spawns.
        // The second condition checks whether a lane is available.
        // || means OR.
        // So if EITHER condition is true → return.
        if (Random.value > strawBerrySpawnChance || availableLanes.Count <= 0) return;
        

          // Select one FREE lane.
        // SelectLane() also removes that lane from
        // availableLanes so another object cannot use it.
        int selectedLane = SelectLane();

         // Create Strawberry's 3D spawn position.
        Vector3 spawnWallPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);

        // Instantiate() returns the newly created GameObject.
        // GetComponent<Strawberry>() then finds the
        // Strawberry script attached to that GameObject.
        // So:
        //
        // Instantiate
        //      ↓
        // New GameObject
        //      ↓
        // GetComponent<Strawberry>()
        //      ↓
        // Strawberry reference
        // That reference is stored in newStrawberry.
       Strawberry newStrawberry = Instantiate(strawBerryPrefab, spawnWallPosition, Quaternion.identity, this.transform).GetComponent<Strawberry>(); //Queternion.identity gives no rotation 

       // Send the RoadGenerator reference to the newly created Strawberry.
        // This allows Strawberry to communicate with RoadGenerator.
       newStrawberry.Init(roadGeneratorReference);
    }

    void SpawnCoins()
    {
         // Check two conditions before spawning coins.
        // Condition 1:
        // availableLanes.Count <= 0
        // → No free lane.
        // Condition 2:
        // Random.value > coinSpawnChance
        // → Random chance failed.
        // || means OR.
        // If either condition is true → return.
        if (availableLanes.Count <= 0 || Random.value > coinSpawnChance) return;

         // Select ONE free lane for the entire coin line.
        // All coins will use this same lane.
       
        int selectedLane = SelectLane();


        //coin generation logic
           // Maximum value used for Random.Range().
        // Random.Range(1, 6) gives:
        // 1, 2, 3, 4, or 5
        // 6 is excluded for integer Random.Range().
        int maxCoinSpawn = 6;
        int coinSpawn = Random.Range(1, maxCoinSpawn);

         // Calculate where the first coin should start on Z axis.
        // Example:
        // chunk Z = 100
        // separation = 2
        // chunkZPosition = 100 + (2 × 2)
        //               = 104
        float chunkZPosition = transform.position.z + (coinSeperationLength * 2f);

          // Repeat the loop based on the random coin count.
        // Example:
        // coinSpawn = 4
        // Loop runs:
        // i = 0
        // i = 1
        // i = 2
        // i = 3
        for (int i = 0; i < coinSpawn; i++)
        {
             // Calculate the Z position of the current coin.
            // Formula:
            // Start position - (coin number × separation)
            // Example:
            // Start Z = 104
            // Separation = 2
            // i = 0 → 104 - (0 × 2) = 104
            // i = 1 → 104 - (1 × 2) = 102
            // i = 2 → 104 - (2 × 2) = 100
            // i = 3 → 104 - (3 × 2) = 98
            // This creates a line of coins with equal spacing.
            float spawnActualPositionZ = chunkZPosition - (i * coinSeperationLength);

               // Create the coin's 3D position.
            // X → selected lane
            // Y → chunk Y
            // Z → calculated coin Z
            Vector3 spawnWallPosition = new Vector3(lanes[selectedLane], transform.position.y, spawnActualPositionZ);

             // Create the coin GameObject.
            // Then get the Coin script attached to it.
           Coin newCoin = Instantiate(coinSpawnPrefab, spawnWallPosition, Quaternion.identity, this.transform).GetComponent<Coin>();

             // Give the Coin its ScoreManager reference.
            // This allows the Coin to increase the player's score
            // when the player collects it.
           newCoin.Init(scoreManagerReferance);
        }

    }

    int SelectLane()
    {
        // availableLanes.Count tells us how many FREE lanes exist.
        // Example:
        // availableLanes = [0, 2]
        // Count = 2
        // Random.Range(0, 2) gives:
        // 0 or 1
        // IMPORTANT:
        // randomLaneIndex is the LIST INDEX,
        // NOT the actual lane number.
        int randomLaneIndex = Random.Range(0, availableLanes.Count);

         // Use the random list index to get the actual lane number.
        // Example:
        // availableLanes = [0, 2]
        // randomLaneIndex = 1
        // availableLanes[1] = 2
        // Therefore:
        // selectedLane = 2
        int selectedLane = availableLanes[randomLaneIndex];

         // Remove the selected lane from the List.
        // RemoveAt() removes an element using its INDEX.
        // Example:
        // availableLanes = [0, 2]
        // randomLaneIndex = 1
        // RemoveAt(1)
        // availableLanes becomes [0]
        // This is IMPORTANT because it prevents
        // another spawned object from using the same lane.
        availableLanes.RemoveAt(randomLaneIndex);

        // return sends the selected lane number back
        // to the method that called SelectLane().
        // Example:
        // SelectLane() → returns 2
        // int selectedLane = SelectLane();
        // selectedLane becomes 2
        return selectedLane;
    }
}
