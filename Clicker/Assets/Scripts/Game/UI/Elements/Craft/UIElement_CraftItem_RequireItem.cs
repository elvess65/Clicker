﻿using clicker.datatables;
using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI необходимого для создания предмета обїекта
    /// </summary>
    public class UIElement_CraftItem_RequireItem : MonoBehaviour
    {
        public Text Text_RequireItem;

        private DataTableItems.ItemTypes m_Type;
        private int m_Amount;

        public void Init(DataTableItems.ItemTypes type, int amount)
        {
            m_Type = type;
            m_Amount = amount;

            Text_RequireItem.text = string.Format("{0}: {1}", type, amount);

            UpdateState();
        }

        public void UpdateState()
        {
            bool accountHasItem = DataManager.Instance.PlayerAccount.Inventory.HasAmountOfItem(m_Type, m_Amount);
            Text_RequireItem.color = accountHasItem ? Color.black : Color.red;
        }
    }
}