using UnityEngine;

public class CameraSwitchController : MonoBehaviour
{
    public GameObject cam;
    public GameObject debugCamera;

    private void Start()
    {
        cam.SetActive(true);
        debugCamera.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cam.SetActive(!cam.activeSelf);
            debugCamera.SetActive(!debugCamera.activeSelf);
        }
    }
}