using UnityEngine;

public abstract class Upgradeable : MonoBehaviour
{
    public int level = 1;
    public int baseCost = 50;
    public float costMultiplier = 1.5f;

    public virtual int GetUpgradeCost()
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level - 1));
    }

    public virtual bool Upgrade()
    {
        int cost = GetUpgradeCost();
        if (GameManager.Instance.TrySpendMoney(cost))
        {
            level++;
            ApplyUpgrade();
            return true;
        }
        return false;
    }

    protected abstract void ApplyUpgrade();
}
