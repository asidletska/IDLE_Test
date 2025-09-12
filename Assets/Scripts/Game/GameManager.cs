using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money = 100;
    public int star = 0;
    [SerializeField] private TextMeshProUGUI currentMoney;
    [SerializeField] private TextMeshProUGUI currentStar;

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Path.Combine(Application.persistentDataPath, "player_data.json");
            LoadData(); 
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateUI();
    }
    public bool TrySpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateUI(); 
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI(); 
    }

    public void AddStar(int amount)
    {
        star += amount;
        UpdateUI(); 
    }

    private void UpdateUI()
    {
        if (currentMoney != null)
        {
            currentMoney.text = money.ToString();
        }

        if (currentStar != null)
        {
            currentStar.text = star.ToString();
        }
    }
    public void SaveData()
    {
        StatisticData data = new StatisticData();
        data.money = money;
        data.star = star;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
        Debug.Log("Game data saved!");
    }

    public void LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            StatisticData data = JsonUtility.FromJson<StatisticData>(json);

            money = data.money;
            star = data.star;

            UpdateUI();
            Debug.Log("Game data loaded!");
        }
        else
        {
            Debug.Log("Save file not found. Starting with default values.");
        }
    }

    private void OnApplicationQuit()
    {
        SaveData(); 
    }
}
