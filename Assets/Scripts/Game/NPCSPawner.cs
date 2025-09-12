using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class NPCSPawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private GameObject[] visitorPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;

    [Header("Queue Settings")]
    [SerializeField] private Transform queueParent;
    [SerializeField] private float queueSpacing = 0.5f;
    [SerializeField] private int maxQueueSize = 5;

    [Header("Toilet Settings")]
    [SerializeField] private Transform toiletPoint;
    [SerializeField] private float toiletUseTime = 3f;

    private Queue<Visitor> visitorQueue = new Queue<Visitor>();
    private float spawnTimer;
    private bool toiletBusy = false;

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && visitorQueue.Count < maxQueueSize)
        {
            SpawnVisitor();
            spawnTimer = 0f;
        }

        UpdateQueuePositions();
    }

    private void SpawnVisitor()
    {
        GameObject prefab = visitorPrefabs[Random.Range(0, visitorPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newVisitorObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        Visitor visitor = newVisitorObj.GetComponent<Visitor>();
        visitor.Setup(this, toiletPoint, toiletUseTime);

        EnqueueVisitor(visitor);
    }

    public void EnqueueVisitor(Visitor visitor)
    {
        if (!visitorQueue.Contains(visitor))
            visitorQueue.Enqueue(visitor);
    }

    public Visitor DequeueVisitor()
    {
        if (visitorQueue.Count == 0) return null;
        return visitorQueue.Dequeue();
    }

    public void UpdateQueuePositions()
    {
        int i = 0;
        foreach (Visitor visitor in visitorQueue)
        {
            if (visitor != null && visitor.CurrentState == Visitor.State.InQueue)
            {
                Vector3 targetPos = queueParent.position + Vector3.left * (i * queueSpacing);
                visitor.MoveTo(targetPos);
            }
            i++;
        }
    }

    public bool IsToiletBusy() => toiletBusy;
    public void SetToiletBusy(bool value) => toiletBusy = value;

    public void ExpandQueue(int extraSlots)
    {
        maxQueueSize += extraSlots;
    }
}
