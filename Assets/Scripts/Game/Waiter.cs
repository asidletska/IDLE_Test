using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Waiter : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private bool isBusy;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("walk", true);
    }

    public IEnumerator ServeVisitor(Visitor visitor, Transform table, Chef chef)
    {
        if (isBusy) yield break;
        isBusy = true;

        visitor.GoToTable(table);
        yield return new WaitUntil(() => !visitor.GetComponent<NavMeshAgent>().pathPending &&
                                          visitor.GetComponent<NavMeshAgent>().remainingDistance < 0.2f);

        agent.SetDestination(chef.GetCookingPoint().position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.2f);

        bool ready = false;
        yield return StartCoroutine(chef.CookMeal(() => ready = true));
        while (!ready) yield return null;

        agent.SetDestination(table.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.2f);

        visitor.StartEating(Random.Range(4f, 7f));

        isBusy = false;
    }
}

