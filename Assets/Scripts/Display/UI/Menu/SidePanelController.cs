using UnityEngine;

namespace Display.UI.Menu
{
    public class SidePanelController : MonoBehaviour
    {
        public bool Active { get; set; }

        private void Start()
        {
            Active = true;
        }
    }
}
