using System;
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
        private const float CameraHeight = GameController.CameraHeight;
        private bool _cameraIsTop;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            var forwardProjection = transform.forward;
            forwardProjection.y = 0;
            _cameraForwardProjection = Vector3.Normalize(forwardProjection);
            _cameraRightProjection = transform.right;

            playerInput.actions["RotateCameraLeft"].performed += _ => RotateCameraClockwise();
            playerInput.actions["RotateCameraRight"].performed += _ => RotateCameraCounterclockwise();
            playerInput.actions["RotateCameraUp"].performed += _ => RotateCameraUp();
            playerInput.actions["RotateCameraDown"].performed += _ => RotateCameraDown();
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
                StartCoroutine(RotateCameraSidewaysCoroutine(-60, 1));
            }
        }

        private void RotateCameraCounterclockwise()
        {
            if (!_coroutineIsExecuting)
            {
                StartCoroutine(RotateCameraSidewaysCoroutine(60, 1));
            }
        }

        private void RotateCameraUp()
        {
            if (_coroutineIsExecuting || _cameraIsTop) return;
            StartCoroutine(RotateCameraUpCoroutine(1));
            _cameraIsTop = true;
        }

        private void RotateCameraDown()
        {
            if (_coroutineIsExecuting || !_cameraIsTop) return;
            StartCoroutine(RotateCameraDownCoroutine(1));
            _cameraIsTop = false;
        }

        private IEnumerator RotateCameraUpCoroutine(float duration)
        {
            _coroutineIsExecuting = true;

            var fulcrum = GetMousePointedRayHit();
            var t = transform;
            var startPos = t.position;
            var finishPos = fulcrum;
            finishPos.y = CameraHeight;
            var fullShift = finishPos - startPos;
            var rotation = t.eulerAngles;
            var startCameraSlope = rotation.x;

            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                MoveTowards(fullShift * elapsedTime / duration, startPos,
                    startCameraSlope + 30 * elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            MoveTowards(fullShift, startPos,
                startCameraSlope + 30);

            _coroutineIsExecuting = false;
        }

        private IEnumerator RotateCameraDownCoroutine(float duration)
        {
            _coroutineIsExecuting = true;

            var startPos = transform.position;
            var ray = _camera.ScreenPointToRay(new Vector3(_camera.pixelWidth / 2f, _camera.pixelHeight / 2f,
                0));
            Physics.Raycast(ray, out var hit);
            var fulcrum = hit.point;
            var rotationVector = startPos - fulcrum;
            var height = rotationVector.y;
            rotationVector = Quaternion.AngleAxis(-30, _cameraRightProjection) * rotationVector;
            rotationVector = Vector3.Normalize(rotationVector) * (float) (height / Math.Sin(Math.PI / 3));
            var finishPos = fulcrum + rotationVector;
            var fullShift = finishPos - startPos;

            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                MoveTowards(fullShift * elapsedTime / duration, startPos,
                    90 - 30 * elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            MoveTowards(fullShift, startPos,
                60);

            _coroutineIsExecuting = false;
        }

        private IEnumerator RotateCameraSidewaysCoroutine(float angle, float duration)
        {
            _coroutineIsExecuting = true;

            var elapsedTime = 0f;
            var t = transform;
            var (startPosition, startRotation) = (t.position, t.eulerAngles);

            Vector3 fulcrum = GetMousePointedRayHit();

            while (elapsedTime < duration)
            {
                RotateAroundVector(elapsedTime / duration * angle, fulcrum, startPosition, startRotation);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            RotateAroundVector(angle, fulcrum, startPosition, startRotation);

            _cameraForwardProjection = Quaternion.AngleAxis(angle, Vector3.up) * _cameraForwardProjection;
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

        private void MoveTowards(Vector3 shiftFromStartPosition,
            Vector3 startPosition,
            float cameraSlope)
        {
            var rotation = transform.eulerAngles;
            rotation.x = cameraSlope;
            Transform t;
            (t = transform).rotation = Quaternion.Euler(rotation);
            t.position = startPosition + shiftFromStartPosition;
        }

        private Vector3 GetMousePointedRayHit()
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Physics.Raycast(ray, out var hit);
            return hit.point;
        }
    }
}
