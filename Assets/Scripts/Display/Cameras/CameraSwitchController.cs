using UnityEngine;

public class CameraSwitchController : MonoBehaviour
{
    public GameObject cam;
    public GameObject debugCamera;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            cam.SetActive(!cam.activeSelf);
            debugCamera.SetActive(!debugCamera.activeSelf);
        }
    }
}