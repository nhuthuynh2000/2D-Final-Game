using UnityEngine;
[CreateAssetMenu(fileName = "Skill Stats", menuName = "Huynh DEV/ Skill/ Create Shield Stats")]

public class ShiledSkillSO : SkillSO
{
    public float shieldValue;
    public float moveSpeedDown;

    [Header("Level UP")]
    public float shieldValueUp;
}
