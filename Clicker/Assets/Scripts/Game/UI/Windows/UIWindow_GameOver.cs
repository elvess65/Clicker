using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui.windows
{
    public class UIWindow_GameOver : UIWindow_CloseButton
    {
        public System.Action OnButtonClosePress;
        public System.Action OnButtonContinueAdPress;
        public System.Action OnButtonContinueChildPress;

        [Header("Texts")]
        public Text Text_Continue;
        public Text Text_Continue_Ad_Description;
        public Text Text_Continue_Child_Description;
        [Header("Buttons")]
        public Button Button_Continue_Ad;
        public Button Button_Continue_Child;

        public override void Show()
        {
            base.Show();

            Text_Continue_Ad_Description.text = "Do you want to find adopter and continue?";
            Text_Continue_Child_Description.text = "U have 2 child. Do U want them to continue Yours?";

            Button_Close.onClick.AddListener(Button_Close_PressHandler);
            Button_Continue_Ad.onClick.AddListener(Button_Continue_Ad_PressHandler);
            Button_Continue_Child.onClick.AddListener(Button_Continue_Child_PressHandler);
        }

        public void SetContinueStatus(bool canWatchAd, int childrenAmount)
        {
            if (!canWatchAd)
                Button_Continue_Ad.transform.parent.gameObject.SetActive(false);

            if (childrenAmount == 0)
                Button_Continue_Child.transform.parent.gameObject.SetActive(false);
        }

        void Button_Close_PressHandler()
        {
            OnButtonClosePress?.Invoke();
        }

        void Button_Continue_Ad_PressHandler()
        {
            OnButtonContinueAdPress?.Invoke();

            Hide();
        }

        void Button_Continue_Child_PressHandler()
        {
            OnButtonContinueChildPress?.Invoke();

            Hide();
        }
    }
}
