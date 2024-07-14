using UnityEngine;


[CreateAssetMenu(fileName = "Skill Stats", menuName = "Huynh DEV/ Create Skill Stats")]
public class SkillSO : ScriptableObject
{
    public float timeTrigger;
    public float coolDownTime;
    public Sprite skillIcon;
    public AudioClip triggerSoundFX;
}
