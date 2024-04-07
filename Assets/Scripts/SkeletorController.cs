using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletorController : MonoBehaviour
{
    [SerializeField] 
    Animator animator;
    [SerializeField] 
    Transform target;

    [SerializeField] 
    float distanceToTarget;
    [SerializeField] 
    int attackRate = 1;
    [SerializeField] 
    LayerMask playerLayer;
    [SerializeField] 
    float attackCooldown = 1f;

    private float nextAttackTime;
    private MiddlewareController middlewareController;

    void Start()
    {
        middlewareController = GetComponentInChildren<MiddlewareController>();
    }

    void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, target.position);
        if (distance <= distanceToTarget)
        {
            if (Time.time >= nextAttackTime)
            {
                Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, distanceToTarget, playerLayer);
                if (playerCollider != null)
                {
                    animator.SetTrigger("attack");
                    middlewareController.OnMeleeAttack();
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }
    }
}
