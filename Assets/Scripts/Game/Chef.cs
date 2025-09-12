using System.Collections;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [SerializeField] private Transform cookingPoint; 
    [SerializeField] private float cookingTime = 3f; 

    private bool isCooking = false;

    public IEnumerator CookMeal(System.Action onMealReady)
    {
        if (isCooking) yield break; 

        isCooking = true;
        yield return new WaitForSeconds(cookingTime);

        isCooking = false;

        onMealReady?.Invoke();
    }
    public void UpgradeChef(float timeReduction)
    {
        cookingTime = Mathf.Max(1f, cookingTime - timeReduction);
    }
}
