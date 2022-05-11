using UnityEngine;

namespace Display.UI.Menu.Buttons
{
    public class ExitButtonController : ButtonController
    {
        protected override bool Press()
        {
            if (!base.Press()) return false;

            Application.Quit();
            return true;
        }
    }
}
