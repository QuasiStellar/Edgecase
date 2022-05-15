using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Display.UI.Menu.Buttons
{
    public abstract class ButtonController :
        MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        private const float GlowValue = 0.25f;
        private const float PointerMovingDuration = 0.15f;

        public PointerController pointer;
        public SidePanelController sidePanel;
        private bool _pressing;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!sidePanel.Active) return;

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
            pointer.CurrentButton = this;
            if (!sidePanel.Active) return;

            MovePointer();
            if (_pressing)
            {
                TurnOnGlow();
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            pointer.CurrentButton = null;
            TurnOffGlow();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            Press();
        }

        public void AlignPointer()
        {
            if (pointer.Dismissed)
            {
                MovePointer();
            }
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

        protected virtual bool Press()
        {
            return sidePanel.Active;
        }
    }
}
