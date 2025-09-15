using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Toilet : MonoBehaviour
{
    [SerializeField] private ParticleSystem dirtyEffect;
    [SerializeField] private float useTime = 3f;

    private bool isBusy;
    private bool isDirty;

    public bool IsAvailable => !isBusy && !isDirty;

    public IEnumerator UseToilet(Visitor visitor)
    {
        if (isBusy || isDirty) yield break;

        isBusy = true;
        visitor.GoToToilet(transform);
        yield return new WaitUntil(() => visitor.GetComponent<NavMeshAgent>().remainingDistance < 0.1f);

        yield return new WaitForSeconds(useTime);

        isBusy = false;
        isDirty = true;
        dirtyEffect?.Play();

        visitor.LeaveRestaurant();
    }

    public void Clean()
    {
        isDirty = false;
        dirtyEffect?.Stop();
    }
}
