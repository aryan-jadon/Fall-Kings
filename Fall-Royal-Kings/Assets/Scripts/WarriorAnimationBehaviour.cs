using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAnimationBehaviour : MonoBehaviour
{
    [Header("Component References")]
    public Animator playerAnimator;

    public void UpdateMovementAnimation(float movementBlendValue)
    {
        playerAnimator.SetFloat("Movement", movementBlendValue);
    }

    public void PlayAttackAnimation()
    {
        playerAnimator.SetTrigger("Warrior_Attack");
    }

    public void PlayHitAnimation()
    {
        playerAnimator.SetTrigger("GetHit");
    }

    public void PlayDeathAnimation()
    {
        playerAnimator.SetTrigger("IsDead");
    }


    public void PlayRunAnimation()
    {
        playerAnimator.SetFloat("Movement", 0.5f);
    }

    public void PlayIdleAnimation()
    {
        playerAnimator.SetFloat("Movement", 0.0f);
    }

}
