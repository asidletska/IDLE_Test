using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    [SerializeField] private Waiter waiterPrefab;

    private Dictionary<Transform, bool> tableBusy = new Dictionary<Transform, bool>();
    private Queue<Visitor> queue = new Queue<Visitor>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (var table in tables)
            {
                tableBusy.Add(table, false);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetExitPoint() => exitPoint;

    public void EnqueueVisitor(Visitor visitor)
    {
        queue.Enqueue(visitor);
        visitor.GoToQueue(queuePoint.position + Vector3.right * queue.Count * 1.5f);
    }

    public void TryServeNextVisitor()
    {
        if (queue.Count == 0) return;

        Visitor v = queue.Dequeue();
        Transform table = GetFreeTable();
        if (table != null)
        {
            Waiter waiter = Instantiate(waiterPrefab);
            StartCoroutine(waiter.ServeVisitor(v, table, chef));
        }
        else
        {
            EnqueueVisitor(v);
        }
    }

    private Transform GetFreeTable()
    {
        foreach (var table in tables)
        {
            if (!tableBusy[table])
            {
                tableBusy[table] = true;
                return table;
            }
        }
        return null;
    }

    public void FreeTable(Transform table)
    {
        if (table != null && tableBusy.ContainsKey(table))
        {
            tableBusy[table] = false;
        }
    }
 public void AddNewTable()
    {
        if (hiddenTables.Count > 0)
        {
            Transform newTable = hiddenTables.First();
            newTable.gameObject.SetActive(true);

            tables.Add(newTable);
            hiddenTables.Remove(newTable);
            
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
            
            Debug.Log($"New stove added! Total stoves: {stoves.Count}");
        }
        else
        {
            Debug.Log("No more hidden stoves to activate.");
        }
    }
    public void AttemptUpgrade(Upgradeable item)
    {
        if (item.Upgrade())
        {
            Debug.Log($"");
        }
        else
        {
            Debug.Log("Not ");
        }

    }

}
