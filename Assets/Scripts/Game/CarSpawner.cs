using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Cars")]
    [SerializeField] private List<GameObject> carPrefabs; // Префаби машин
    [SerializeField] private float spawnInterval = 2f;    // Інтервал спавну
    [SerializeField] private int maxCars = 30;            // Ліміт

    [Header("Road Lanes")]
    [SerializeField] private List<Transform> spawnPoints; // Початки доріг
    [SerializeField] private List<Transform> endPoints;   // Кінці доріг

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

        // Рандомна машина
        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];

        // Вибираємо полосу (індекс)
        int laneIndex = Random.Range(0, spawnPoints.Count);

        // Створюємо машину
        GameObject car = Instantiate(prefab, spawnPoints[laneIndex].position, spawnPoints[laneIndex].rotation);

        // Запускаємо рух
        StartCoroutine(MoveCar(car, endPoints[laneIndex]));
        currentCars++;
    }

    private IEnumerator MoveCar(GameObject car, Transform endPoint)
    {
        float speed = Random.Range(3f, 6f); // Швидкість різна
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
