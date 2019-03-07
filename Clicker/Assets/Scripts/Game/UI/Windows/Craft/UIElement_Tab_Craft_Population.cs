using clicker.datatables;
using UnityEngine;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Population : UIElement_Tab_Craft
    {
        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Population: InitTab");

                base.InitTab();
            }
            else
                UpdateTabState();

            SubscribeForEvents();
        }

        protected override void ItemCrafted_Handler(DataTableItems.ItemTypes craftedItemType)
        {
            base.ItemCrafted_Handler(craftedItemType);

            Debug.Log("Population crafted");
            GameManager.Instance.Manager_Battle.PopulationManager.AddPopulation(craftedItemType);
        }

        protected override void SubscribeForEvents()
        {
            base.SubscribeForEvents();

            GameManager.Instance.Manager_Battle.PopulationManager.OnPopulationProgressChanged += PopulationProgressChangedHandler;
        }

        protected override void UnscribeFromEvents()
        {
            base.UnscribeFromEvents();

            GameManager.Instance.Manager_Battle.PopulationManager.OnPopulationProgressChanged -= PopulationProgressChangedHandler;
        }


        void PopulationProgressChangedHandler(DataTableItems.ItemTypes itemType, float progress)
        {
            Debug.Log(itemType + " " + progress);
        }
    }
}
