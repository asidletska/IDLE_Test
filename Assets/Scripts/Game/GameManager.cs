using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int money = 100;
    public int star = 0;
    [SerializeField] private TextMeshProUGUI currentMoney;
    [SerializeField] private TextMeshProUGUI currentStar;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TrySpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        return false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void AddStar(int amount)
    {
        star += amount;
    }
}
