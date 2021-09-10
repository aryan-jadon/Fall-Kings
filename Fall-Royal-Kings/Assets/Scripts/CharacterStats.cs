using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int characterHealth = 100;
    public int damagePower = 10;
    public WarriorAnimationBehaviour playerAnimationBehaviour;

    private void Awake()
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

    public int GetCharacterHealth()
    {
        return characterHealth;
    }


    public void TakeDamage(int value)
    {
        characterHealth -= value;
        Debug.Log(characterHealth);
    }

    public void HitAnimation()
    {
        playerAnimationBehaviour.PlayHitAnimation();
    }

}
