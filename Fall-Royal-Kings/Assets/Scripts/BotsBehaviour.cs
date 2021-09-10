using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BotsBehaviour : MonoBehaviour
{

    // animation
    public WarriorAnimationBehaviour playerAnimationBehaviour;

    public float maxHealth = 100;
    public float currentHealth = 100;


    public void GotAttacked()
    {
        currentHealth -= 10;
        playerAnimationBehaviour.PlayHitAnimation();
    }

    public void Attack()
    {
        playerAnimationBehaviour.PlayAttackAnimation();
    }

    public void Die()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
