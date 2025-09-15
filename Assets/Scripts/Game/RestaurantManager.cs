using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RestaurantManager : MonoBehaviour
{
    public static RestaurantManager Instance;

    [Header("References")]
    [SerializeField] private Transform queuePoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private List<Transform> tables = new List<Transform>();
    [SerializeField] private List<Transform> toilets = new List<Transform>();
    [SerializeField] private List<Transform> fridges = new List<Transform>();
    [SerializeField] private List<Transform> stoves = new List<Transform>();
    [SerializeField] private List<Transform> hiddenTables = new List<Transform>();
    [SerializeField] private List<Transform> hiddenToilets = new List<Transform>();
    [SerializeField] private List<Transform> hiddenFridges = new List<Transform>();
    [SerializeField] private List<Transform> hiddenStoves = new List<Transform>();

    [SerializeField] private Chef chef;
    [SerializeField] private Waiter waiter;

    public float queueOffset = 1f;

    private List<Transform> busyTables = new List<Transform>();
    private Queue<Visitor> queue = new Queue<Visitor>();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (queue.Count > 0 && !waiter.IsBusy())
        {
            Visitor visitor = queue.Dequeue();
            Transform table = GetFreeTable();

            if (table != null)
            {
                busyTables.Add(table);
                StartCoroutine(waiter.ServeVisitor(visitor, table, chef));

                UpdateQueuePositions();
            }
            else
            {
                queue.Enqueue(visitor);
            }
        }
    }

    public void OnVisitorArrived(Visitor visitor)
    {
        queue.Enqueue(visitor);
        UpdateQueuePositions();
    }
    public Transform GetFirstQueuePoint()
    {
        return queuePoint;
    }
    private void UpdateQueuePositions()
    {
        int index = 0;
        foreach (var visitor in queue)
        {
            Vector3 targetPos = queuePoint.position + Vector3.left * queueOffset * index;
            visitor.GoToQueue(targetPos);
            index++;
        }
    }

    public void FreeTable(Transform table)
    {
        if (busyTables.Contains(table))
            busyTables.Remove(table);
    }

    private Transform GetFreeTable()
    {
        foreach (var t in tables)
        {
            if (!busyTables.Contains(t))
                return t;
        }
        return null;
    }

    public Transform GetFreeToilet()
    {
        return toilets.FirstOrDefault(); 
    }
    public Transform GetExitPoint() => exitPoint;
    public void OnVisitorLeft()
    {
        FindObjectOfType<NPCSPawner>()?.VisitorLeft();
    }

    public void AddNewTable()
    {
        if (hiddenTables.Count > 0)
        {
            Transform newTable = hiddenTables.First();
            newTable.gameObject.SetActive(true);

            tables.Add(newTable);
            hiddenTables.Remove(newTable);
            GameManager.Instance.TrySpendMoney(500);
            Debug.Log($"New table added! Total tables: {tables.Count}");
        }
        else
        {
            Debug.Log("No more hidden tables to activate.");
        }
    }

    public void AddNewToilet()
    {
        if (hiddenToilets.Count > 0)
        {
            Transform newToilet = hiddenToilets.First();
            newToilet.gameObject.SetActive(true);

            toilets.Add(newToilet);
            hiddenToilets.Remove(newToilet);
            GameManager.Instance.TrySpendMoney(500);
            Debug.Log($"New toilet added! Total toilets: {toilets.Count}");
        }
        else
        {
            Debug.Log("No more hidden toilets to activate.");
        }
    }

    public void AddNewFridge()
    {
        if (hiddenFridges.Count > 0)
        {
            Transform newFridge = hiddenFridges.First();
            newFridge.gameObject.SetActive(true);

            fridges.Add(newFridge);
            hiddenFridges.Remove(newFridge);
            GameManager.Instance.TrySpendMoney(500);
            Debug.Log($"New fridge added! Total fridges: {fridges.Count}");
        }
        else
        {
            Debug.Log("No more hidden fridges to activate.");
        }
    }

    public void AddNewStove()
    {
        if (hiddenStoves.Count > 0)
        {
            Transform newStove = hiddenStoves.First();
            newStove.gameObject.SetActive(true);

            stoves.Add(newStove);
            hiddenStoves.Remove(newStove);
            GameManager.Instance.TrySpendMoney(500);
            Debug.Log($"New stove added! Total stoves: {stoves.Count}");
        }
        else
        {
            Debug.Log("No more hidden stoves to activate.");
        }
    }
}
