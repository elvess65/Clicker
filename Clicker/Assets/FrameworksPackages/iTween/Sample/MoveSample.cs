using UnityEngine;
using System.Collections;

namespace FrameworkPackage.iTween
{
    public class MoveSample : MonoBehaviour
    {
        void Start()
        {
            iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
        }
    }
}

