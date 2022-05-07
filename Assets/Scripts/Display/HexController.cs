using UnityEngine;

namespace Display
{
    public class HexController : MonoBehaviour
    {
        private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Select()
        {
            _meshRenderer.material.SetColor
            (
                EdgeColor,
                new Color(52 / 256f, 204 / 256f, 255 / 256f)
            );
        }

        public void Deselect()
        {
            _meshRenderer.material.SetColor
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
}