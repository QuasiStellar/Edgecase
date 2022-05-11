﻿namespace Display.UI.Menu.Buttons
{
    public class BackButtonController : ButtonController
    {
        public MenuController mainMenu;
        public MenuController settingsMenu;

        protected override bool Press()
        {
            if (!base.Press()) return false;

            pointer.Disappear();
            mainMenu.MoveUp();
            settingsMenu.MoveUp();
            return true;
        }
    }
}
