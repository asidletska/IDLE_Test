using System.Collections;
using UnityEngine;

public class Visitor : MonoBehaviour
{
    public enum State { InQueue, GoingToToilet, UsingToilet, Returning }

    private NPCSPawner spawner;
    private Transform toiletPoint;
    private float toiletUseTime;
    private float moveSpeed = 2f;
    public bool IsEating;

    public State CurrentState { get; private set; } = State.InQueue;

    public void Setup(NPCSPawner spawner, Transform toiletPoint, float toiletUseTime)
    {
        this.spawner = spawner;
        this.toiletPoint = toiletPoint;
        this.toiletUseTime = toiletUseTime;

        // Шанс піти в туалет
        if (Random.value < 0.3f) // 30%
        {
            StartCoroutine(GoToToiletRoutine());
        }
    }

    public void MoveTo(Vector3 targetPos)
    {
        if (CurrentState == State.InQueue)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }

    private IEnumerator GoToToiletRoutine()
    {
        CurrentState = State.GoingToToilet;

        // Чекаємо, поки туалет звільниться
        while (spawner.IsToiletBusy())
        {
            yield return null;
        }

        spawner.SetToiletBusy(true);

        // Рухаємось до туалету
        while (Vector3.Distance(transform.position, toiletPoint.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, toiletPoint.position, Time.deltaTime * moveSpeed);
            yield return null;
        }

        CurrentState = State.UsingToilet;

        // Використовує туалет
        yield return new WaitForSeconds(toiletUseTime);

        spawner.SetToiletBusy(false);

        CurrentState = State.Returning;

        // Повертаємось у кінець черги
        spawner.EnqueueVisitor(this);
        CurrentState = State.InQueue;
    }

    public void SitAtTable(Transform table)
    {
        transform.position = table.position;
        CurrentState = State.UsingToilet; // тут можна зробити State.AtTable
    }

    public void StartEating()
    {
        StartCoroutine(EatingRoutine());
    }

    private IEnumerator EatingRoutine()
    {
        IsEating = true;
        yield return new WaitForSeconds(Random.Range(4f, 7f)); // час їжі
        IsEating = false;
    }

    public void LeaveTable()
    {
       
    }
}
