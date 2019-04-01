using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public data[] dataList;

    [System.Serializable]
    public struct data
    {
        public enum dataTypes
        {
            one,
            two, 
            three
        }

		public dataTypes Type;
        public int val;
        public bool isActive;
    }
}
