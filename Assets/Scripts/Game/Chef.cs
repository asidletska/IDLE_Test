using System.Collections;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [SerializeField] private Transform cookingPoint;
    [SerializeField] private float cookingTime = 3f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public Transform GetCookingPoint() => cookingPoint;

    public IEnumerator CookMeal(System.Action onMealReady)
    {
        animator?.SetBool("idle", true);
        yield return new WaitForSeconds(cookingTime);
        onMealReady?.Invoke();
    }

    public void UpgradeChef(float timeReduction)
    {
        cookingTime = Mathf.Max(1f, cookingTime - timeReduction);
    }
}
