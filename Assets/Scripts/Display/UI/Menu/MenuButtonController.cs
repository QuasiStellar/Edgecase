using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Display.UI.Menu
{
    public abstract class MenuButtonController :
        MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        private const float GlowValue = 0.25f;
        private const float PointerMovingDuration = 0.15f;

        public PointerController pointer;
        private bool _pressing;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _pressing = true;
            TurnOnGlow();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            _pressing = false;
            TurnOffGlow();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            MovePointer();
            if (_pressing)
            {
                TurnOnGlow();
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            TurnOffGlow();
        }

        private void TurnOnGlow(float value = GlowValue)
        {
            GetComponent<TextMeshProUGUI>().fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, value);
            pointer.GetComponent<TextMeshProUGUI>().fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, value);
        }

        private void TurnOffGlow()
        {
            GetComponent<TextMeshProUGUI>().fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
            pointer.GetComponent<TextMeshProUGUI>().fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
        }

        private void MovePointer(float duration = PointerMovingDuration)
        {
            pointer.MoveTowardsButton(gameObject, duration);
        }

        public abstract void Press();
    }
}
