using TMPro;
using UnityEngine;

public class BuyItems : MonoBehaviour
{
    [Header("Item to Activate")]
    [SerializeField] private GameObject itemToActivate;

    [Header("Cost")]
    [SerializeField] private int itemCost;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI costText;

    private void Start()
    {
        if (itemToActivate != null && itemToActivate.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }

        // Оновлюємо текст на кнопці, щоб показати вартість
        if (costText != null)
        {
            costText.text = $"Buy for {itemCost}$";
        }
    }

    public void BuyItem()
    {
        if (GameManager.Instance.TrySpendMoney(itemCost))
        {
            itemToActivate.SetActive(true);
            gameObject.SetActive(false);

            Debug.Log($"Item purchased for {itemCost}$!");
        }
    }
}
