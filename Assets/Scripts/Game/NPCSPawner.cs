using UnityEngine;

public class NPCSPawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] visitorPrefabs;
    [SerializeField] private Transform[] spawnPoints;   
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxVisitors = 10;

    private float spawnTimer;
    private int spawnedCount;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && spawnedCount < maxVisitors)
        {
            SpawnVisitor();
            spawnTimer = 0f;
        }
    }

    private void SpawnVisitor()
    {
        GameObject prefab = visitorPrefabs[Random.Range(0, visitorPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newVisitorObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Visitor visitor = newVisitorObj.GetComponent<Visitor>();

        if (visitor != null)
        {
            RestaurantManager.Instance.OnVisitorArrived(visitor);

            Transform firstQueuePoint = RestaurantManager.Instance.GetFirstQueuePoint();
            if (firstQueuePoint != null)
            {
                visitor.GoToQueue(firstQueuePoint.transform.position);
            }
        }
        spawnedCount++;
    }
    public void VisitorLeft()
    {
        spawnedCount = Mathf.Max(0, spawnedCount - 1);
    }
}
