using UnityEngine.SceneManagement;

namespace Display.UI.Menu
{
    public class PlayButtonController : MenuButtonController
    {
        public override void Press()
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
