using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    public class UIElement_WeaponSlot : MonoBehaviour
    {
        public Text Text_ItemName;
        public Text Text_Amount;
        public Image Image_DurabilityProgress;

        public void Init(datatables.DataTableItems.ItemTypes type, int amount)
        {
            Text_ItemName.text = type.ToString();
            Text_Amount.text = amount != 0 ? amount.ToString() : string.Empty;
        }

        void Update()
        {

        }
    }
}
