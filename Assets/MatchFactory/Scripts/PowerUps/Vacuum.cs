using System;
using UnityEngine;


public class Vacuum : PowerUp
{
    [Header("Elements")]
    [SerializeField] private Animator animator;


    [Header("Actions")]
    public static Action started;

    private void TriggerPowerUpStart()
    {
        started?.Invoke();
    }

    public void Play()
    {
        animator.Play("Activate");
    }

}
