using UnityEngine;

public class WorkerUpgrade : Upgradeable
{
    public float baseSpeed = 1f;
    public float speed;

    protected override void ApplyUpgrade()
    {
        speed = baseSpeed + (level - 1) * 0.2f; // ����� ����� +20% ��������
        Debug.Log("Worker upgraded! Speed = " + speed);
    }

}
