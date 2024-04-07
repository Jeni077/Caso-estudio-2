using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public float health = 100;
    public float healthMax = 100;

    [SerializeField]
    Animator animator;

    [Header("Interfaz")]
    public Slider HealthBar;

    public void Update()
    {
        ActualizarInterfaz();
    }
    [Header("Dead")]
    public GameObject Dead;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Instantiate(Dead);
            animator.SetTrigger("Dead");
        }
    }
    private void ActualizarInterfaz()
    {
        HealthBar.value = health / healthMax;
    }

}
