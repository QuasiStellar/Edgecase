using UnityEngine.SceneManagement;

namespace Display.UI.Menu.Buttons
{
    public class PlayButtonController : ButtonController
    {
        protected override bool Press()
        {
            if (!base.Press()) return false;

            SceneManager.LoadScene("GameScene");
            return true;
        }
    }
}
