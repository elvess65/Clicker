using UnityEngine;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Ammo : UIElement_Tab_Craft
    {
        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Ammo: InitTab");

                base.InitTab();
            }
            else
                UpdateTabState();

            SubscribeForEvents();
        }
    }
}
