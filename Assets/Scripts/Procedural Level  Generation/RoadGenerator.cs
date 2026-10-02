using System.Collections.Generic;  // Gives us access to List<T>
using UnityEngine;                // Gives access to Unity classes and functions
using UnityEngine.UIElements;    // UI Toolkit namespace

public class RoadGenerator : MonoBehaviour
{
    // =========================================================
    // REFERENCES
    // =========================================================

    [Header("References")]
    // Reference to the camera controller.
    // We use this to change the camera FOV when road speed changes.
    [SerializeField] CameraController myCameraController;

    // Array of different normal chunk prefabs.
    // Array = one variable that stores multiple GameObjects.
    // Example:
    // chunkPrefabs[0] = Chunk A
    // chunkPrefabs[1] = Chunk B
    // chunkPrefabs[2] = Chunk C
    [SerializeField] GameObject[] chunkPrefabs; //INFO: cube based chunk prefab Instantiate into king Feet(paitala) drag and drop chunk prefab cube

    // Special checkpoint chunk prefab.
    [SerializeField] GameObject checkPointChunkPrefab;

    // Parent Transform used to keep all spawned chunks organized
    // under one GameObject in the Hierarchy.
    [SerializeField] Transform chunkParentCube; //spawn bhayeko chunk lai yeutai gameobject bhitra rakhne 

    // Reference to ScoreManager.
    // This reference will later be passed to each Chunk.
    [SerializeField] ScoreManager scoreManagerReference;

    [Header("Chunk Road Settings")]

    // Reference to ScoreManager.
    // This reference will later be passed to each Chunk.
    [SerializeField] int startingCubeChunksAmount = 12;

    // After every 8 chunks, spawn a checkpoint chunk.
    [SerializeField] int checkPointChunkInterval = 8;


    [Tooltip("Donot Change chunk Length ")]

    // Length of one chunk.
    // If one chunk is 10 units long:
    // Chunk 1 → Z = 0
    // Chunk 2 → Z = 10
    // Chunk 3 → Z = 20
    // IMPORTANT:
    // This value must match the actual prefab length.
    [SerializeField] float chunkCubeLength = 10f; // yeuta cube generate bhayeko 10 distance pachhi arko cube banau 

    // Current movement speed of all road chunks.
    [SerializeField] float cubeChunkMoveSpeed = 8f;

    // Minimum allowed road speed.
    [SerializeField] float cubeChunkMinMoveSpeed = 2f;

    // Maximum allowed road speed.
    [SerializeField] float cubeChunkMaxMoveSpeed = 20f;

    // Physics gravity limits on the Z axis.
    // Gravity is changed when road speed changes.
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;



    // GameObject[] cubeChunks = new GameObject[12]; //12 sized cube chunk array created  but this case alternate and best  way of using Array has use List 
    //OR
    // =========================================================
    // CHUNK STORAGE
    // =========================================================

    // List stores references to all currently active chunks.
    // Why List instead of Array?
    // Because chunks are continuously:
    // Add → Remove → Add → Remove
    // A List can dynamically change its size.
    // Example:
    // [Chunk1, Chunk2, Chunk3]
    // Remove Chunk1:
    // [Chunk2, Chunk3]
    // Spawn new Chunk:
    // [Chunk2, Chunk3, Chunk4]
    List<GameObject> cubeChunks = new List<GameObject>();


    // Keeps track of how many chunks have been spawned.
    // This is NOT the current number of chunks.
    // It is a counter used to decide when to spawn
    // a checkpoint chunk.
    int chunksSpawnTracker = 0;



    // =========================================================
    // UNITY EVENTS
    // =========================================================
    void Start()
    {
        // Start() runs once when the object starts.
        // Generate the initial road.
        CubeChunksGenerationLogic();
    }


    void Update()
    {
        // Update() runs every frame.
        // Continuously move all active road chunks.
        MoveCubeChunks();
    }

