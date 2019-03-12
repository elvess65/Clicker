using clicker.datatables;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Population : UIElement_Tab_Craft
    {
        public RectTransform ProgressParent;

        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Population: InitTab");

                UpdatePopulationItems();

                base.InitTab();
            }
            else
                UpdateTabState();

            SubscribeForEvents();
        }

        protected override void ItemCrafted_Handler(DataTableItems.ItemTypes craftedItemType)
        {
            base.ItemCrafted_Handler(craftedItemType);

            //Добавить население
            GameManager.Instance.Manager_Battle.PopulationManager.AddPopulation(craftedItemType);

            //Обновить локальную панель 
            UpdatePopulationItems();
        }

        protected override void SubscribeForEvents()
        {
            base.SubscribeForEvents();

            GameManager.Instance.Manager_Battle.PopulationManager.OnPopulationProgressChanged += PopulationProgressChangedHandler;
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodFinishedWithSuccess += PopulationPerdionFinished_Success;
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodFinishedWithPopulationReduce += PopulationPerdionFinished_PopulationReduce;
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodFinishedWithPopulationLose += PopulationPerdionFinished_PopulationLose;
        }

        protected override void UnscribeFromEvents()
        {
            base.UnscribeFromEvents();

            GameManager.Instance.Manager_Battle.PopulationManager.OnPopulationProgressChanged -= PopulationProgressChangedHandler;
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodFinishedWithSuccess -= PopulationPerdionFinished_Success;
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodFinishedWithPopulationReduce -= PopulationPerdionFinished_PopulationReduce;
            GameManager.Instance.Manager_Battle.PopulationManager.OnPeriodFinishedWithPopulationLose -= PopulationPerdionFinished_PopulationLose;
        }


        private Dictionary<DataTableItems.ItemTypes, UIElement_PopulationProgressItem> m_PopulationUIIems;
        void UpdatePopulationItems()
        {
            if (m_PopulationUIIems == null)
                m_PopulationUIIems = new Dictionary<DataTableItems.ItemTypes, UIElement_PopulationProgressItem>();

            DataTableItems.ItemTypes[] population = DataManager.Instance.PlayerAccount.Inventory.GetItemsByFilterType(DataTableItems.ItemFilterTypes.Population);
            for (int i = 0; i < population.Length; i++)
            {
                DataTableItems.ItemTypes itemType = population[i];

                //Если такого типа населения еще нет - создать
                if (!m_PopulationUIIems.ContainsKey(population[i]))
                {
                    UIElement_PopulationProgressItem item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_PopulationProgressItemPrefab, ProgressParent);
                    item.Init(population[i], 
                              GameManager.Instance.Manager_Battle.PopulationManager.GetProgressForPopulation(population[i]),
                              GameManager.Instance.Manager_Battle.PopulationManager.GetMultiplayerForPopulation(population[i]));

                    m_PopulationUIIems.Add(population[i], item);
                }
                else //Если такой тип населения есть - обновить данные
                    m_PopulationUIIems[itemType].SetMultiplayer(GameManager.Instance.Manager_Battle.PopulationManager.GetMultiplayerForPopulation(itemType));
            }
        }


        void PopulationProgressChangedHandler(DataTableItems.ItemTypes itemType, float progress)
        {
            if (m_PopulationUIIems.ContainsKey(itemType))
                m_PopulationUIIems[itemType].SetProgress(progress);
        }

        void PopulationPerdionFinished_Success(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_Success");

            PeriodFinishedHandler();

            if (m_PopulationUIIems.ContainsKey(itemType))
                m_PopulationUIIems[itemType].SetStatus(false);
        }

        void PopulationPerdionFinished_PopulationReduce(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_PopulationReduce");

            PeriodFinishedHandler();

            if (m_PopulationUIIems.ContainsKey(itemType))
            {
                m_PopulationUIIems[itemType].SetMultiplayer(GameManager.Instance.Manager_Battle.PopulationManager.GetMultiplayerForPopulation(itemType));
                m_PopulationUIIems[itemType].SetStatus(true);
            }
        }

        void PopulationPerdionFinished_PopulationLose(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_PopulationLose");

            PeriodFinishedHandler();

            if (m_PopulationUIIems.ContainsKey(itemType))
            {
                Destroy(m_PopulationUIIems[itemType].gameObject);
                m_PopulationUIIems.Remove(itemType);
            }
        }

        void PeriodFinishedHandler()
        {
            //Пройтись по всем UI объектам и обновить количество предметов, а так же состояние требуемых для создания предметов
            foreach (UIElement_CraftItem item in m_Items.Values)
            {
                //Количество предметов
                item.SetItemAmount(DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(item.Type));

                //Состояние требуемых для создания предметов
                item.UpdateRequireItemsState();
            }
        }
    }
}
