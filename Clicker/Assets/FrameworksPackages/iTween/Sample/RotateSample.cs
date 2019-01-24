using UnityEngine;
using System.Collections;

namespace FrameworkPackage.iTween
{
    public class RotateSample : MonoBehaviour
    {
        void Start()
        {
            iTween.RotateBy(gameObject, iTween.Hash("x", .25, "easeType", "easeInOutBack", "loopType", "pingPong", "delay", .4));
        }
    }
}

