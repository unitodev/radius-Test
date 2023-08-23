using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour,IDamageable
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    // Component
    private Animator m_Anim;
    private NavMeshAgent m_Agent;

    // Enemy
    private Transform m_Target;

    [SerializeField]
    public int HP = 100;
    private int Atk = 10;
    private int AtkRange = 2;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (HP <= 0)
        {
            m_Anim.Play(Die);
        }
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
    }
}