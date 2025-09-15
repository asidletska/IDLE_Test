using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chef : MonoBehaviour
{

    [SerializeField] private Transform fridgePoint;
    [SerializeField] private Transform stovePoint;
    [SerializeField] private Transform counterPoint;
    [SerializeField] private float cookingTime = 3f;

    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0.3f;
    }

    public Transform GetFridgePoint() => fridgePoint;

    public IEnumerator CookMeal(System.Action onMealReady)
    {
        agent.SetDestination(fridgePoint.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.4f);

        agent.SetDestination(stovePoint.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.4f);

        animator?.SetTrigger("idle");
        yield return new WaitForSeconds(cookingTime);

        agent.SetDestination(counterPoint.position);
        yield return new WaitUntil(() => agent.remainingDistance < 0.4f);

        onMealReady?.Invoke();
    }

    public void UpgradeChef(float timeReduction)
    {
        cookingTime = Mathf.Max(1f, cookingTime - timeReduction);
    }
}
