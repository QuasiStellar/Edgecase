using UnityEngine;

namespace Display.UI.Menu
{
    public class SettingsButtonController : MenuButtonController
    {
        public GameObject mainMenu;
        public GameObject settingsMenu;
        public override void Press()
        {
            pointer.Disappear();
            mainMenu.SetActive(false);
            settingsMenu.SetActive(true);
        }
    }
}
