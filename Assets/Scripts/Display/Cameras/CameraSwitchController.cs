using UnityEngine;

namespace Display.Cameras
{
    public class CameraSwitchController : MonoBehaviour
    {
        public GameObject cam;
        public GameObject debugCam;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                cam.SetActive(!cam.activeSelf);
                debugCam.SetActive(!debugCam.activeSelf);
            }
        }
    }
}