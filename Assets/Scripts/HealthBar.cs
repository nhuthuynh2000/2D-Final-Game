using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public void UpdateHealthBar(float curHP, float maxHP)
    {
        transform.Find("Bar").transform.localScale = new Vector3(curHP / maxHP, 0.1f, 0);
    }
}
