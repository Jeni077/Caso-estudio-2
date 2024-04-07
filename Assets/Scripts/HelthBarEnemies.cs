using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelthBarEnemies : MonoBehaviour
{
    public float health = 100;
    public float healthMax = 100;


    [Header("Interfaz")]
    public Slider HealthBar;

    public void Update()
    {
        ActualizarInterfaz();
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ActualizarInterfaz()
    {
        HealthBar.value = health / healthMax;
    }

}
