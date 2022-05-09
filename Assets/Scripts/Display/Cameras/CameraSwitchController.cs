using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Display.Cameras
{
    public class CameraSwitchController : MonoBehaviour
    {
        public GameObject cam;
        public GameObject debugCam;
        public PlayerInput playerInput;

        private void Start()
        {
            playerInput.actions["SwitchCameras"].performed += _ => SwitchCameras();
        }

        private void SwitchCameras()
        {
            var toDebug = playerInput.currentActionMap.name == "InGame";
            cam.SetActive(!toDebug);
            debugCam.SetActive(toDebug);
            playerInput.SwitchCurrentActionMap(toDebug ? "InGameDebug" : "InGame");
        }
    }
}
