using clicker.datatables;
using FrameworkPackage.UI.Windows;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace clicker.general.ui
{
    /// <summary>
    /// UI объекта, который отображает информацию о предмете
    /// </summary>
    public class UIElement_CraftItem : UIElement_ClickableItem
    {
        public System.Action<DataTableItems.ItemTypes, bool> OnAutoCraftToggled;
        public System.Action<DataTableItems.ItemTypes> OnItemPress;
        public System.Action<PointerEventData, DataTableItems.ItemTypes> OnPoinerDownEvent;
        public System.Action<PointerEventData, DataTableItems.ItemTypes> OnDragEvent;
        public System.Action<PointerEventData, DataTableItems.ItemTypes> OnPointerUpEvent;

        [Header("Texts")]
        public Text Text_ItemName;
        public Text Text_Amount;
        [Header("Images")]
        public Image Image_ItemIcon;
        public Image Image_Progress;
        [Header("Required Items")]
        public RectTransform RequiredItemsParent;
        [Header("Auto Craft")]
        public UIElement_Toggle Toggle_AutoCraft;
        public Image Image_AutoCraftProgress;
        [Header("Other")]
        public UIElement_CraftItem_DraggableIcon UIElement_DraggableIcon;

        private float m_CurTime;
        private float m_TotalTime = 0.2f;
        private bool m_IsAnimating = false;
        private float m_TargetProgress;
        private float m_CurProgress;

        private bool m_ItemCrafted = false;
        private int m_AmountAfterCraft;
        private bool m_CanDrag = false;

        private Dictionary<DataTableItems.ItemTypes, UIElement_CraftItem_RequireItem> m_RequiredItems;

        public DataTableItems.ItemTypes Type { get; private set; }

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <param name="amount">Количество предметов</param>
        /// <param name="progress">Текущий прогресс создания</param>
        public void Init(DataTableItems.ItemTypes type, int amount, float progress, bool allowAutocraft, DataTableItems.ItemAmountContainer[] requiredItems)
        {
            //Тип предмета
            Type = type;

            //Иконка предмета
            Image_ItemIcon.sprite = GameManager.Instance.AssetsLibrary.GetSprite_Item(type);

            //Имя и количество предметов
            Text_ItemName.text = type.ToString();
            SetItemAmount(amount);

            //Текущий прогресс создания
            SetItemProgress(progress);

            //Вывод необходимых для создания предметов
            m_RequiredItems = new Dictionary<DataTableItems.ItemTypes, UIElement_CraftItem_RequireItem>();

            if (requiredItems.Length == 0)
                RequiredItemsParent.gameObject.SetActive(false);
            else
            {
                for (int i = 0; i < requiredItems.Length; i++)
                {
                    UIElement_CraftItem_RequireItem item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_CraftRequireItemPrefab, RequiredItemsParent);
                    item.Init(requiredItems[i].Type, requiredItems[i].Amount);

                    m_RequiredItems.Add(requiredItems[i].Type, item);
                }
            }

            if (!allowAutocraft)
                Toggle_AutoCraft.gameObject.SetActive(false);
            else
            {
                //Toggle автокрафта
                if (Toggle_AutoCraft != null)
                {
                    Toggle_AutoCraft.OnTryChangeValue += Toggle_TryChangeValueHandler;
                    Toggle_AutoCraft.Init();
                }

                //Прогресс тиков автокрафта
                if (Image_AutoCraftProgress != null)
                    Image_AutoCraftProgress.fillAmount = 0;
            }

            if (UIElement_DraggableIcon != null)
            {
                UIElement_DraggableIcon.enabled = false;
                UIElement_DraggableIcon.OnPoinerDownEvent += PointerDown_Handler;
                UIElement_DraggableIcon.OnDragEvent += Drag_Handler;
                UIElement_DraggableIcon.OnPointerUpEvent += PointerUp_Handler;
            }

            base.Init();
        }

        /// <summary>
        /// Обновить прогресс (при добавлении тика)
        /// </summary>
        /// <param name="progress">Прогресс</param>
        public void UpdateProgress(float progress)
        {
            StartAnimation(progress);
        }

        /// <summary>
        /// Обновить количество предметов (при крафте)
        /// </summary>
        /// <param name="amount">Новое количество предметов</param>
        public void UpdateAmount(int amount)
        {
            //Пометить текущую анимацию как анимацию крафта (блокирует возможность нажататия до окончания анимации)
            m_ItemCrafted = true;

            //Запомнить новое количество предметов (для вывода после анимации)
            m_AmountAfterCraft = amount;

            //Начать анимацию
            StartAnimation(1);
        }

        /// <summary>
        /// Обновить состояние необходимых для создания предмета предметов 
        /// </summary>
        public void UpdateRequireItemsState()
        {
            foreach (UIElement_CraftItem_RequireItem item in m_RequiredItems.Values)
                item.UpdateState();
        }

        /// <summary>
        /// Обновить количество предмета
        /// </summary>
        /// <param name="amount">Количество предмета</param>
        public void SetItemAmount(int amount)
        {
            Text_Amount.text = amount.ToString();

            m_CanDrag = amount > 0;
        }

        /// <summary>
        /// Задать прогресс
        /// </summary>
        /// <param name="progress">Прогресс</param>
        public void SetItemProgress(float progress)
        {
            Image_Progress.fillAmount = progress;
        }

        /// <summary>
        /// Задать прогресс тику автодобывания
        /// </summary>
        public void SetAutoCraftProgress(float progress)
        {
            Image_AutoCraftProgress.fillAmount = progress;
        }


        void PointerDown_Handler(PointerEventData eventData)
        {
            if (m_CanDrag)
                OnPoinerDownEvent?.Invoke(eventData, Type);
        }

        void Drag_Handler(PointerEventData eventData)
        {
            if (m_CanDrag)
                OnDragEvent?.Invoke(eventData, Type);
        }

        void PointerUp_Handler(PointerEventData eventData)
        {
            if (m_CanDrag)
                OnPointerUpEvent?.Invoke(eventData, Type);
        }


        protected override void Button_PressHandler()
        {
            if (m_ItemCrafted)
                return;

            OnItemPress?.Invoke(Type);
        }

        /// <summary>
        /// Обработчик выбора калочки для добавления в автосбор
        /// </summary>
        void Toggle_TryChangeValueHandler(bool isToggled)
        {
            OnAutoCraftToggled?.Invoke(Type, isToggled);
        }

        /// <summary>
        /// Начать анимацию прогресса
        /// </summary>
        /// <param name="progress">Прогресс</param>
        void StartAnimation(float progress)
        {
            m_CurProgress = Image_Progress.fillAmount;
            m_TargetProgress = progress;
            m_CurTime = 0;
            m_IsAnimating = true;
        }

        void Update()
        {
            if (m_IsAnimating)
            {
                m_CurTime += Time.deltaTime;

                float progress = Mathf.Lerp(m_CurProgress, m_TargetProgress, m_CurTime / m_TotalTime);
                SetItemProgress(progress);

                if (m_CurTime >= m_TotalTime)
                {
                    m_IsAnimating = false;
                    
                    //Если это была анимация после  крафта
                    if (m_ItemCrafted)
                    {
                        //Снять пометку анимации для крафта (разблокировать возможность нажатия)
                        m_ItemCrafted = false;

                        //Вывести новое количество предметов
                        SetItemAmount(m_AmountAfterCraft);

                        //Обнулить прогесс
                        SetItemProgress(0);
                    }
                }
            }
        }
    }
}