    // =========================================================
    // CHANGE ROAD SPEED
    // =========================================================
    public void changeSpeedOfChunk(float speedAmount)
    {
        // Calculate the new speed first.
        // Example:
        // Current speed = 8
        // speedAmount = +2
        // new speed = 8 + 2 = 10
        // We calculate into a temporary variable first
        // instead of immediately changing cubeChunkMoveSpeed.
        float newChunkCubeMoveSpeed = cubeChunkMoveSpeed + speedAmount;

        // Mathf.Clamp keeps a value inside a specific range.
        // Example:
        // min = 2
        // max = 20
        // If new speed = 25 → becomes 20
        // If new speed = 1  → becomes 2
        // If new speed = 10 → remains 10
        newChunkCubeMoveSpeed = Mathf.Clamp(newChunkCubeMoveSpeed, cubeChunkMinMoveSpeed, cubeChunkMaxMoveSpeed);

        // Only continue if the speed actually changed.
        //
        // Example:
        // Current speed = 2
        // speedAmount = -5
        // New speed gets clamped to 2.
        // 2 != 2 → false
        // So there is no need to update gravity/camera.
        if (newChunkCubeMoveSpeed != cubeChunkMoveSpeed)
        {
            // Finally save the new speed.
            cubeChunkMoveSpeed = newChunkCubeMoveSpeed;

            // Change gravity based on speed change.
            //
            // Example:
            // Current gravity Z = -10
            // speedAmount = +2
            //
            // -10 - 2 = -12
            float newGravityZ = Physics.gravity.z - speedAmount;

            // Keep gravity between the allowed limits.
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);

            // Physics.gravity is a global Unity setting.
            // Vector3:
            // X → keep current gravity X
            // Y → keep current gravity Y
            // Z → use the newly calculated gravity
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);


