using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Visitor : MonoBehaviour
{
    public enum State { GoingToQueue, InQueue, GoingToTable, Eating, GoingToToilet, Leaving }

    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;
    private State currentState;

    public bool IsEating { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = 0.3f; 
    }

    private void Update()
    {
        animator.SetBool("walk", agent.velocity.magnitude > 0.1f);
        if (currentState == State.GoingToQueue && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            InQueue();
            animator.SetBool("idle", true);
        }
    }

    public void GoToQueue(Vector3 queuePos)
    {
        currentState = State.GoingToQueue;
        agent.SetDestination(queuePos);
    }

    public void InQueue()
    {
        currentState = State.InQueue;
        animator.SetBool("walk", false);
    }

    public void GoToTable(Transform table)
    {
        currentState = State.GoingToTable;
        target = table;
        agent.SetDestination(table.position);
    }

    public void GoToToilet(Transform toilet)
    {
        currentState = State.GoingToToilet;
        target = toilet;
        agent.SetDestination(toilet.position);
    }

    public void StartEating(float duration)
    {
        StartCoroutine(EatingRoutine(duration));
    }

    private IEnumerator EatingRoutine(float duration)
    {
        currentState = State.Eating;
        IsEating = true;
        animator.SetBool("idle", true);

        yield return new WaitForSeconds(duration);

        IsEating = false;
        GameManager.Instance.AddMoney(100);
        animator.SetBool("idle", false);

        if (Random.value < 0.3f)
        {
            var toilet = RestaurantManager.Instance.GetFreeToilet();
            if (toilet != null)
            {
                GoToToilet(toilet);
                yield break;
            }
        }

        LeaveRestaurant();
    }

    public void LeaveRestaurant()
    {
        currentState = State.Leaving;
        Vector3 exit = RestaurantManager.Instance.GetExitPoint().position;
        agent.SetDestination(exit);
        Destroy(gameObject, 5f);
        RestaurantManager.Instance.OnVisitorLeft();
    }
}
