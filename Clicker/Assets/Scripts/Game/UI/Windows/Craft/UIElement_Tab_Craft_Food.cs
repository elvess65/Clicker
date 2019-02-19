using UnityEngine;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Food : UIElement_Tab_Craft
    {
        private UIElement_FoodSlotsController m_FoodSlotsController;

        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Food: InitTab");

                base.InitTab();
            }
            else
                UpdateTabState();

            SubscribeForEvents();
        }
    }
}
