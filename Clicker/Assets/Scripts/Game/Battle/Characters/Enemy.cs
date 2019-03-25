using clicker.battle.HP;
using clicker.general;
using FrameworkPackage.PathCreation;
using PathCreation;

namespace clicker.battle.character
{
    /// <summary>
    /// Класс врага
    /// </summary>
    public class Enemy : Character
    {
        public System.Action<int> OnPickReward;

        public MovePathController PathMoveController;

        private int m_CoinReward;

        public int Damage => 10;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="health">HP врага</param>
        /// <param name="speed">Скорость врага</param>
        /// <param name="pathController">Контроллер пути, по которому должен следовать враг</param>
        public void Init(int health, float speed, PathCreator pathController, int coinReward)
        {
            //Базовая инициализация
            Init(health);

            //Обнулить положение ХП бра (TEMP)
            HPController.ResetHPBarPosition();

            //Начать следовать по пути
            PathMoveController.StartMoveAlongPath(pathController, speed, GameManager.Instance.TimeMultiplayerController.CurValue);

            //Награда
            m_CoinReward = coinReward;

            //Подписаться на событие изменения изменения скорости мнодителя времени
            GameManager.Instance.OnTimeMultiplayerValueChanged += TimeMultiplayerChanged_Handler;
        }

        /// <summary>
        /// Обработка столкновения с игроком (нанесение урона игроку)
        /// </summary>
        public void DestroyEnemyOnCollisionWithPlayer()
        {
            DestroyObjectHandler();
        }


        /// <summary>
        /// Уничтожение объекта
        /// </summary>
        protected override void DestroyObjectHandler()
        {
            base.DestroyObjectHandler();

            //Выпадение награды
            if (m_CoinReward > 0)
            {
                Behaviour_CoinReward coinReward = Instantiate(GameManager.Instance.AssetsLibrary.CoinRewardPrefab);
                coinReward.transform.position = transform.position;
                coinReward.OnPick += PickReward_Handler;
                coinReward.Init(m_CoinReward);
            }

            //Отписаться от события множителя времени
            GameManager.Instance.OnTimeMultiplayerValueChanged -= TimeMultiplayerChanged_Handler;
            Destroy(gameObject);
        }

        /// <summary>
        /// Объет HP Bar
        /// </summary>
        protected override HPBarController GetHPBarControllerPrefab()
        {
            return GameManager.Instance.Manager_UI.WindowsManager.EnemyHPBarControllerPrefab;
        }


        /// <summary>
        /// Обработка изменения множителя времени
        /// </summary>
        /// <param name="curValue">Текущее значение множителя времени</param>
        void TimeMultiplayerChanged_Handler(float curValue)
        {
            PathMoveController.SetMultiplyer(curValue);
        }

        /// <summary>
        /// Обработка поднятия награды
        /// </summary>
        /// <param name="reward">Награда</param>
        void PickReward_Handler(int reward)
        {
            OnPickReward?.Invoke(reward);
        }
    }
}
