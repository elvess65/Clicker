using clicker.general;
using FrameworkPackage.UI;
using System.Collections;
using UnityEngine;

namespace clicker.initScene
{
    public class InitSceneController : MonoBehaviour
    {
        public LevelLoader LevelLoader;

        void Start()
        {
            if (DataManager.Instance.IsInitialized)
                LoadLevel();
            else
                DataManager.Instance.OnInitializationFinished += InitializationFinishedHandler;
        }

        void InitializationFinishedHandler()
        {
            if (DataManager.Instance.IsInitialized)
                return;

            LoadLevel();
        }

        void LoadLevel()
        {
            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1);

            LevelLoader.LoadLevel(1);
        }
    }
}
