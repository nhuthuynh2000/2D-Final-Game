using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Huynh DEV/ Create Enemy Stats")]
public class EnemyStats : ActorStats
{
    [Header("XP Bonus: ")]
    public float minXPBonus;
    public float maxXPBonus;

    [Header("Level Up: ")]
    public float hpUp;
    public float damageUp;
}
