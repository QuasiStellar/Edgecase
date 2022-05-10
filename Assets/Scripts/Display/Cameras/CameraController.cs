using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Display.Cameras
{
    /// Keys:
    /// q               - switch cameras
    ///	wasd / arrows	- movement
    ///	space / shift	- up/down
    ///	hold control	- enable fast movement mode
    ///	right mouse  	- enable free look
    ///	mouse			- free look / rotation
    public class CameraController : MonoBehaviour
    {
        public PlayerInput playerInput;

        /// Normal speed of camera movement.
        private const float MovementSpeed = 50f;

        /// Speed of camera movement when control is held down.
        private const float FastMovementSpeed = 200f;

        private Camera _camera;
        private bool _coroutineIsExecuting;
        private Vector3 _cameraForwardProjection;
        private Vector3 _cameraRightProjection;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            _cameraForwardProjection = GetForwardProjectionAndNormalizeIt();
            _cameraRightProjection = transform.right;

            playerInput.actions["RotateCameraLeft"].performed += _ => RotateCameraClockwise();
            playerInput.actions["RotateCameraRight"].performed += _ => RotateCameraCounterclockwise();
        }

        private void Update()
        {
            if (_coroutineIsExecuting) return;

            var inputMoveVector = playerInput.actions["Move"].ReadValue<Vector2>();

            var fastMode = playerInput.actions["FastMode"].IsPressed();
            var movementSpeed = fastMode ? FastMovementSpeed : MovementSpeed;

            transform.position += _cameraRightProjection * (movementSpeed * Time.deltaTime * inputMoveVector.x) +
                                  _cameraForwardProjection * (movementSpeed * Time.deltaTime * inputMoveVector.y);
        }

        private void RotateCameraClockwise()
        {
            if (!_coroutineIsExecuting)
            {
                StartCoroutine(RotateCamera(-60, 1));
            }
        }

        private void RotateCameraCounterclockwise()
        {
            if (!_coroutineIsExecuting)
            {
                StartCoroutine(RotateCamera(60, 1));
            }
        }

        private IEnumerator RotateCamera(float angle, float duration)
        {
            _coroutineIsExecuting = true;

            var elapsedTime = 0f;
            var t = transform;
            var (startPosition, startRotation) = (t.position, t.eulerAngles);

            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Physics.Raycast(ray, out var hit);
            var fulcrum = hit.point;

            while (elapsedTime < duration)
            {
                RotateAroundVector(elapsedTime / duration * angle, fulcrum, startPosition, startRotation);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            RotateAroundVector(angle, fulcrum, startPosition, startRotation);

            _cameraForwardProjection = GetForwardProjectionAndNormalizeIt();
            _cameraRightProjection = transform.right;
            _coroutineIsExecuting = false;
        }

        private void RotateAroundVector(float angle, Vector3 fulcrum, Vector3 startPosition, Vector3 startRotation)
        {
            var vector = Quaternion.AngleAxis(angle, Vector3.up) * (startPosition - fulcrum);
            transform.position = new Vector3
            (
                fulcrum.x + vector.x,
                fulcrum.y + vector.y,
                fulcrum.z + vector.z
            );

            startRotation.y += angle;
            transform.rotation = Quaternion.Euler(startRotation);
        }

        private Vector3 GetForwardProjectionAndNormalizeIt()
        {
            var forwardProjection = transform.forward;
            forwardProjection.y = 0;
            return Vector3.Normalize(forwardProjection);
        }
    }
}