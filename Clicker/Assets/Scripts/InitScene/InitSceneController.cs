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
            DataManager.Instance.OnInitializationFinished += InitializationFinishedHandler;
        }

        void InitializationFinishedHandler()
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
