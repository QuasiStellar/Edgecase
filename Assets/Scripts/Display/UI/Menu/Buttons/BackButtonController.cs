namespace Display.UI.Menu.Buttons
{
    public class BackButtonController : ButtonController
    {
        protected override bool Press()
        {
            if (!base.Press()) return false;

            pointer.Disappear();

            sidePanel.lastActiveController.MoveLeft();
            sidePanel.activeController.MoveRight();

            (sidePanel.lastActiveController, sidePanel.activeController)
                = (sidePanel.activeController, sidePanel.lastActiveController);

            return true;
        }
    }
}
