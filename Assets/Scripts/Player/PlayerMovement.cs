using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    private Vector3 movement;
    private Animator anim;
    private Rigidbody playerRigidbody;
    int floorMask;
    private float camRayLength = 100f;

    //called no matter what, even if script isn't referenced
    void Awake()
    {
        //put floor quad in floor layer
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    //fires every physics update
    void FixedUpdate()
    {
        //standard is values between -1 and 1
        //raw axis is only -1, 0, or 1, no variation
        //this allows character to snap to full speed
        //axis are input - defaults within unity
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        //set the movement speeds but not in the Y axis
        movement.Set(h, 0f, v);
        //normalize the speed, otherwise diaganol travel would be at a speed advantage
        //also we should only move as much as happening during delta times instead of entire units every tick
        movement = movement.normalized*speed*Time.deltaTime;
        //moves current position plus new movement
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //shoots a ray from the camera and finds the point underneath the mouse
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        //if the ray we created has hit anything, the code below will execute
        //parametrs:
        //ray that we made,
        //out variable stores info from the function, but stored in the variable we made above
        //the length we stored above, how far we are going to do this raycast for
        //and only use the floor mask we created earlier
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            //create a vec3 of the point that we hit the floor, minus the position of the player
            Vector3 playerToMouse = floorHit.point - transform.position;
            //always set it to f
            playerToMouse.y = 0f;
            //can't set players position using a vector, need to use a quaternion
            //the z axis is the 'forward' vector of the player, by default
            //want to make the z location of hte mouse be the forward location of the player
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            //since we're not adding it, we just re-apply it, overwrite it instead of doing math to it like Move() above
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        //if h or v has a value then the player is walking.
        bool walking = h != 0f || v != 0f;
        //tells the animator component to set the specified property we made earlier to the value we tested for above
        anim.SetBool("IsWalking", walking);
    }

}













