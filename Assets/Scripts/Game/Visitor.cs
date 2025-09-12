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
        animator.SetBool("walk", true);
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
        animator.SetBool("idle", true);
    }
    public void GoToTable(Transform table)
    {
        animator.SetBool("walk", true);
        currentState = State.GoingToTable;
        agent.SetDestination(table.position);
        target = table;
    }

    public void GoToToilet(Transform toilet)
    {
        animator.SetBool("walk", true);
        currentState = State.GoingToToilet;
        GameManager.Instance.AddMoney(100);
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
        GameManager.Instance.AddMoney(100);
        IsEating = true;
        animator.SetBool("Sit", true);
        yield return new WaitForSeconds(duration);
        IsEating = false;
        RestaurantManager.Instance.FreeTable(target);
        LeaveRestaurant();
    }

    public void LeaveRestaurant()
    {
        animator.SetBool("Sit", false);
        animator.SetBool("walk", true);
        currentState = State.Leaving;
        Vector3 exit = RestaurantManager.Instance.GetExitPoint().position;
        agent.SetDestination(exit);
        Destroy(gameObject, 5f);
    }
}
