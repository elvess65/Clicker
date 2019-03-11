using clicker.datatables;
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

            Debug.Log("Population crafted");
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


        void UpdatePopulationItems()
        {
            DataTableItems.ItemTypes[] population = DataManager.Instance.PlayerAccount.Inventory.GetItemsByFilterType(DataTableItems.ItemFilterTypes.Population);
            for (int i = 0; i < population.Length; i++)
            {
                UIElement_PopulationProgressItem item = Instantiate(GameManager.Instance.Manager_UI.WindowsManager.UIElement_PopulationProgressItemPrefab, ProgressParent);
                item.Init(population[i], 0.5f);
            }

            //TODO:
            //Create items
            //Create dictionary
            //Match progress with items
        }


        void PopulationProgressChangedHandler(DataTableItems.ItemTypes itemType, float progress)
        {
            Debug.Log(itemType + " " + DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(itemType) + " " + progress);

            //TODO: 
            //Update progress
        }

        void PopulationPerdionFinished_Success(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_Success");

            PeriodFinishedHandler();

            //TODO: 
            //Update progress
        }

        void PopulationPerdionFinished_PopulationReduce(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_PopulationReduce");

            PeriodFinishedHandler();

            //TODO: 
            //Update progress
        }

        void PopulationPerdionFinished_PopulationLose(DataTableItems.ItemTypes itemType)
        {
            Debug.Log(itemType + " PopulationPerdionFinished_PopulationLose");

            PeriodFinishedHandler();

            //TODO: 
            //Update progress
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
