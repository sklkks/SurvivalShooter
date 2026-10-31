using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Zombie : LivingEntity
{
    public enum Status
    {
        Idle,
        Trace,
        Attack,
        Death
    }

    public Transform target;
    public LayerMask targetLayer;

    public float traceDistance = 10f;
    public float attackDistance = 1.5f;

    public Collider zombieCollider;
    public Renderer zombieRenderer;
    public AudioClip hitClip;
    public AudioClip deathClip;

    private NavMeshAgent agent;
    private Animator zombieAnimator;
    private AudioSource zombieAudioSource;

    public float damage = 10;
    public float attackInteval = 1.0f;
    private float lastAttackTime;
    private Status currentStatus;

    public static readonly int IdHasTarget = Animator.StringToHash("HasTarget");
    public static readonly int IdDeath = Animator.StringToHash("Death");
    public static readonly int IdAttack = Animator.StringToHash("Attack");

    // 클래스 상단 변수 선언부 적당한 곳에 추가
private bool isTargetInRange = false;

private void OnTriggerStay(Collider other)
{
    if (IsDead) return;

    if (((1 << other.gameObject.layer) & targetLayer) != 0)
    {
        isTargetInRange = true;
    }
}

private void OnTriggerExit(Collider other)
{
    if (((1 << other.gameObject.layer) & targetLayer) != 0)
    {
        isTargetInRange = false;
    }
}

    public Status CurrentStatus
    {
        get => currentStatus;
        set
        {
            currentStatus = value;            
            switch (currentStatus)
            {
                case Status.Idle:
                     zombieAnimator.SetBool(IdHasTarget, false);
                     agent.isStopped = true;
                    break;
                case Status.Trace:
                     zombieAnimator.SetBool(IdHasTarget, true);
                     agent.isStopped = false;
                    break;
                case Status.Attack:
                    zombieAnimator.SetBool(IdHasTarget, false);
                    agent.isStopped = true;
                    break;
                case Status.Death:
                    agent.isStopped = true;
                    zombieAnimator.SetTrigger(IdDeath);
                    zombieCollider.enabled = false;
                    zombieAudioSource.PlayOneShot(deathClip);
                    break;
            }
        }
    }

    public void Setup(ZombieData data)
    {
        startingHealth = data.maxHp;
        damage = data.damage;
        agent.speed = data.speed;
        zombieRenderer.material.color = data.skinColor;
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();
        zombieAudioSource = GetComponent<AudioSource>();
    }
 
    private void Update()
    {
        switch (currentStatus)
            {
                case Status.Idle:
                    UpdateIdle();
                    break;
                case Status.Trace:
                    UpdateTrace();
                    break;
                case Status.Attack:
                    UpdateAttack();
                    break;
                case Status.Death:
                    UpdateDeath();
                    break;
            }
    }
    
    private void UpdateIdle()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < traceDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        target = FindTarget(traceDistance);
    }


    private void UpdateAttack()
    {
        if (!isTargetInRange || target == null)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        if (Time.time >= lastAttackTime + attackInteval)
        {
            lastAttackTime = Time.time;

            IDamageable damageable = target.GetComponent<IDamageable>();
            var hitNormal = (transform.position - target.position).normalized;
            damageable.OnDamage(damage, target.position, hitNormal);
        }
    }

    public void Attack()
    {
        Debug.Log("Attack");
    }

    private void UpdateTrace()
    {
        if (target == null)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        agent.SetDestination(target.position);

        if (isTargetInRange)
        {
            CurrentStatus = Status.Attack;
        }
    }

    private void UpdateDeath()
    {
    }

    private Transform FindTarget(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, targetLayer);

        Transform closet = null;
        float closetDistance = float.MaxValue;

        for (int i = 0; i < colliders.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (distance < closetDistance)
            {
                closetDistance = distance;
                closet = colliders[i].transform;
            }
        }


        return closet;
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
        
        if (agent.isOnNavMesh)
        {
            agent.ResetPath();
        }

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        zombieCollider.enabled = true;
        isTargetInRange = false;

        target = GameObject.FindWithTag("Player")?.transform;
        if (target != null)
        {
            CurrentStatus = Status.Trace;
        }
        else
        {
            CurrentStatus = Status.Idle;
        }
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
        {
            return;
        }
        base.OnDamage(damage, hitPoint, hitNormal);

        zombieAudioSource.PlayOneShot(hitClip);                       
    }

    public override void Death()
    {
        base.Death();

        CurrentStatus = Status.Death;
    }

    public void StartSinking()
    {
        Collider collider = GetComponent<Collider>();
        
        if(collider != null)
        {
            collider.enabled = false;
        }
        Destroy(gameObject, 2f);
    }
}