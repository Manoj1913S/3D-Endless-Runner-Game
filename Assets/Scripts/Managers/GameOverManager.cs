using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{

    // [SerializeField] allows a private field to be visible and editable
    // in the Unity Inspector.
    // PlayerController is the TYPE of this variable.
    // It stores a REFERENCE to the PlayerController component.
    // We need this reference because when Game Over happens,
    // we want to disable the player's movement/control.
    // Inspector example:
    // Player Controller → Drag Player GameObject here
    [SerializeField] PlayerController playerController;

    // Reference to the TextMeshPro UI text that displays the remaining time.
    // TMP_Text is the component TYPE used for displaying text on the UI.
    // [SerializeField] makes this field appear in the Inspector,
    // so we can drag the Time Text UI object into it.
    // Example:
    // timeLeft = 4.7
    //        ↓
    // timeText displays "4.7"
    [SerializeField] TMP_Text timeText;

    // Reference to the Game Over UI GameObject.
    // We keep this reference so that we can show the Game Over message
    // when the timer reaches zero.
    // At the beginning:
    // gameOverText.SetActive(false)
    // When Game Over happens:
    // gameOverText.SetActive(true)
    // [SerializeField] lets us assign the Game Over UI object
    // from the Unity Inspector.
    [SerializeField] GameObject gameOverText;

    // Starting amount of time given to the player.
    // float is used because the timer needs decimal values.
    // Example:
    // startTime = 5f
    // means the player starts with 5 seconds.
    // [SerializeField] allows us to change this value directly
    // from the Unity Inspector without changing the code.
    // Example:
    // Inspector: 5 → game starts with 5 seconds
    // Inspector: 10 → game starts with 10 seconds
    [SerializeField] float startTime = 5f;

    // Stores the CURRENT remaining time during gameplay.
    // startTime = starting value
    // timeLeft  = changing/current value
    // Example:
    // Start → timeLeft = 5
    // After 1 second → timeLeft ≈ 4
    // After 2 seconds → timeLeft ≈ 3
    float timeLeft;

    // Internal variable that stores the current Game Over state.
    // false → Game is still running
    // true  → Game is over
    // It is private by default because no access modifier is written.
    // This means other scripts cannot directly access this variable.
    bool gameOver = false;

    /*
     public bool GameOver
     {
         get{return gameOver;}
        // set{gameOver = value;}

     }
     */ //alternative of this comment line code

    // Public read-only property.
    // This gives OTHER scripts permission to READ the gameOver value
    // without giving them permission to CHANGE it.
    // Example:
    // ScoreManager can do:
    // if(gameOverManager.GameOver)
    // But another script cannot do:
    // gameOverManager.GameOver = false;
    // '=>' is expression-bodied syntax.
    // It means:
    // public bool GameOver
    // {
    //     get { return gameOver; }
    // }
    // So:
    // GameOver → returns the current value of gameOver. 
    public bool GameOver => gameOver;



    void Start()
    {
        // Copy the starting time into the current timer.
        // Example:
        // startTime = 5
        //     ↓
        // timeLeft = 5
        timeLeft = startTime;

    }

    void Update()
    {
        // Update() runs once every frame.
        // We call DecreaseTime() every frame so that
        // the countdown continuously decreases.
        DecreaseTime();
    }

    // Public method because another script, such as Checkpoint,
    // needs to give the player extra time.
    // 'float increaseTimeAmount' is a parameter.
    // The calling script decides how much time to add.
    // Example:
    // IncreaseTime(5f)
    // 5 seconds are added to the current time.
    public void IncreaseTime(float increaseTimeAmount) //Increase time when player reached checkpoint
    {
        // Add the received amount to the current remaining time.
        // Example:
        // timeLeft = 2.5
        // increaseTimeAmount = 5
        // 2.5 + 5 = 7.5
        timeLeft += increaseTimeAmount;
    }

    void DecreaseTime()
    {

        // If Game Over has already happened,
        // immediately stop executing this method.
        // 'return' exits the current method.
        // This prevents the timer from continuing to decrease
        // after Game Over.
        if (gameOver) return;


        // Time.deltaTime represents the time taken since the last frame.
        // Using deltaTime makes the timer frame-rate independent.
        // Example:
        // 60 FPS → small deltaTime
        // 30 FPS → larger deltaTime
        // But approximately 1 real second is still removed
        // after 1 real second.
        timeLeft -= Time.deltaTime;

        // Convert the float value into text and display it on the UI.
        // ToString("F1") means:
        // F1 = display exactly 1 digit after the decimal point.
        // Example:
        // 4.876 → "4.9"
        // 3.21  → "3.2"
        // 1     → "1.0"
        timeText.text = timeLeft.ToString("F1");

        // Check whether the remaining time has reached zero
        // or gone below zero.
        // <= means "less than OR equal to".
        // Example:
        // timeLeft = 0    → Game Over
        // timeLeft = -0.1 → Game Over
        if (timeLeft <= 0f)
        {
            // Call the method responsible for handling
            // everything that should happen when the game ends.
            PlayerGameOver();
        }


    }
    void PlayerGameOver()
    {
        // Change the Game Over state from false to true.
        // Before:
        // gameOver = false
        // After:
        // gameOver = true
        // Other scripts can now check:
        // gameOverManager.GameOver → true
        gameOver = true;

        // Disable the PlayerController component.
        // This stops the player's movement/control logic.
        // enabled = false means the component is disabled.
        playerController.enabled = false;


        // Activate the Game Over UI GameObject.
        // SetActive(true) makes the Game Over text visible
        // if the GameObject was previously inactive.
        gameOverText.SetActive(true);

        // Slow down the entire Unity game.
        // 1.0 = normal speed
        // 0.1 = 10% of normal speed
        // This makes the game appear to slow down when
        // Game Over happens.
        Time.timeScale = 0.1f;
    }
}
