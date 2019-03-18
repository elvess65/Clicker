using clicker.datatables;
using clicker.general;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.battle
{
    public class PopulationManager : MonoBehaviour
    {
        public System.Action OnPeriodChangedItemsAmount;
        public System.Action<DataTableItems.ItemTypes, float> OnPopulationProgressChanged;
        public System.Action<DataTableItems.ItemTypes> OnPeriodFinishedWithSuccess;
        public System.Action<DataTableItems.ItemTypes> OnPeriodFinishedWithPopulationReduce;
        public System.Action<DataTableItems.ItemTypes> OnPeriodFinishedWithPopulationLose;

        public enum PeriodResultStates
        {
            Success,
            PopulationReduce,
            PopulationLoose
        }

        private const float m_PERIOD = 1;                   //Время, за которое пройдет период из PopulationMultiplayer населения
        private Dictionary<DataTableItems.ItemTypes, PeriodicManager> m_PopulationPeriod;

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Init()
        {
            m_PopulationPeriod = new Dictionary<DataTableItems.ItemTypes, PeriodicManager>();

            DataTableItems.ItemTypes[] population = DataManager.Instance.PlayerAccount.Inventory.GetItemsByFilterType(DataTableItems.ItemFilterTypes.Population);
            for (int i = 0; i < population.Length; i++)
                AddPopulation(population[i]);
        }

        /// <summary>
        /// Добавить население
        /// </summary>
        /// <param name="itemType">Тип предмета</param>
        public void AddPopulation(DataTableItems.ItemTypes itemType)
        {
            if (!m_PopulationPeriod.ContainsKey(itemType))
            {
                //Создать менеджер периода и подписаться на события
                PeriodicManager pManager = gameObject.AddComponent<PeriodicManager>();

                //Событие изменения прогресса периода
                pManager.OnProgress += (float progress) =>
                { OnPopulationProgressChanged?.Invoke(itemType, progress); };

                //Сообытие окончания периода
                pManager.OnPeriodFinished += () =>
                {
                    PeriodResultStates periodResult = PeriodFinishedHandler(itemType);

                    //Вызов событий для результатов завершения периода
                    switch (periodResult)
                    {
                        case PeriodResultStates.Success:
                            OnPeriodFinishedWithSuccess?.Invoke(itemType);
                            break;
                        case PeriodResultStates.PopulationReduce:
                            OnPeriodFinishedWithPopulationReduce?.Invoke(itemType);
                            break;
                        case PeriodResultStates.PopulationLoose:
                            OnPeriodFinishedWithPopulationLose?.Invoke(itemType);
                            break;
                    }
                };

                pManager.Init(m_PERIOD, true, GetMultiplayer(itemType));
                pManager.StartPeriod();

                //Добавить в словарь тип населения
                m_PopulationPeriod.Add(itemType, pManager);
            }
            else //Если такой тип населения уже есть 
            {
                //Если период уже останавливался - запустить
                if (m_PopulationPeriod[itemType].WasStopped)
                    m_PopulationPeriod[itemType].StartPeriod();

                //Изменить множитель периода
                m_PopulationPeriod[itemType].SetMultiplyer(GetMultiplayer(itemType));
            }
        }

        /// <summary>
        /// Получить текущий прогресс для типа населения (На старте окна)
        /// </summary>
        public float GetProgressForPopulation(DataTableItems.ItemTypes itemType)
        {
            if (m_PopulationPeriod.ContainsKey(itemType))
                return m_PopulationPeriod[itemType].Progress;

            return 0;
        }

        /// <summary>
        /// Получить текущий множитель для населения
        /// </summary>
        public float GetMultiplayerForPopulation(DataTableItems.ItemTypes itemType)
        {
            if (m_PopulationPeriod.ContainsKey(itemType))
                return m_PopulationPeriod[itemType].Multiplayer;

            return 0;
        }


        PeriodResultStates PeriodFinishedHandler(DataTableItems.ItemTypes itemType)
        {
            bool hasEnoughtItems = true;
            DataTableItems.Item itemData = DataTableItems.GetItemDataByType(itemType);

            //Проверить, есть ли достаточное количество предметов для следующего периода для всего населения
            for (int i = 0; i < itemData.RequiredItems.Length; i++)
            {
                if (!DataManager.Instance.PlayerAccount.Inventory.HasAmountOfItem(itemData.RequiredItems[i].Type, 
                    itemData.RequiredItems[i].Amount * DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(itemType)))
                {
                    hasEnoughtItems = false;
                    break;
                }
            }

            //Если есть достаточное количество предметов, необходимых для поддержания периода - отнять
            if (hasEnoughtItems)
            {
                //Снять ресурсы, которые необходимы для поддержки текущего количества населения
                RemoveAbsorbedItems(itemData, itemType);

                return PeriodResultStates.Success;
            }
            else
            {
                //Уменьшить количество населения
                DataManager.Instance.PlayerAccount.Inventory.RemoveItem(itemType);

                //Если еще есть население 
                if (DataManager.Instance.PlayerAccount.Inventory.HasItem(itemType))
                {
                    //Снять ресурсы, которые необходимы для поддержки текущего количества населения
                    RemoveAbsorbedItems(itemData, itemType);

                    //Пересчитать множитель 
                    m_PopulationPeriod[itemType].SetMultiplyer(GetMultiplayer(itemType));

                    return PeriodResultStates.PopulationReduce;
                }
                else
                    m_PopulationPeriod[itemType].StopPeriod(); //Остановить период
            }

            return PeriodResultStates.PopulationLoose;
        }

        void RemoveAbsorbedItems(DataTableItems.Item itemData, DataTableItems.ItemTypes itemType)
        {
            for (int i = 0; i < itemData.RequiredItems.Length; i++)
            {
                DataManager.Instance.PlayerAccount.Inventory.RemoveItem(itemData.RequiredItems[i].Type,
                            itemData.RequiredItems[i].Amount * DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(itemType));
            }

            OnPeriodChangedItemsAmount?.Invoke();
        }

        float GetMultiplayer(DataTableItems.ItemTypes itemType)
        {
            return DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(itemType) / DataTablePeriodic.GetPeriodicDataByItemID(itemType).PopulationMultiplayer;
        }
    }
}
