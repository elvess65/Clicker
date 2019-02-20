using System;
using UnityEngine;

public class SimpleAIController : MonoBehaviour
{
    public float Speed = 5;
    public Vector3 Dir = Vector3.right;

    private Rigidbody m_Rigidbody;
    private DateTime m_MoveDirTime;
    private int m_Sign = 1;
    private int m_MoveTime = 4;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        m_MoveDirTime = DateTime.Now.AddSeconds(m_MoveTime / 2);
    }

    void FixedUpdate()
    {
        if ((m_MoveDirTime - DateTime.Now).TotalSeconds <= 0)
        {
            m_MoveDirTime = DateTime.Now.AddSeconds(m_MoveTime);
            m_Sign *= -1;
        }

        Move(Dir * m_Sign);
    }

    void Move(Vector3 dir)
    {
        Vector3 moveVelocity = dir * Speed;
        moveVelocity.y = m_Rigidbody.velocity.y;
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveVelocity * Time.deltaTime);

        SetDirection(dir.normalized);
    }

    public void SetDirection(Vector2 dir)
    {
        float targetRotAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        m_Rigidbody.rotation = Quaternion.AngleAxis(targetRotAngle, Vector3.up);
    }
}
