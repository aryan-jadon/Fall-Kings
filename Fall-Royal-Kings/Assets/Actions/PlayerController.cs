using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Sub Behaviours")]
    public WarriorMovementBehaviour playerMovementBehaviour;
    public WarriorAnimationBehaviour playerAnimationBehaviour;

    public Rigidbody playerRigidbody;
    private Player playerInput;
    public int playerHealth;
    bool attackManager = true;
    bool playerManager = true;


    public float movementSmoothingSpeed = 1f;
    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;

    // attack settings
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackPower;
    public LayerMask enemyLayers;


    private void Awake()
    {
        playerInput = new Player();
        playerHealth = 100;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

   
    // Start is called before
    // the first frame update
    void Start()
    {

    }


    public int GetPlayerHealth()
    {
        return playerHealth;
    }



    public void GotAttacked(int damagePoints)
    {

        playerHealth -= damagePoints;
        playerAnimationBehaviour.PlayHitAnimation();

        if (playerHealth <= 0 && playerManager == true) 
        {
         
            playerAnimationBehaviour.PlayDeathAnimation();
            playerManager = false;

            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            gameObject.GetComponent<Collider>().enabled = false;

        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    IEnumerator WaitCoroutine()
    {
        attackManager = false;

        yield return new WaitForSeconds(0.8f);

        attackManager = true;
    }


    public void PlayerAttack()
    {
        if (playerHealth > 0)
        {

            if (attackManager == true)
            {

                playerAnimationBehaviour.PlayAttackAnimation();
                StartCoroutine(WaitCoroutine());

                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

                foreach (Collider enemy in hitEnemies)
                {
                    if (enemy.name != gameObject.name)
                    {
                        try
                        {
                            enemy.GetComponent<BotsKing>().GotAttacked(attackPower);
                            
                        }
                        catch (System.Exception e)
                        {
                            Debug.Log(e);
                        }

                        try
                        {

                            enemy.GetComponent<PlayerController>().GotAttacked(attackPower);
                            

                        }
                        catch (System.Exception e)
                        {
                            Debug.Log(e);
                        }

                    }

                }
            }

        }

    }



    // Update is called once per frame
    void Update()
    {
        if (GetPlayerHealth() > 0)
        {
            Vector2 movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();
            rawInputMovement = new Vector3(movementInput.x, 0, movementInput.y);

            CalculateMovementInputSmoothing();
            UpdatePlayerMovement();
            UpdatePlayerAnimationMovement();
        }
        else
        {
            OnDisable();
      
        }
    }

    //Input's Axes values are raw
    void CalculateMovementInputSmoothing()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement,
            rawInputMovement, Time.deltaTime * movementSmoothingSpeed);

    }

    void UpdatePlayerMovement()
    {
        playerMovementBehaviour.UpdateMovementData(smoothInputMovement);
    }


    void UpdatePlayerAnimationMovement()
    {
        playerAnimationBehaviour.UpdateMovementAnimation(smoothInputMovement.magnitude);
    }


}
