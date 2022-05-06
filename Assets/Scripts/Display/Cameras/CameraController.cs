using System;
using System.Collections;
using UnityEngine;

/// Keys:
/// q               - switch cameras
///	wasd / arrows	- movement
///	space / shift	- up/down
///	hold control	- enable fast movement mode
///	right mouse  	- enable free look
///	mouse			- free look / rotation
public class CameraController : MonoBehaviour
{
    private const float Tolerance = 0.001f;

    /// Normal speed of camera movement.
    private const float MovementSpeed = 50f;

    /// Speed of camera movement when control is held down.
    private const float FastMovementSpeed = 200f;

    /// Sensitivity for free look.
    private const float FreeLookSensitivity = 3f;

    /// Amount to zoom the camera when using the mouse wheel.
    private const float ZoomSensitivity = 50f;

    /// Amount to zoom the camera when using the mouse wheel (fast mode).
    private const float FastZoomSensitivity = 100f;

    /// Set to true when free looking (on right mouse button).
    private bool _looking;

    private bool _coroutineIsExecuting;
    private void Update()
    {
        if (_coroutineIsExecuting) return;
        var fastMode = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        var movementSpeed = fastMode ? FastMovementSpeed : MovementSpeed;
        var dPosition = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            dPosition += -transform.right * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            dPosition += transform.right * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            var forward = transform.forward;
            dPosition += new Vector3(forward.x, 0, forward.z) * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            var forward = transform.forward;
            dPosition += -new Vector3(forward.x, 0, forward.z) * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            dPosition += Vector3.up * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            dPosition += Vector3.down * (movementSpeed * Time.deltaTime);
        }

        if (_looking)
        {
            var localEulerAngles = transform.localEulerAngles;
            var newRotationX = localEulerAngles.y + Input.GetAxis("Mouse X") * FreeLookSensitivity;
            var newRotationY = localEulerAngles.x - Input.GetAxis("Mouse Y") * FreeLookSensitivity;
            localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
            transform.localEulerAngles = localEulerAngles;
        }

        var axis = Input.GetAxis("Mouse ScrollWheel");
        if (Math.Abs(axis) > Tolerance)
        {
            var zoomSensitivity = fastMode ? FastZoomSensitivity : ZoomSensitivity;
            dPosition = transform.forward * (axis * zoomSensitivity);
        }

        transform.position += dPosition;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartLooking();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopLooking();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !_coroutineIsExecuting)
        {
            StartCoroutine(RotateCamera(60, 1));
        }
        
        if (Input.GetKeyDown(KeyCode.E) && !_coroutineIsExecuting)
        {
            StartCoroutine(RotateCamera(-60, 1));
        }
    }
    private IEnumerator RotateCamera(float angle, float duration)
    {
        _coroutineIsExecuting = true;
        var elapsedTime = 0f;

        var t = transform;
        var (startCameraPos, startCameraDir) = (t.position, t.eulerAngles);

        var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        
        Physics.Raycast(ray, out var hit);
        
        var fulcrum = hit.point;
        while (elapsedTime < duration)
        {
            RotateCamRoundVector(elapsedTime / duration * angle, fulcrum, startCameraPos, startCameraDir);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        RotateCamRoundVector(angle, fulcrum, startCameraPos, startCameraDir);
        _coroutineIsExecuting = false;
    }
    private void RotateCamRoundVector(float angle, Vector3 fulcrum, Vector3 startCameraPos, Vector3 startCameraDir)
    {
        var vector = Quaternion.AngleAxis(angle, Vector3.up) * (startCameraPos - fulcrum);
        transform.position = new Vector3(
            fulcrum.x + vector.x,
            fulcrum.y + vector.y,
            fulcrum.z + vector.z);

        startCameraDir.y += angle;
        transform.rotation = Quaternion.Euler(startCameraDir);
    }
    private void OnDisable()
    {
        StopLooking();
    }

    /// Enable free looking.
    private void StartLooking()
    {
        _looking = true;
    }

    /// Disable free looking.
    private void StopLooking()
    {
        _looking = false;
    }
}