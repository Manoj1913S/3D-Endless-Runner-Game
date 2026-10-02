using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float movePlayerSpeed = 10f;

    [Header("Player Clamp Variables")]
    [SerializeField] float xClampPos = 2f;
    [SerializeField] float zClampPos = 2f;
    




    Rigidbody playerRigidbody;
    Vector2 movementPlayer; //invoke unity event input system

    void Awake()
    {
        //it can be a good idea to get references on your own game object in awake
        playerRigidbody = GetComponent<Rigidbody>(); //component is directly attached to the same GameObject as our player controller 
    }

    void FixedUpdate()
    {
        PlayerRun();
       
    }

    public void MovePlayerInput(InputAction.CallbackContext context)
    {
        movementPlayer = context.ReadValue<Vector2>();
        Debug.Log(movementPlayer); //It gives -1 for (A/S) and 1 for (W/D)
    }
    //use this(-1/+1) values for character moving around using rigidbody So

    void PlayerRun()
    {
         Vector3 currentPosition = playerRigidbody.position; // this is physics based movement so instead of transform.position we use playerRigidbody.position
        Vector3 moveDirection = new Vector3(movementPlayer.x, 0f ,movementPlayer.y);
        Vector3 newMovementPosition = currentPosition + moveDirection* (movePlayerSpeed * Time.fixedDeltaTime); 
      
       //Clamp Logic :- newMovementPosition chai player  movement ma clamp gara bhaneko ho ke 
       newMovementPosition.x = Mathf.Clamp(newMovementPosition.x , -xClampPos, xClampPos); //-x to +x
       newMovementPosition.z = Mathf.Clamp(newMovementPosition.z , -zClampPos, zClampPos); //-z to z

     //clamp pachhi movement gar ke 
            playerRigidbody.MovePosition(newMovementPosition);
    }



}
