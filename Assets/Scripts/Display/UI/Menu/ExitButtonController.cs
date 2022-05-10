using UnityEngine;

namespace Display.UI.Menu
{
    public class ExitButtonController : MenuButtonController
    {
        public override void Press()
        {
            Application.Quit();
        }
    }
}
