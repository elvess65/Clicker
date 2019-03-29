using FrameworkPackage.UI.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameworkPackage.UI
{
    public class LevelLoader : MonoBehaviour
    {
        public UIAnimationController_Base AnimationController;

        private int m_TargetLevelIndex;

        void Start()
        {
            AnimationController.OnShowFinished += AnimationController_OnShowFinished;
            AnimationController.PlayAnimation(true);
        }

        public void LoadLevel(int levelIndex)
        {
            m_TargetLevelIndex = levelIndex;

            AnimationController.OnHideFinished += AnimationController_OnHideFinished;
            AnimationController.gameObject.SetActive(true);
            AnimationController.PlayAnimation(false);
        }

        //Окончание анимации входа в сцену
        void AnimationController_OnShowFinished()
        {
            AnimationController.OnShowFinished -= AnimationController_OnShowFinished;
            AnimationController.gameObject.SetActive(false);
        }

        //Окончание анимации выхода из сцены
        private void AnimationController_OnHideFinished()
        {
            AnimationController.OnHideFinished -= AnimationController_OnHideFinished;

            SceneManager.LoadScene(m_TargetLevelIndex);
        }
    }
}
