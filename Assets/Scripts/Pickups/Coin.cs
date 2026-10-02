using UnityEngine;

public class Coin : Pick_UpLogic
{
     // Amount of score that this coin gives when collected.
    //
    // [SerializeField] makes this field visible in the Unity
    // Inspector, so we can change the coin's score value
    // without changing the code.
    //
    // Example:
    // scoreAmount = 100
    // Player collects coin → +100 score
    [SerializeField] int scoreAmount = 100;

      // Stores a REFERENCE to the ScoreManager component.
    // The Coin itself does not store or display the total score.
    // ScoreManager is responsible for managing the score.
    // So Coin needs a reference to ScoreManager in order to
    // call its IncreaseScore() method.
    ScoreManager scoreManager;

    
    // Initializes this Coin with the ScoreManager reference.
    // 'scoreManagerDependency' is a PARAMETER.
    // The object that creates the Coin provides the ScoreManager
    // reference through this parameter.
    // This is called Dependency Injection because the Coin
    // receives the object/dependency it needs from outside
    // instead of finding it by itself.
    // Flow:
    // Chunk creates Coin
    //      ↓
    // Chunk calls Init(scoreManagerReference)
    //      ↓
    // Coin receives ScoreManager
    //      ↓
    // Coin stores the reference in scoreManager
     public void Init(ScoreManager scoreManagerDependency)
    {
        // 'this.scoreManager' means the field belonging to
        // the current Coin object.
        //
        // 'scoreManagerDependency' means the ScoreManager
        // reference received as a parameter.
        //
        // So:
        //
        // Coin's ScoreManager reference
        //          =
        // ScoreManager received from outside
        //
        // After this assignment, the Coin can communicate
        // with the ScoreManager.
        this.scoreManager =scoreManagerDependency;
    }

     // 'override' means we are providing the actual implementation
    // of the abstract OnPickup() method from Pick_UpLogic.
    // Pick_UpLogic says:
    // "Every pickup must define what happens when it is collected."
    // Coin defines:
    // "When I am collected, increase the score."
    protected override void OnPickup()
    {
        // Call the IncreaseScore() method from ScoreManager.
        // scoreManager → reference to ScoreManager
        // IncreaseScore() → method that changes the score
        // scoreAmount → amount of score to add
        // Example:
        // Current score = 500
        // scoreAmount = 100
        // IncreaseScore(100)
        //        ↓
        // New score = 600
       scoreManager.IncreaseScore(scoreAmount);
    }
}
