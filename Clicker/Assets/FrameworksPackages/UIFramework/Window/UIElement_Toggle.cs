using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// Реализация Toggle, которая позволяет производить производить переключение при условии
    /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class UIElement_Toggle : MonoBehaviour
    {
        public System.Action<bool> OnTryChangeValue;

        [Header("Images")]
        public Image Image_Checkbox;
        [Header("Buttons")]
        public Button Button_Main;
        [Header("Settings")]
        public bool IsToggled = false;

        private bool m_Value = false;


        public void Init()
        {
            m_Value = IsToggled;
            SetValue(m_Value);

            Button_Main.onClick.AddListener(Button_Main_PressHandler);
        }

        public void SetValue(bool isToggled)
        {
            m_Value = isToggled;
            UpdateVisual();
        }

        public void PlayErrorAnimation()
        {
            StartCoroutine(Wait());
        }


        void Button_Main_PressHandler()
        {
            OnTryChangeValue?.Invoke(!m_Value);
        }

        void UpdateVisual()
        {
            Image_Checkbox.gameObject.SetActive(m_Value);
        }

        IEnumerator Wait()
        {
            Image bg = Image_Checkbox.transform.parent.gameObject.GetComponent<Image>();
            Color initColor = bg.color;
            bg.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            bg.color = initColor;
        }
    }
}