            // Tell the camera to change its FOV.
            // Positive speedAmount → camera zoom effect for speed up
            // Negative speedAmount → camera adjusts for slowdown
            myCameraController.ChangeCameraFOV(speedAmount);
        }
    }

    // =========================================================
    // INITIAL CHUNK GENERATION
    // =========================================================
    void CubeChunksGenerationLogic()
    {
        // Create multiple chunks at the beginning of the game.
        // Example:
        // startingCubeChunksAmount = 12
        // i = 0 → SpawnChunk()
        // i = 1 → SpawnChunk()
        // ...
        // i = 11 → SpawnChunk()
        // Total = 12 chunks
        for (int i = 0; i < startingCubeChunksAmount; i++) //Yek Patak ma 12 chunk lai show gar wa render gar 
        {
            SpawnChunk();
        }
    }

    // =========================================================
    // SPAWN ONE CHUNK
    // =========================================================

    void SpawnChunk()
    {
        // First calculate where the new chunk should be placed.
        float spawnChunkPositionZ = CalculateSpawnPositionInZ();

        // Create the complete 3D position.
        // X and Y come from RoadGenerator.
        // Z comes from our calculation.
        Vector3 cubeChunkSpawnPosition = new Vector3(transform.position.x, transform.position.y, spawnChunkPositionZ);

        // Decide which prefab should be spawned.
        // It can be:
        // Normal random chunk
        // OR
        // Checkpoint chunk
        GameObject chunkToSpawn = ChooseChunkToSpawn();


        // Instantiate creates a new copy of the selected prefab.
        // Parameter 1 → prefab
        // Parameter 2 → position
        // Parameter 3 → rotation
        // Parameter 4 → parent
        // Quaternion.identity = no rotation.
        GameObject newCubeChunkGO = Instantiate(chunkToSpawn, cubeChunkSpawnPosition, Quaternion.identity, chunkParentCube); //NOTE: Let's instantiate it at the Transform.position(cubeChunkSpawnPosition) of the game object on which our level generator is attached (In this case this script attached in RoadGeneratorGameObject so yehi roadgeneratorgameobject jaha chha tehi instantiate harchha chunk prefab jun chai aaghi cube prefab banayera delete gareko drag and drop garne

        // Add the newly created chunk to our List.
        // The List now knows that this chunk is active.
        cubeChunks.Add(newCubeChunkGO); //INFO: each and every instantiate chunk of cube store in above 12 size Game Object array

        // Get the Chunk script from the newly created GameObject.
        // GetComponent<T>() searches for component T
        // attached to that GameObject.
        Chunk newChunk = newCubeChunkGO.GetComponent<Chunk>();

        // Pass required references into the Chunk.
        // "this" = current RoadGenerator object.
        // Chunk receives:
        // RoadGenerator reference
        // ScoreManager reference
        newChunk.Init(this, scoreManagerReference);

        // Increase the total spawn counter.
        // This is used by ChooseChunkToSpawn()
        // to decide when a checkpoint should appear.
        chunksSpawnTracker++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        // Variable that will store the prefab we decide to spawn.
        GameObject chunkToSpawn;

        // "%" is the MODULO operator.
        // It gives the remainder after division.
        // Example:
        // 8 % 8 = 0
        // 16 % 8 = 0
        // 10 % 8 = 2
        // Therefore:
        // If chunksSpawnTracker is exactly divisible by
        // checkPointChunkInterval, spawn checkpoint.
        //
        // chunksSpawnTracker != 0 prevents the very first
        // chunk from being a checkpoint.
        if (chunksSpawnTracker % checkPointChunkInterval == 0 && chunksSpawnTracker != 0)
        {
            // Select the special checkpoint prefab.
            chunkToSpawn = checkPointChunkPrefab;
        }
        else
        {
            // Randomly choose one normal chunk prefab.
            //
            // chunkPrefabs.Length = number of prefabs.
            //
            // Example:
            // Length = 3
            // Random.Range(0, 3)
            // → 0, 1, or 2
            //
            // Then:
            // chunkPrefabs[randomIndex]
            // → gets that prefab from the array.
            chunkToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        }

        // Return sends the selected prefab back
        // to the SpawnChunk() method.
        return chunkToSpawn;
    }

    // =========================================================
    // CALCULATE NEW CHUNK Z POSITION
    // =========================================================
    float CalculateSpawnPositionInZ()
    {
        // Temporary variable to store the calculated Z position.
        float spawnChunkPositionZ;

        // Check whether any chunk currently exists.
        // Count = current number of elements in the List.
        // If Count == 0:
        // There is no previous chunk to calculate from.
        if (cubeChunks.Count == 0)
        {
            // Place the first chunk at RoadGenerator's Z position.
            spawnChunkPositionZ = transform.position.z;
        }
        else
        {
            // spawnChunkPositionZ = transform.position.z + (i * chunkCubeDistance); alternate of this below code
            // Get the LAST chunk in the List.
            // List indexes start from 0.
            // If Count = 12:
            // Last index = 11
            // Therefore:
            // cubeChunks[cubeChunks.Count - 1]
            // means "get the last chunk".
            // Then:
            // last chunk Z + chunk length
            // Example:
            // Last chunk Z = 110
            // Length = 10
            // New chunk Z = 120
            spawnChunkPositionZ = cubeChunks[cubeChunks.Count - 1].transform.position.z + chunkCubeLength;
        }

        // Return the calculated Z position.
        return spawnChunkPositionZ;
    }

    // =========================================================
    // MOVE AND REMOVE OLD CHUNKS
    // =========================================================
    void MoveCubeChunks()
    {
        // Loop through every currently active chunk.
        // List uses .Count
        // Array uses .Length
        for (int i = 0; i < cubeChunks.Count; i++) //in case of Array use .Length not Count
        {
            // Get the chunk at the current List index.
            // Example:
            // i = 0 → first chunk
            // i = 1 → second chunk
            // i = 2 → third chunk
            GameObject induividualChunk = cubeChunks[i];

            // Move the chunk every frame.
            // transform.forward = object's forward direction.
            // "-" makes the movement go backward.
            // cubeChunkMoveSpeed = movement speed.
            // Time.deltaTime = time passed since previous frame.
            // Speed × Time = Distance
            // Example:
            // Speed = 8
            // deltaTime = 0.02
            // Distance = 8 × 0.02
            //          = 0.16 units/frame
            // Multiplication is grouped inside brackets
            // so the complete movement vector is calculated first.
            induividualChunk.transform.Translate(-transform.forward * (cubeChunkMoveSpeed * Time.deltaTime)); //for vector*float take time to calculate so bracket ma rakheko 


            // Check whether the chunk has moved far enough
            // behind the camera.
            // Camera Z - chunk length
            // creates a safe distance behind the camera.
            // If the chunk is behind this point,
            // we no longer need it.
            if (induividualChunk.transform.position.z <= Camera.main.transform.position.z - chunkCubeLength)
            {
                // Remove the chunk reference from the List.
                // IMPORTANT:
                // Remove() removes the actual GameObject
                // from the List.
                // It does NOT destroy the GameObject from the scene.
                cubeChunks.Remove(induividualChunk); //removing list in this liat consist of cube chunks gameobject

                // Destroy the actual GameObject from the scene.
                //Destroy chunk
                // Remove() → removes from List
                // Destroy() → removes from Unity scene
                Destroy(induividualChunk);

                //After deleting the whole chunks again create same chunks so call this method 
                // A chunk was removed,
                // so immediately create a new chunk.
                //
                // This keeps the road continuously generated.
                SpawnChunk();

            }
        }
    }
}
