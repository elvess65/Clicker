﻿using FrameworkPackage.UI.Windows;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// Кнопка, позволяющая добавлять слоты оружия 
    /// Выводит количество необходимых нажатий для добавления слота
    /// </summary>
    public class UIElement_AddWeaponSlot : UIElement_ClickableItem
    {
        public Text Text_Title;
        public Text Text_Progress;
        public Image Image_Progress;

        public void Init(int curStep, int totalSteps)
        {
            Text_Title.text = "Add Slot";
            UpdateProgress(curStep, totalSteps);
        }

        public void UpdateProgress(int curStep, int totalStep)
        {
            Text_Progress.text = string.Format("{0}/{1}", curStep, totalStep);
            Image_Progress.fillAmount = curStep / (float)totalStep;
        }
    }
}