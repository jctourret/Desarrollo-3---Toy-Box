﻿using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemi AI")]
    public static Action<GameObject> OnEnemySpawn;
    public Camera cam;

    protected NavMeshAgent agent;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    [SerializeField] protected Rigidbody rbody;
    [SerializeField] protected bool hasAttacked;
    [SerializeField] bool targetInAttackRange;
    [SerializeField] float fallLimitY = -20f;

    public GameObject target;

    protected bool isDead;
    bool lastDirRecorded = false;
    protected bool right;
    protected bool up;
    Collider coll;
    
    //======================================

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        coll = GetComponentInChildren<Collider>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        rbody.isKinematic = true;
        OnEnemySpawn?.Invoke(gameObject);
    }

    protected virtual void Update()
    {
        animator.SetFloat("Horizontal",agent.velocity.x);
        animator.SetFloat("Vertical", agent.velocity.z);
        animator.SetFloat("Magnitude",agent.velocity.magnitude);

        if (agent.velocity.magnitude < 0.1)
        {
            if (!lastDirRecorded)
            {
                animator.SetFloat("LastDirX", agent.velocity.x);
                animator.SetFloat("LastDirY", agent.velocity.z);
                lastDirRecorded = true;
            }
        }
        else
        {
            animator.SetFloat("Horizontal", agent.velocity.x);
            animator.SetFloat("Vertical", agent.velocity.z);
            lastDirRecorded = false;
        }

        right = agent.velocity.normalized.x <= 0;
        up = agent.velocity.normalized.z > 0;

        if (right && up) // right up
        {
            spriteRenderer.flipX = true;
        }
        else if (right && !up) // right down
        {
            spriteRenderer.flipX = false;
        }
        else if (!right && up) //left up
        {
            spriteRenderer.flipX = false;
        }
        else if (!right && !up) //left down
        {
            spriteRenderer.flipX = true;
        }


        AkSoundEngine.PostEvent("arana_pasos", gameObject);

        DeleteSpiderWhenFall();
    }

    private void LateUpdate()
    {
        Vector3 camAdjusted = cam.transform.position;
        camAdjusted.y = 0.0f;
        transform.LookAt(camAdjusted);
    }

    //=======================================

    void GetCamera(Camera newCamera)
    {
        cam = newCamera;
    }

    protected void DeleteSpiderWhenFall()
    {
        if (this.transform.position.y <= fallLimitY)
        {
            Destroy(this.gameObject);
        }
    }

    protected Vector3 initialVelocity(Vector3 origin, Vector3 target, float height)
    {
        //Calculo distancias del tiro oblicuo
        float distanceY = target.y - origin.y;
        Vector3 distanceXZ = new Vector3(target.x - origin.x,
            0.0f, target.z - origin.z);

        //Calculo tiempos del tiro oblicuo
        float timeUp = Mathf.Sqrt(-2 * height / Physics.gravity.y);
        float timeDown = Mathf.Sqrt(2 * (distanceY - height) / Physics.gravity.y);

        //Calculo Velocidades del tiro oblicuo
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * height * Physics.gravity.y);
        Vector3 velocityXZ = distanceXZ / (timeUp + timeDown);

        return velocityY + velocityXZ;
    }
    public void Fall()
    {
        if(agent != null)
        {
            agent.enabled = false;
        }
        if(rbody != null)
        {
            rbody.isKinematic = false;
        }
    }

    public void KillSpider(bool state)
    {
        isDead = state;
    }
}
