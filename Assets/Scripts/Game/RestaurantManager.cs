using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance;

    [SerializeField] private Transform queuePoint;
    [SerializeField] private Transform[] tables;
    [SerializeField] private Transform[] toilets;

    private bool[] tableBusy;

    private void Awake()
    {
        Instance = this;
        tableBusy = new bool[tables.Length];
    }

    public Transform GetQueuePoint() => queuePoint;

    public Transform GetFreeTable()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            if (!tableBusy[i])
            {
                tableBusy[i] = true;
                return tables[i];
            }
        }
        return null;
    }

    public Transform GetToilet(int index = 0)
    {
        if (index >= 0 && index < toilets.Length)
            return toilets[index];
        return null;
    }
}
