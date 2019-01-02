using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// Вкладка окна
    /// </summary>
    public abstract class UITabContent : UIObject
    {
        public virtual void InitTab()
        {
            if (!m_IsInitialized)
                Init();
        }
    }
}
