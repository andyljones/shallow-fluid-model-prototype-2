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

    private float azi;
    private float pol;
    private float r;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    void Start()
    {
        _initialPosition = transform.position;
        azi = Mathf.Atan2(_initialPosition.x, _initialPosition.y);
        pol = Mathf.Acos(_initialPosition.z/_initialPosition.magnitude);
        r = _initialPosition.magnitude;

        _initialRotation = transform.rotation;

    }

    void LateUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            azi = (azi + Input.GetAxis("Mouse X") * xSpeed) % 360;
            pol = (pol + Input.GetAxis("Mouse Y") * ySpeed) % 360;
        }

        r += Input.GetAxis("Mouse ScrollWheel") * rSpeed;

        var x = r * Mathf.Sin(azi)*Mathf.Sin(pol);
        var y = r*Mathf.Cos(azi)*Mathf.Sin(pol);
        var z = r*Mathf.Cos(pol);

        Vector3 cameraPosition = new Vector3(x, y, z);
        Vector3 localEast = Vector3.Cross(cameraPosition, new Vector3(0, 0, 1));
        Vector3 localNorth = Vector3.Cross(localEast, cameraPosition);

        transform.rotation = Quaternion.LookRotation(-cameraPosition, localNorth);
        transform.position = cameraPosition;
    }
}