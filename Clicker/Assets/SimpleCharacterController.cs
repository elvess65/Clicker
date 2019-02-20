using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterController : MonoBehaviour
{
    public float Speed = 5;

    private Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        //if (Input.GetKey(KeyCode.W))
        {
            Vector3 moveVelocity = transform.forward * Speed;
            moveVelocity.y = m_Rigidbody.velocity.y;
            //m_Rigidbody.velocity = moveVelocity;
            m_Rigidbody.MovePosition(m_Rigidbody.position + moveVelocity * Time.deltaTime);
        }
        SetDirection(Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, Input.mousePosition.z));
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Vector3 dirToTarget = (collision.gameObject.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dirToTarget * 1000);
        }*/
    }

    public void SetDirection(Vector2 dir)
    {
        float targetRotAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        m_Rigidbody.rotation = Quaternion.AngleAxis(targetRotAngle, Vector3.up);
    }

    /*void OnTriggerEnter(Collider col)
    {
        Vector3 pustDir = new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);
        if (col.attachedRigidbody != null)
            col.attachedRigidbody.velocity = pustDir;
    }*/
}
