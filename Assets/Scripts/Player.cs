using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour,IDamageable
{
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");

    [SerializeField] private LayerMask m_Enemy;
    [SerializeField] private LayerMask m_Ground;

    // Component
    private Animator m_Anim;
    private NavMeshAgent m_Agent;

    // Enemy
    private Transform m_Target;
[SerializeField]
   public int HP= 100;
    int Atk= 10;
    private int AtkRange = 2;
    
    private float delay = .5f;
    private float tempdelay ;
    [SerializeField]
    private state _currentState;
    enum state
    {
        Idle,chase,attack,died
    }
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();

        tempdelay = delay;
    }

    // Update is called once per frame
    private void Update()
    {
        // Left Click on Enemy
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, m_Enemy))
            {
                m_Target = hit.collider.transform;
                m_Anim.Play(Walk);

                m_Agent.SetDestination(m_Target.position);
                _currentState = state.chase;

            }
        }

       
        // Right Click on Ground
        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_Ground))
            {
                m_Target = null;
                m_Anim.Play(Walk);

                m_Agent.SetDestination(hit.point);
                _currentState = state.Idle;
            }
        }

        if (HP <= 0)
            _currentState = state.died;
        switch (_currentState)
        {
            case state.chase:
                m_Anim.Play(Walk);
                if (m_Agent.remainingDistance < AtkRange)
                {
                   
                    _currentState = state.attack;
                }
              
                break;
           case state.attack:
               if (m_Agent.remainingDistance > AtkRange)
               {
                   m_Agent.SetDestination(m_Target.position);  
                   _currentState = state.chase;
               }
               
              
               m_Anim.Play(Attack);
               
               if (tempdelay>0)
               {
                   tempdelay-=Time.deltaTime;
                   
               }
               else
               {
                   AttackEnnemy();
                   tempdelay = delay;
               }
               

               break;
           case state.died:
               m_Anim.Play(Die);
               break;
        }
    }

    void AttackEnnemy()
    {
        Collider[] enemisToDamage =
            Physics.OverlapSphere(transform.position, AtkRange, m_Enemy);
        
        for (int item = 0; item < enemisToDamage.Length; item++)
        {
            if(enemisToDamage[item].gameObject!=this)
            enemisToDamage[item].GetComponent<IDamageable>().TakeDamage(Atk);
            Debug.Log($"Attack to {enemisToDamage[item].gameObject} + {Atk} damage");
        }

      
    }
    void OnDrawGizmos()
    {
       //  Gizmos.color = Color.red;
       //  //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
       // // if (m_Started)
       //      //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
       //      Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    public void TakeDamage(int damage)
    {

        HP -= damage;
    }
}