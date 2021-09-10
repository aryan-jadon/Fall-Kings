using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class WarriorAI : MonoBehaviour
{

	// AI
	CharacterCombat aicharacterCombat;
	public float lookRadius = 10f;

	// navmesh
	private NavMeshAgent navMeshAgent;

	// player
	CharacterStats playerCombact;
	public Transform target;

	// PlayerManager playerManager;


	// animation
	public WarriorAnimationBehaviour playerAnimationBehaviour;


    private void Awake()
    {
		
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.speed = Random.Range(3F, 5F);

		aicharacterCombat = GetComponent<CharacterCombat>();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, lookRadius);
	}


	// Start is called before the first frame update
	private void Start()
    {
		// playerCombact = playerManager.player.GetComponent<CharacterStats>();
	}

	private void Update()
	{
		float distance = Vector3.Distance(target.position, transform.position);

		if (distance <= lookRadius)
        {
			// if (playerCombact.GetCharacterHealth() > 0 )
            
				navMeshAgent.SetDestination(target.position);
				FaceTarget();
				playerAnimationBehaviour.PlayRunAnimation();

				if (distance <= navMeshAgent.stoppingDistance)
				{
				
					// aicharacterCombat.Attack(playerCombact);
					playerAnimationBehaviour.PlayAttackAnimation();

				}	
		}
        else
        {
			playerAnimationBehaviour.PlayIdleAnimation();
		}

	}

	public void FaceTarget()
    {
		Vector3 direction = (target.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
	

}
