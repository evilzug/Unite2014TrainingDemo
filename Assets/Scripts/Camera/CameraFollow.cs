using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    //target for camera to follow
    public Transform target;
    //lag/smoothing to make the camera a bit smooth
    public float smoothing = 5f;

    //store distance between camera and player
    private Vector3 offset;

    void Start()
    {
        //the vector from the camera to the player
        offset = transform.position - target.position;
    }

    //follows every physics update, because we're following a physics update. Using Update() it would follow at a different time
    void FixedUpdate()
    {
        //position of the player, plus the offset we're storing
        Vector3 targetCamPos = target.position + offset;
        //move a 'little bit' towards the target position we just created, by the amount of the smoothing variable above, multipliued by the thig deltatime
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

    }

}
