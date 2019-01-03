using UnityEngine;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Weapons : UIElement_Tab_Craft
    {
        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Weapons: InitTab");

                base.InitTab();
            }
        }
    }
}
