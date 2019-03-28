using clicker.datatables;
using System.Collections.Generic;
using UnityEngine;
using static clicker.datatables.DataTableItems;

namespace clicker.general.ui.windows
{
    public class UIElement_Tab_Craft_Population : UIElement_Tab_Craft
    {
        public RectTransform ProgressParent;
        public UIElement_Workers WorkersController;

        private Dictionary<DataTableItems.ItemTypes, UIElement_PopulationProgressItem> m_PopulationUIIems;

        public override void InitTab()
        {
            if (!m_IsInitialized)
            {
                Debug.Log("UIElement_Tab_Craft_Population: InitTab");

                UpdatePopulationItems();
                WorkersController.Init();

                base.InitTab();
            }
            else
                UpdateTabState();

            SubscribeForEvents();
        }

        protected override void ItemCrafted_Handler(DataTableItems.ItemTypes craftedItemType)
        {
            base.ItemCrafted_Handler(craftedItemType);

            DataTableItems.Item itemData = DataTableItems.GetItemDataByType(craftedItemType);

            //Если созданный предмет удовлетворяет текущему фильтру - добавить население
            if (itemData.MatchFilter(ItemFilterTypes.Population))
            {
                //Добавить население
                GameManager.Instance.Manager_Battle.PopulationManager.AddPopulation(craftedItemType);

                //Обновить локальную панель 
                UpdatePopulationItems();
            }
            else //Если был создан какой-то другой предмет - обновить текущее состояние вкладки
                UpdateTabState();
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

        protected override void UpdateTabState()
        {
            base.UpdateTabState();

            //Обновить всем UI элементам населения состояние потребляемых ресурсов
            UpdateAbsorbedResourceItems();

            //Обновить состояние панели рабочих
            WorkersController.UpdateState();
        }


        void UpdatePopulationItems()
        {
            if (m_PopulationUIIems == null)
                m_PopulationUIIems = new Dictionary<DataTableItems.ItemTypes, UIElement_PopulationProgressItem>();

            //Пройтись по всему доступному населению
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
                {
                    //Вывести текущий множитель скорости
                    m_PopulationUIIems[itemType].SetMultiplayer(GameManager.Instance.Manager_Battle.PopulationManager.GetMultiplayerForPopulation(itemType));

                    //Обновить состояние потребляемых ресурсов
                    m_PopulationUIIems[itemType].UpdateAbsorbedResourceItems();
                }
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

            UpdateTabState();
        }

        void PopulationPerdionFinished_PopulationReduce(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_PopulationReduce");

            UpdateTabState();

            if (m_PopulationUIIems.ContainsKey(itemType))
                m_PopulationUIIems[itemType].SetMultiplayer(GameManager.Instance.Manager_Battle.PopulationManager.GetMultiplayerForPopulation(itemType));
        }

        void PopulationPerdionFinished_PopulationLose(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_PopulationLose");

            UpdateTabState();

            if (m_PopulationUIIems.ContainsKey(itemType))
            {
                Destroy(m_PopulationUIIems[itemType].gameObject);
                m_PopulationUIIems.Remove(itemType);
            }
        }

        void UpdateAbsorbedResourceItems()
        {
            foreach (UIElement_PopulationProgressItem item in m_PopulationUIIems.Values)
                item.UpdateAbsorbedResourceItems();
        }
    }
}
