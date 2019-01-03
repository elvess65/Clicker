using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.datatables
{
    public class DataTableWeapons 
    {
        #region Data Structures
        public class Weapon
        {
            private ItemTypes m_Type;                           //Тип предмета
            private int m_Damage;
            private int m_Durability;

            public ItemTypes Type => m_Type;
        }
        #endregion
    }
}