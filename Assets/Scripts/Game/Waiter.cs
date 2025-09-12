using System.Collections;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private bool isBusy = false;

    public IEnumerator ServeVisitor(Visitor visitor, Transform table, Chef chef)
    {
        if (isBusy) yield break;
        isBusy = true;

        yield return MoveTo(visitor.transform, table.position);
        visitor.SitAtTable(table);

        yield return MoveTo(transform, chef.transform.position);

        bool mealReady = false;
        yield return chef.StartCoroutine(chef.CookMeal(() => mealReady = true));
        while (!mealReady) yield return null;

        yield return MoveTo(transform, table.position);
        visitor.StartEating();

        while (visitor.IsEating) yield return null;

        visitor.LeaveTable();
        isBusy = false;
    }

    private IEnumerator MoveTo(Transform obj, Vector3 targetPos)
    {
        while (Vector3.Distance(obj.position, targetPos) > 0.05f)
        {
            obj.position = Vector3.MoveTowards(obj.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
