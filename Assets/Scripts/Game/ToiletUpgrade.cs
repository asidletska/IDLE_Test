using UnityEngine;

public class ToiletUpgrade : Upgradeable
{
    public float satisfaction = 1f;

    protected override void ApplyUpgrade()
    {
        satisfaction = 1f + (level - 1) * 0.1f; // ����� ����� +10% �����������
        Debug.Log("Toilet upgraded! Satisfaction = " + satisfaction);
    }
}
