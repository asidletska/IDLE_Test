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

        // веде відвідувача до столика
        yield return MoveTo(visitor.transform, table.position);
        visitor.SitAtTable(table);

        // йде на кухню за стравою
        yield return MoveTo(transform, chef.transform.position);

        // чекає поки кухар приготує
        bool mealReady = false;
        yield return chef.StartCoroutine(chef.CookMeal(() => mealReady = true));
        while (!mealReady) yield return null;

        // несе їжу відвідувачу
        yield return MoveTo(transform, table.position);
        visitor.StartEating();

        // чекає поки відвідувач поїсть
        while (visitor.IsEating) yield return null;

        // прибирає і звільняє стіл
        visitor.LeaveTable();
        Debug.Log("🧹 Стіл звільнено.");

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
