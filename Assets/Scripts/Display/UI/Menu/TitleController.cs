using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Display.UI.Menu
{
    public class TitleController : MonoBehaviour
    {
        private TextMeshProUGUI _mesh;

        private void Start()
        {
            _mesh = GetComponent<TextMeshProUGUI>();
            StartCoroutine(MoveTitleCoroutine(0.8f, 150f, 0f));
        }

        private IEnumerator MoveTitleCoroutine(float duration, float deltaX, float deltaY)
        {
            var elapsedTime = 0f;
            var startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                transform.localPosition = new Vector2
                (
                    startingPosition.x + deltaX * elapsedTime / duration,
                    startingPosition.y + deltaY * elapsedTime / duration
                );
                _mesh.color = new Color32(255, 255, 255, (byte)(255f * Math.Pow(elapsedTime / duration, 2)));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector2
            (
                startingPosition.x + deltaX,
                startingPosition.y + deltaY
            );
            _mesh.color = new Color32(255, 255, 255, 255);
        }
    }
}
