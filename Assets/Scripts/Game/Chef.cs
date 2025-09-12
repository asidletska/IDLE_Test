using System.Collections;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [SerializeField] private Transform cookingPoint;  // де "готуються" страви
    [SerializeField] private float cookingTime = 3f; // час приготування однієї страви

    private bool isCooking = false;

    public IEnumerator CookMeal(System.Action onMealReady)
    {
        if (isCooking) yield break; // вже готує

        isCooking = true;
        Debug.Log("👨‍🍳 Кухар почав готувати...");

        yield return new WaitForSeconds(cookingTime);

        Debug.Log("🍲 Їжа готова!");
        isCooking = false;

        onMealReady?.Invoke();
    }

    // апгрейд (зменшує час приготування)
    public void UpgradeChef(float timeReduction)
    {
        cookingTime = Mathf.Max(1f, cookingTime - timeReduction);
    }
}
