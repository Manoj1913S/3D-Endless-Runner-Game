using UnityEngine;

public abstract class Pick_UpLogic : MonoBehaviour
{
    // Controls how fast the pickup rotates around the Y-axis.
    // [SerializeField] makes this private field visible in the
    // Unity Inspector, so the rotation speed can be changed
    // without modifying the code.
    // Example:
    // rotationYSpeed = 100
    // means the object rotates at 100 degrees per second.
    [SerializeField] float rotationYSpeed = 100f;

     // A constant string used to identify the Player tag.
    // 'const' means the value cannot be changed after compilation.
    // Instead of writing "Player" directly every time,
    // we store it once and reuse it.
    // This avoids repeatedly writing the same string and
    // reduces the chance of typing the tag incorrectly.
    const string playerString = "Player";



    void Update()
    {
         // Rotate the pickup object every frame around the Y-axis.
        // transform.Rotate(x, y, z)
        // X = 0 → no X rotation
        // Y = rotationYSpeed * Time.deltaTime → rotate around Y
        // Z = 0 → no Z rotation
        // Time.deltaTime makes the rotation frame-rate independent.
        // Example:
        // rotationYSpeed = 100
        // After approximately 1 second → object rotates 100 degrees.
        transform.Rotate(0f, rotationYSpeed*Time.deltaTime,0f);
    }
    void OnTriggerEnter(Collider other)
    {
         // OnTriggerEnter is a Unity physics callback.
        // It runs automatically when another Collider enters
        // this object's Trigger Collider.
        // 'other' contains information about the object
        // that entered the trigger.
        // Check whether the object that entered has the "Player" tag.
        // CompareTag() is safer and clearer than comparing
        // the tag string directly.
        // Example:
        // Player enters → true → continue
        // Enemy enters  → false → do nothing
        if(other.CompareTag(playerString))
        {
            // Call the pickup-specific behavior.
            // Pick_UpLogic does NOT decide what happens here.
            // The child class decides it.
            // Coin:
            //     OnPickup() → increase score
            // Strawberry:
            //     OnPickup() → increase road speed
            OnPickup();

                // Destroy this pickup GameObject after it has been collected.
            // gameObject means the GameObject this script is attached to.
            // So after pickup:
            // Coin → destroyed
            // Strawberry → destroyed
            Destroy(gameObject);
        }
    }

    //this protected keyword means we actually won't be able to call it from other classes unless we actually inherit from it For example, like our coin and our apple, the abstract keyword simply means we actually have to implemented in some of this other classes like our coin and apple

      // 'protected' means this method can be accessed by this class
    // and by classes that INHERIT from this class.
    //
    // Other unrelated classes cannot directly call it.
    //
    // 'abstract' means this method has NO implementation/body here.
    // The child class MUST provide the actual implementation.
    //
    // Because this class does not know what every pickup should do,
    // it leaves the exact behavior to the child classes.
    //
    // Example:
    //
    // Coin : Pick_UpLogic
    // {
    //     protected override void OnPickup()
    //     {
    //         // Increase score
    //     }
    // }
    //
    // Strawberry : Pick_UpLogic
    // {
    //     protected override void OnPickup()
    //     {
    //         // Increase road speed
    //     }
    // } 
    protected abstract void OnPickup();
    
}
