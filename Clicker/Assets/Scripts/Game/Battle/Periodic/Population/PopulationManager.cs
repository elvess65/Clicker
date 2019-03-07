using clicker.datatables;
using clicker.general;
using System.Collections.Generic;
using UnityEngine;

namespace clicker.battle
{
    public class PopulationManager : MonoBehaviour
    {
        public System.Action<DataTableItems.ItemTypes, float> OnPopulationProgressChanged;

        private const float m_PERIOD = 1;                   //Время, за которое пройдет период из m_POPULATION_MULTIPLAYER населения
        private const float m_POPULATION_MULTIPLAYER = 10;  
        private Dictionary<DataTableItems.ItemTypes, PeriodicManager> m_PopulationPeriod;

        public void Init()
        {
            m_PopulationPeriod = new Dictionary<DataTableItems.ItemTypes, PeriodicManager>();

            DataTableItems.ItemTypes[] population = DataManager.Instance.PlayerAccount.Inventory.GetItemsByFilterType(DataTableItems.ItemFilterTypes.Population);
            for (int i = 0; i < population.Length; i++)
                AddPopulation(population[i]);
        }

        public void AddPopulation(DataTableItems.ItemTypes itemType)
        {
            if (!m_PopulationPeriod.ContainsKey(itemType))
            {
                //Создать менеджер периода и подписаться на события
                PeriodicManager pManager = gameObject.AddComponent<PeriodicManager>();
                pManager.OnProgress += (float progress) => 
                { OnPopulationProgressChanged.Invoke(itemType, progress); };

                pManager.OnPeriodFinished += () =>
                { PeriodFinishedHandler(itemType); };
    
                pManager.Init(m_PERIOD, true, GetMultiplayer(itemType));
                pManager.StartPeriod();

                //Добавить в словарь тип населения
                m_PopulationPeriod.Add(itemType, pManager);
            }
            else //Если такой тип населения уже есть - изменить множитель периода
                m_PopulationPeriod[itemType].SetMultiplyer(GetMultiplayer(itemType));
        }


        void PeriodFinishedHandler(DataTableItems.ItemTypes itemType)
        {
            DataTableItems.Item itemData = DataTableItems.GetIemDataByType(itemType);
            for (int i = 0; i < itemData.RequiredItems.Length; i++)
                Debug.Log(itemData.RequiredItems[i].Type + " " + itemData.RequiredItems[i].Amount);

            //TODO: 
            //Reduce amount of required products
            //if no products
            //  reduce population
            //if no population
            //  stop loop
        }

        float GetMultiplayer(DataTableItems.ItemTypes itemType)
        {
            return DataManager.Instance.PlayerAccount.Inventory.GetItemAmount(itemType) / m_POPULATION_MULTIPLAYER;
        }
    }
}
