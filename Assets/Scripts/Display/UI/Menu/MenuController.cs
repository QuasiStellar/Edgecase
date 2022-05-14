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

        public void MoveLeft()
        {
            StartCoroutine(MoveCoroutine(-((RectTransform)transform).rect.width));
        }

        public void MoveRight()
        {
            StartCoroutine(MoveCoroutine(((RectTransform)transform).rect.width));
        }

        private IEnumerator MoveCoroutine(float deltaX, float duration = TransitionDuration)
        {
            sidePanel.Active = false;
            var elapsedTime = 0f;
            var startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                transform.localPosition = new Vector2
                (
                    startingPosition.x + deltaX * Mathf.Pow(elapsedTime / duration, 2),
                    startingPosition.y
                );
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector2
            (
                startingPosition.x + deltaX,
                startingPosition.y
            );
            sidePanel.Active = true;
            pointer.CurrentButton?.AlignPointer();
        }
    }
}
