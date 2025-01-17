﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSpiderAI : EnemyAI
{
    [Header("RangedAttack")]
    [SerializeField] float rangedCooldown = 1;
    [SerializeField] float rangedCooldownTimer;
    [SerializeField] float rangedRange = 3;
    [SerializeField] float shotHeight = 1.5f;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform shotPoint;
    // Update is called once per frame
    new void Update()
    {
        base.Update();
        float distance;
        if (agent != null && target != null && isDead == false)
        {
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= rangedRange)
            {
                if (!hasAttacked)
                {
                    AkSoundEngine.PostEvent("arana_lanza_tela", gameObject);

                    GameObject go = Instantiate(projectile, shotPoint.position, Quaternion.identity, null);

                    Rigidbody rb = go.GetComponent<Rigidbody>();

                    rb.velocity = initialVelocity(shotPoint.position, target.transform.position, target.transform.position.y + shotHeight);

                    agent.isStopped = true;
                    hasAttacked = true;
                }
            }
            else
            {
                if (agent.isActiveAndEnabled)
                {
                    agent.isStopped = false;
                    agent.SetDestination(target.transform.position);
                }
            }
            if (hasAttacked)
            {
                if (rangedCooldownTimer < rangedCooldown)
                {
                    rangedCooldownTimer += Time.deltaTime;
                }
                else
                {
                    hasAttacked = false;
                    rangedCooldownTimer = 0.0f;
                }
            }
        }
    }
}
