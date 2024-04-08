using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;

public class MiddlewareController : MonoBehaviour
{
    public UnityEvent OnAttack;
    PlayerController2D playerController2D;

    public void OnMeleeAttack()
    {
        OnAttack?.Invoke();
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))

        {
            if(playerController2D != null)
            {
                playerController2D.OnMeleeAttack();
            }
        }
    }
}
