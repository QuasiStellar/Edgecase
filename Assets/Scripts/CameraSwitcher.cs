using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject freeCamera;
    public GameObject topDownCamera;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            freeCamera.SetActive(!freeCamera.activeSelf);
            topDownCamera.SetActive(!topDownCamera.activeSelf);
        }
    }
}
