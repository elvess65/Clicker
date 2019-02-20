using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplemathTarget : MonoBehaviour
{
    float forceMltp = 1;
    float friction = 1.5f;
    Vector3 forceDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (forceMltp >= 0)
        {
            transform.position = transform.position + forceDir * forceMltp * Time.deltaTime;
            forceMltp -= Time.deltaTime * friction;
        }
    }

    public void AddForce(Vector3 force)
    {
        forceMltp = 1;
        //GetComponent<Rigidbody>().AddForce(force);
        forceDir = force;
    }
}
