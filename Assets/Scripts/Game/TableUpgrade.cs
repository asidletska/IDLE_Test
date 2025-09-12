using UnityEngine;

public class TableUpgrade : Upgradeable
{
    public int capacity = 2;

    protected override void ApplyUpgrade()
    {
        capacity = 2 + (level - 1); // кожен апгрейд додає 1 місце
        Debug.Log("Table upgraded! Capacity = " + capacity);
    }
}
