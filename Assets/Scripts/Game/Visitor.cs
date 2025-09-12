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
    }

    private void Update()
    {
        animator.SetBool("walk", agent.velocity.magnitude > 0.1f);
    }

    public void GoToQueue(Vector3 position)
    {
        agent.SetDestination(position); 
        currentState = State.GoingToQueue;
    }

    public void InQueue()
    {
        currentState = State.InQueue;
        animator.SetBool("walk", false);
        animator.SetBool("idle", true);
    }

    public void GoToTable(Transform table)
    {
        currentState = State.GoingToTable;
        agent.SetDestination(table.position);
        target = table;
    }

    public void GoToToilet(Transform toilet)
    {
        currentState = State.GoingToToilet;
        agent.SetDestination(toilet.position);
        target = toilet;
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
        GameManager.Instance.AddMoney(100);

        yield return new WaitForSeconds(duration);

        IsEating = false;
        animator.SetBool("idle", false);

        if (target != null)
            RestaurantManager.Instance.FreeTable(target);

        LeaveRestaurant();
    }

    public void LeaveRestaurant()
    {
        currentState = State.Leaving;
        Vector3 exit = RestaurantManager.Instance.GetExitPoint().position;
        agent.SetDestination(exit);
        Destroy(gameObject, 5f);
    }
}
