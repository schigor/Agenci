using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    private Camera Camera;
    private UnityEngine.AI.NavMeshAgent Agent;

    private RaycastHit[] raycastHits = new RaycastHit[1];

    private void Awake()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.RaycastNonAlloc(ray, raycastHits) > 0)
        {
            Agent.SetDestination(raycastHits[0].point);
        }
    }
}
