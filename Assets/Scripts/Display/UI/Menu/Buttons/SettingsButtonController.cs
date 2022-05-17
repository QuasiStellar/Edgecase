namespace Display.UI.Menu.Buttons
{
    public class SettingsButtonController : ButtonController
    {
        public MenuController settingsMenu;

        protected override bool Press()
        {
            if (!base.Press()) return false;

            base.Press();

            pointer.Disappear();

            sidePanel.activeController.MoveRight();
            settingsMenu.MoveLeft();

            sidePanel.lastActiveController = sidePanel.activeController;
            sidePanel.activeController = settingsMenu;

            return true;
        }
    }
}
