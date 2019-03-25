using UnityEngine;

namespace clicker.battle
{
    /// <summary>
    /// Объект награды, который выпадает из врага
    /// </summary>
    public class Behaviour_CoinReward : PickableObject
    {
        public System.Action<int> OnPick;

        private int m_Reward;

        public void Init(int reward)
        {
            m_Reward = reward;
        }

        public override void Pick()
        {
            OnPick?.Invoke(m_Reward);
            Destroy(gameObject);
        }
    }
}
