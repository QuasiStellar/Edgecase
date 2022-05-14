namespace Display.UI.Menu.Buttons
{
    public class SettingsButtonController : ButtonController
    {
        public MenuController mainMenu;
        public MenuController settingsMenu;

        protected override bool Press()
        {
            if (!base.Press()) return false;

            base.Press();
            pointer.Disappear();
            mainMenu.MoveRight();
            settingsMenu.MoveLeft();
            return true;
        }
    }
}
