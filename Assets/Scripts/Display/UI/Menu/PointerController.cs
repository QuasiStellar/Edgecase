using System.Collections;
using Display.UI.Menu.Buttons;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Display.UI.Menu
{
    public class PointerController : MonoBehaviour
    {
        public bool Dismissed { get; private set; }
        [CanBeNull] public ButtonController CurrentButton { get; set; }
        private const float AppearingDuration = 0.25f;
        private const float AppearingIndent = 100f;
        private bool _coroutineIsExecuting;
        private float _normalX;
        private IEnumerator _executingCoroutine;
        private TextMeshProUGUI _mesh;

        private void Start()
        {
            _mesh = GetComponent<TextMeshProUGUI>();
            Dismissed = false;
            _normalX = transform.localPosition.x;
        }

        public void MoveTowardsButton(GameObject button, float duration)
        {
            if (_coroutineIsExecuting)
            {
                StopCoroutine(_executingCoroutine);
            }

            if (Dismissed)
            {
                AlignWithButton(button);
                Appear();
            }
            else
            {
                _executingCoroutine = MoveTowardsButtonCoroutine(button, duration);
                StartCoroutine(_executingCoroutine);
            }
        }

        public void Disappear()
        {
            if (_coroutineIsExecuting)
            {
                StopCoroutine(_executingCoroutine);
            }

            Dismissed = true;
            StartCoroutine(DisappearCoroutine());
        }

        private void Appear()
        {
            if (_coroutineIsExecuting)
            {
                StopCoroutine(_executingCoroutine);
            }

            Dismissed = false;
            StartCoroutine(AppearCoroutine());
        }

        private void AlignWithButton(GameObject button)
        {
            var t = transform;
            var buttonPosition = button.transform.localPosition;
            t.localPosition = new Vector3(t.localPosition.x, buttonPosition.y, buttonPosition.z);
        }

        private IEnumerator MoveTowardsButtonCoroutine(GameObject button, float duration)
        {
            _coroutineIsExecuting = true;

            var position = transform.localPosition;
            var currentY = position.y;
            var targetY = button.transform.localPosition.y;
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                transform.localPosition = new Vector3
                (
                    position.x,
                    currentY + (targetY - currentY) * Mathf.Pow(elapsedTime / duration, 2),
                    position.z
                );
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(position.x, targetY, position.z);

            _coroutineIsExecuting = false;
        }

        private IEnumerator DisappearCoroutine(float duration = AppearingDuration, float indent = AppearingIndent)
        {
            _coroutineIsExecuting = true;

            var position = transform.localPosition;
            var currentX = position.x;
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                transform.localPosition = new Vector3
                (
                    currentX - indent * Mathf.Pow(elapsedTime / duration, 2),
                    position.y,
                    position.z
                );
                _mesh.color = new Color32(255, 255, 255, (byte)(255f * (1 - Mathf.Pow(elapsedTime / duration, 2))));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(position.x - indent, position.y, position.z);
            _mesh.color = new Color32(255, 255, 255, 0);

            _coroutineIsExecuting = false;
        }

        private IEnumerator AppearCoroutine(float duration = AppearingDuration, float indent = AppearingIndent)
        {
            _coroutineIsExecuting = true;

            var t = transform;
            var localPosition = t.localPosition;
            localPosition = new Vector3(_normalX - indent, localPosition.y, localPosition.z);
            t.localPosition = localPosition;
            var position = localPosition;
            var currentX = position.x;
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                transform.localPosition = new Vector3
                (
                    currentX + indent * (1 - Mathf.Pow(1 - elapsedTime / duration, 2)),
                    position.y,
                    position.z
                );
                _mesh.color = new Color32(255, 255, 255, (byte)(255f * Mathf.Pow(elapsedTime / duration, 2)));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(position.x + indent, position.y, position.z);
            _mesh.color = new Color32(255, 255, 255, 255);

            _coroutineIsExecuting = false;
        }
    }
}
