using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    public class UIElement_Coins : MonoBehaviour
    {
        public Text Text_Amount;

        public void UpdateAmount(int amount)
        {
            Text_Amount.text = amount.ToString();
        }
    }
}
