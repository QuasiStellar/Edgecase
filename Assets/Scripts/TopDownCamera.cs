using System;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    private const float Tolerance = 0.001f;
    
    /// Normal speed of camera movement.
    private const float MovementSpeed = 50f;

    /// Speed of camera movement when control is held down.
    private const float FastMovementSpeed = 200f;
    
    /// Amount to zoom the camera when using the mouse wheel.
    private const float ZoomSensitivity = 20f;

    /// Amount to zoom the camera when using the mouse wheel (fast mode).
    private const float FastZoomSensitivity = 40f;

    private void Update()
    {
        var fastMode = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        var movementSpeed = fastMode ? FastMovementSpeed : MovementSpeed;
        var dPosition = Vector3.zero;
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dPosition += Vector3.left * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dPosition += Vector3.right * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            dPosition += Vector3.forward * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            dPosition += Vector3.back * (movementSpeed * Time.deltaTime);
        }
        
        var axis = Input.GetAxis("Mouse ScrollWheel");
        if (Math.Abs(axis) > Tolerance)
        {
            var zoomSensitivity = fastMode ? FastZoomSensitivity : ZoomSensitivity;
            GetComponent<Camera>().orthographicSize -= axis * zoomSensitivity;
        }
        
        transform.position += dPosition;
    }
}
