using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : MonoBehaviour
{

    private CharacterStats myStats;

    public float attackSpeed = 1f;
    private float attackCoolDown = 0f;

    // Awake Method
    void Awake()
    {
        myStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        attackCoolDown -= Time.deltaTime;
    }

    public void Attack(CharacterStats targetStats)
    {
        if (attackCoolDown <= 0f)
        {
            targetStats.TakeDamage(myStats.damagePower);
            targetStats.HitAnimation();
            attackCoolDown = 1 / attackSpeed;
        }
        
    }


}
