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
    public class DebugCameraController : MonoBehaviour
    {
        public PlayerInput playerInput;

        /// Normal speed of camera movement.
        private const float MovementSpeed = 50f;

        /// Speed of camera movement when control is held down.
        private const float FastMovementSpeed = 200f;

        /// Sensitivity for free look.
        private const float FreeLookSensitivity = 0.5f;

        private void Update()
        {
            var inputMoveVector = playerInput.actions["Move"].ReadValue<Vector2>();

            var fastMode = playerInput.actions["FastMode"].IsPressed();
            var movementSpeed = fastMode ? FastMovementSpeed : MovementSpeed;

            var t = transform;
            t.position += t.right * (movementSpeed * Time.deltaTime * inputMoveVector.x) +
                          t.up * (movementSpeed * Time.deltaTime * inputMoveVector.y);

            if (!playerInput.actions["Looking"].IsPressed()) return;
            var inputLookVector = playerInput.actions["LookFree"].ReadValue<Vector2>();

            var localEulerAngles = t.localEulerAngles;

            var newRotationX = localEulerAngles.y + inputLookVector.x * FreeLookSensitivity;
            var newRotationY = localEulerAngles.x - inputLookVector.y * FreeLookSensitivity;

            t.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }
    }
}
