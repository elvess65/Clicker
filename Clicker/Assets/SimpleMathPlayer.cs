using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMathPlayer : MonoBehaviour
{
    public float Speed = 5;
    public Transform target;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position = transform.position + transform.forward * Speed * Time.deltaTime;

        SetDirection(Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, Input.mousePosition.z));

        
    }

    public void SetDirection(Vector2 dir)
    {
        float targetRotAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(targetRotAngle, Vector3.up);
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 dirToTarget = (other.transform.position - transform.position).normalized;


        other.GetComponent<SimplemathTarget>().AddForce(dirToTarget * Speed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, (target.transform.position - transform.position).normalized * 
            ((target.transform.position - transform.position).magnitude + 5));
    }
}
