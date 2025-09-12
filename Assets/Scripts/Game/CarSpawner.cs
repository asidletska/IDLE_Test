using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Cars")]
    [SerializeField] private List<GameObject> carPrefabs; 
    [SerializeField] private float spawnInterval = 2f;    
    [SerializeField] private int maxCars = 30;            

    [Header("Road Lanes")]
    [SerializeField] private List<Transform> spawnPoints; 
    [SerializeField] private List<Transform> endPoints;   

    private int currentCars = 0;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (currentCars < maxCars)
            {
                SpawnCar();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnCar()
    {
        if (carPrefabs.Count == 0 || spawnPoints.Count == 0 || endPoints.Count == 0) return;

        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];

        int laneIndex = Random.Range(0, spawnPoints.Count);

        GameObject car = Instantiate(prefab, spawnPoints[laneIndex].position, spawnPoints[laneIndex].rotation);

        StartCoroutine(MoveCar(car, endPoints[laneIndex]));
        currentCars++;
    }

    private IEnumerator MoveCar(GameObject car, Transform endPoint)
    {
        float speed = Random.Range(3f, 6f); 
        while (car != null && Vector3.Distance(car.transform.position, endPoint.position) > 0.1f)
        {
            car.transform.position = Vector3.MoveTowards(car.transform.position, endPoint.position, speed * Time.deltaTime);
            yield return null;
        }

        if (car != null)
        {
            Destroy(car);
            currentCars--;
        }
    }
}
