using System.Collections.Generic;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.datatables
{
    public class LocalItemsDataEditor : MonoBehaviour
    {
        public List<ItemsData> Data_Items;
        public List<ItemsCraftIgnoreData> Data_CraftIgnoreItems;

        [Header("Editor")]
        public Color Color_Filters;
        public Color Color_RequiredItems;
        public Color Color_SelectedCraftIgnoreItem;
        public FilterColorData[] Color_Filter;

      
        [System.Serializable]
        public struct FilterColorData
        {
            public ItemFilterTypes Filter;
            public Color Color;
        }
    }
}
