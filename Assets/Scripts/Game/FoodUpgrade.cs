using UnityEngine;

public class FoodUpgrade : Upgradeable
{
    public int foodValue = 10;

    protected override void ApplyUpgrade()
    {
        foodValue = 10 + (level - 1) * 5; // ����� ����� +5 �� ���� ������
        Debug.Log("Food upgraded! Value = " + foodValue);
    }
}
