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
        animator.SetBool("walk", agent.velocity.magnitude > 0.1f);
    }

    public IEnumerator ServeVisitor(Visitor visitor, Transform table, Chef chef)
    {
        if (isBusy) yield break;
        isBusy = true;

        // крок 1: відвідувач іде до столу
        visitor.GoToTable(table);
        yield return new WaitUntil(() => !visitor.GetComponent<NavMeshAgent>().pathPending &&
                                          visitor.GetComponent<NavMeshAgent>().remainingDistance < 0.2f);

        // крок 2: офіціант іде до кухаря
        agent.SetDestination(chef.GetCookingPoint().position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.2f);

        // крок 3: кухар готує
        bool ready = false;
        yield return StartCoroutine(chef.CookMeal(() => ready = true));
        while (!ready) yield return null;

        // крок 4: офіціант несе їжу відвідувачу
        agent.SetDestination(table.position);
        yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.2f);

        // крок 5: відвідувач починає їсти
        visitor.StartEating(Random.Range(4f, 7f));

        isBusy = false;
    }

    public bool IsBusy() => isBusy;
}

