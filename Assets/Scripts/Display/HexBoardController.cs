using System;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable Unity.NoNullPropagation

namespace Display
{
    public class HexBoardController : MonoBehaviour
    {
        private GameObject _selected;

        public Camera cam;
        public PlayerInput playerInput;

        private void Start()
        {
            playerInput.actions["Click"].performed += _ => HandleClick();
        }

        private void HandleClick()
        {
            var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out var hit)) return;
            if (hit.collider.gameObject.name != "Hex") return;
            var newHex = hit.collider.gameObject;
            _selected?.GetComponent<HexController>().Deselect();
            _selected = newHex;
            newHex.GetComponent<HexController>().Select();
        }
    }
}
