using UnityEngine;
// ReSharper disable Unity.NoNullPropagation

namespace Display
{
    public class HexBoardController : MonoBehaviour
    {
        private GameObject _selected;

        public Camera cam;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit)) return;
                if (hit.collider.gameObject.name != "Hex") return;
                var newHex = hit.collider.gameObject;
                _selected?.GetComponent<HexController>().Deselect();
                _selected = newHex;
                newHex.GetComponent<HexController>().Select();
            }
        }
    }
}
