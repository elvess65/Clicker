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
        public MovePathController PathMoveController;

        public int Damage => 10;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="health">HP врага</param>
        /// <param name="speed">Скорость врага</param>
        /// <param name="pathController">Контроллер пути, по которому должен следовать враг</param>
        public void Init(int health, float speed, PathCreator pathController)
        {
            //Базовая инициализация
            Init(health);

            //Обнулить положение ХП бра (TEMP)
            HPController.ResetHPBarPosition();

            //Начать следовать по пути
            PathMoveController.StartMoveAlongPath(pathController, speed, GameManager.Instance.TimeMultiplayerController.CurValue);

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
    }
}
