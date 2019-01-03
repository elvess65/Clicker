using UnityEngine;
using UnityEngine.UI;

namespace clicker.general.ui
{
    /// <summary>
    /// UI объекта, который отображает информацию о предмете
    /// </summary>
    public class UIElement_CraftItem : MonoBehaviour
    {
        public System.Action<datatables.DataTableItems.ItemTypes> OnItemPress;

        [Header("Texts")]
        public Text Text_ItemName;
        public Text Text_Amount;
        [Header("Images")]
        public Image Image_ItemIcon;
        public Image Image_Progress;

        private datatables.DataTableItems.ItemTypes m_Type;

        private float m_CurTime;
        private float m_TotalTime = 0.2f;
        private bool m_IsAnimating = false;
        private float m_TargetProgress;
        private float m_CurProgress;

        private bool m_ItemCrafted = false;
        private int m_AmountAfterCraft;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="type">Тип предмета</param>
        /// <param name="amount">Количество предметов</param>
        /// <param name="progress">Текущий прогресс создания</param>
        public void Init(datatables.DataTableItems.ItemTypes type, int amount, float progress)
        {
            //Тип предмета
            m_Type = type;

            //Имя и количество предметов
            Text_ItemName.text = type.ToString();
            Text_Amount.text = amount.ToString();

            //Текущий прогресс создания
            SetProgress(progress);

            //Подписаться на нажатие 
            GetComponent<Button>().onClick.AddListener(Button_PressHandler);
        }

        /// <summary>
        /// Обновить прогресс при добавлении тика
        /// </summary>
        /// <param name="progress">Прогресс</param>
        public void UpdateProgress_AddTick(float progress)
        {
            StartAnimation(progress);
        }

        /// <summary>
        /// Обновить прогресс при крафте
        /// </summary>
        /// <param name="amount">Новое количество предметов</param>
        public void UpdateProgress_Craft(int amount)
        {
            //Пометить текущую анимацию как анимацию крафта (так де блокирует возможность нажататия до окончания анимации)
            m_ItemCrafted = true;
            //Запомнить новое количество предметов (для вывода после анимации)
            m_AmountAfterCraft = amount;

            //Начать анимацию
            StartAnimation(1);
        }


        /// <summary>
        /// Задать прогресс
        /// </summary>
        /// <param name="progress">Прогресс</param>
        void SetProgress(float progress)
        {
            Image_Progress.fillAmount = progress;
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

        private void Update()
        {
            if (m_IsAnimating)
            {
                m_CurTime += Time.deltaTime;

                float progress = Mathf.Lerp(m_CurProgress, m_TargetProgress, m_CurTime / m_TotalTime);
                SetProgress(progress);

                if (m_CurTime >= m_TotalTime)
                {
                    m_IsAnimating = false;
                    
                    //Если это была анимация после  крафта
                    if (m_ItemCrafted)
                    {
                        //Снять пометку анимации для крафта (разблокировать возможность нажатия)
                        m_ItemCrafted = false;

                        //Вывести новое количество предметов
                        Text_Amount.text = m_AmountAfterCraft.ToString();

                        //Обнулить прогесс
                        SetProgress(0);
                    }
                }
            }
        }


        void Button_PressHandler()
        {
            if (m_ItemCrafted)
                return;

            OnItemPress?.Invoke(m_Type);
        }
    }
}
