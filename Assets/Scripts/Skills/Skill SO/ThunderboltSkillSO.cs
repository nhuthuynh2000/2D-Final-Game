using UnityEngine;
[CreateAssetMenu(fileName = "Skill Stats", menuName = "Huynh DEV/ Skill/ Create Thunderbolt Stats")]

public class ThunderboltSkillSO : SkillSO
{
    public float firstTargetDamage;
    public float previousTargetDamage;
    public float spanwRate;
    public float moveSpeed;
    public float radius;
    public int numberOfTargets;
}
