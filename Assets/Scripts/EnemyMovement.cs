using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Target;
    public float UpdateSpeed = 0.1f;
    private NavMeshAgent Agent;

    private void Start()
    {
        StartCoroutine(FollowTarget());
    }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }
    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(UpdateSpeed);

        while(enabled)
        {
            if(Target != null)
            {
                Agent.SetDestination(Target.position);
            }
            yield return wait;
        }
    }
}
