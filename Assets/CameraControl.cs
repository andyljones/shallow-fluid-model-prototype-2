using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    /* 
     * Adaptation of the MouseOrbit.js script into C#. 
     * Should be minimally sufficient for orbiting and zooming parent
     */

    public float xSpeed = 5.0F;
    public float ySpeed = 5.0F;
    public float rSpeed = 20.0F;

    private float x;
    private float y;
    private float r;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        y = angles.x;
        x = angles.y;
        r = transform.position.magnitude;   
    }

    void LateUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            x = (x + Input.GetAxis("Mouse X") * xSpeed) % 360;
            y = (y - Input.GetAxis("Mouse Y") * ySpeed) % 360;
        }

        r += Input.GetAxis("Mouse ScrollWheel") * rSpeed;

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0F, 0.0F, -r);

        transform.rotation = rotation;
        transform.position = position;
    }
}