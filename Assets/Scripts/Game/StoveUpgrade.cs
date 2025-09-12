using UnityEngine;

public class StoveUpgrade : Upgradeable
{
    public float cookSpeed = 1f; 

    protected override void ApplyUpgrade()
    {
        cookSpeed = 1f + (level - 1) * 0.25f;
        Debug.Log("Stove upgraded! Cook speed = " + cookSpeed);
    }
}
