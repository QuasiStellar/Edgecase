using UnityEngine;

public class HexController : MonoBehaviour
{
    private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");
    
    public void Select()
    {
        GetComponent<MeshRenderer>().material.SetColor
        (
            EdgeColor,
            new Color(52 / 256f, 204 / 256f, 255 / 256f)
        );
    }

    public void Deselect()
    {
        GetComponent<MeshRenderer>().material.SetColor
        (
            EdgeColor,
            new Color(42 / 256f, 42 / 256f, 42 / 256f)
        );
    }
    
    public void SetParent(GameObject parent)
    {
        transform.SetParent(parent.transform);
    }
}