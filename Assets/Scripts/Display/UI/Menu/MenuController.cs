using System.Collections;
using Display.UI.Menu.Buttons;
using UnityEngine;
// ReSharper disable Unity.NoNullPropagation

namespace Display.UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        private const float TransitionDuration = 0.5f;
        public SidePanelController sidePanel;
        public PointerController pointer;

        public void MoveDown()
        {
            StartCoroutine(MoveCoroutine(-((RectTransform)transform).rect.height));
        }

        public void MoveUp()
        {
            StartCoroutine(MoveCoroutine(((RectTransform)transform).rect.height));
        }

        private IEnumerator MoveCoroutine(float deltaY, float duration = TransitionDuration)
        {
            sidePanel.Active = false;
            var elapsedTime = 0f;
            var startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                transform.localPosition = new Vector2
                (
                    startingPosition.x,
                    startingPosition.y + deltaY * Mathf.Pow(elapsedTime / duration, 2)
                );
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector2
            (
                startingPosition.x,
                startingPosition.y + deltaY
            );
            sidePanel.Active = true;
            pointer.CurrentButton?.AlignPointer();
        }
    }
}
