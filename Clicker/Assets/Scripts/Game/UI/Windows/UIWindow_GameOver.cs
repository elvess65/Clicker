using FrameworkPackage.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui.windows
{
    public class UIWindow_GameOver : UIWindow_CloseButton
    {
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

            Button_Continue_Ad.onClick.AddListener(Button_Continue_Ad_PressHandler);
            Button_Continue_Child.onClick.AddListener(Button_Continue_Child_PressHandler);
        }

        void Button_Continue_Ad_PressHandler()
        {
            Debug.Log("Button_Continue_WatchAd_PressHandler");
        }

        void Button_Continue_Child_PressHandler()
        {
            Debug.Log("Button_Continue_Child_PressHandler");
        }
    }
}
