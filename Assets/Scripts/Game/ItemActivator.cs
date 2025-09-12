using UnityEngine;

public class ItemActivator : MonoBehaviour
{
    public enum ItemType { Table, Toilet, Fridge, Stove }
    public ItemType itemType;

    private void OnEnable()
    {
        if (RestaurantManager.Instance != null)
        {
            switch (itemType)
            {
                case ItemType.Table:
                    RestaurantManager.Instance.AddNewTable();
                    break;
                case ItemType.Toilet:
                    RestaurantManager.Instance.AddNewToilet();
                    break;
                case ItemType.Fridge:
                    RestaurantManager.Instance.AddNewFridge();
                    break;
                case ItemType.Stove:
                    RestaurantManager.Instance.AddNewStove();
                    break;
            }
        }
    }
}

