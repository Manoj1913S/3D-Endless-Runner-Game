using UnityEngine;

public class Strawberry : Pick_UpLogic
{
    // Amount of speed that will be added to the road when
    // the player collects the strawberry.
    // [SerializeField] makes this private field visible in
    // the Unity Inspector, so we can change the value without
    // modifying the code.
    // Example:
    // Current road speed = 8
    // adjustChangeMoveSpeedAmount = 3
    // New road speed = 11
    [SerializeField] float adjustChangeMoveSpeedAmount = 3f;

     // Stores a REFERENCE to the RoadGenerator component.
    // We need this reference because the Strawberry itself
    // does not control the road speed.
    // RoadGenerator contains the method:
    // changeSpeedOfChunk()
    // So Strawberry will communicate with RoadGenerator
    // through this reference.
   RoadGenerator myRoadGenerator;


      // Initializes this Strawberry with the RoadGenerator reference.
    // 'roadGenerator' is a PARAMETER received from the caller.
    // This is a form of Dependency Injection:
    // instead of Strawberry finding RoadGenerator itself,
    // another script gives Strawberry the required dependency.
    // Flow:
    // RoadGenerator
    //      ↓
    // creates Strawberry
    //      ↓
    // calls Init(this, ...)
    //      ↓
    // Strawberry receives RoadGenerator reference
    //      ↓
    // stores it in myRoadGenerator
    public void Init(RoadGenerator roadGenerator/*Dependency Injection*/) 
    {
         // 'this.myRoadGenerator' means the field belonging
        // to this Strawberry object.
        // 'roadGenerator' means the parameter received by Init().
        // So we are basically doing:
        // Strawberry's field = received RoadGenerator
        // After this:
        // myRoadGenerator → points to RoadGenerator
        this.myRoadGenerator = roadGenerator;

    }

        // Override means we are replacing the OnPickup() method
    // behavior provided by the parent Pick_UpLogic class.
    // When this Strawberry is picked up, this method runs.
    protected override void OnPickup()
    {
         // Tell RoadGenerator to increase the road/chunk movement speed.
        // myRoadGenerator → reference to RoadGenerator
        // changeSpeedOfChunk() → method inside RoadGenerator
        // adjustChangeMoveSpeedAmount → +3 speed
        // Example:
        // Current speed = 8
        // Amount = +3
        // New speed = 11
        myRoadGenerator.changeSpeedOfChunk(adjustChangeMoveSpeedAmount);
    }
}
