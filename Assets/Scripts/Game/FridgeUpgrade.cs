using UnityEngine;

public class FridgeUpgrade : Upgradeable
{
    public int capacity = 50;

    protected override void ApplyUpgrade()
    {
        capacity = 50 + (level - 1) * 20;
        Debug.Log("Fridge upgraded! Capacity = " + capacity);
    }
}
