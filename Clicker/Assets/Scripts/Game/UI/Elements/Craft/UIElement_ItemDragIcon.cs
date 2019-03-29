using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    public class UIElement_ItemDragIcon : MonoBehaviour
    {
        public Image Image_Icon;

        public void Init(datatables.DataTableItems.ItemTypes itemType)
        {
            Image_Icon.sprite = GameManager.Instance.AssetsLibrary.GetSprite_Item(itemType);
        }
    }
}
