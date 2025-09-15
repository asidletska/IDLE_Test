using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Waiter : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool isBusy;
    [SerializeField] private Transform idlePoint;
    [SerializeField] private Transform tablePoint;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.stoppingDistance = 0.3f;
    }

    private void Update()
    {
        animator.SetBool("walk", agent.velocity.magnitude > 0.1f);
    }

    public IEnumerator ServeVisitor(Visitor visitor, Transform table, Chef chef)
    {
        if (isBusy) yield break;
        isBusy = true;

        visitor.GoToTable(table);
        yield return new WaitUntil(() => visitor.GetComponent<NavMeshAgent>().remainingDistance < 0.4f);

        //agent.SetDestination(chef.GetFridgePoint().position);
        agent.SetDestination(idlePoint.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.4f);

        bool ready = false;
        yield return StartCoroutine(chef.CookMeal(() => ready = true));
        while (!ready) yield return null;

        agent.SetDestination(tablePoint.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.2f);

        visitor.StartEating(Random.Range(4f, 7f));

        if (idlePoint != null)
        {
            agent.SetDestination(idlePoint.position);
        }

        isBusy = false;
    }
    public bool IsBusy()
    {
        return isBusy;
    }
}

