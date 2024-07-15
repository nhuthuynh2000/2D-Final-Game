using UnityEngine;

public class Thunderbolt : SkillsController
{
    private ThunderboltSkillSO m_curStats;

    private void Awake()
    {
        m_curStats = (ThunderboltSkillSO)skillStats;
    }
    private void OnEnable()
    {
        OnTriggerEnter.AddListener(TriggerEnter);
        OnSkillUpdate.AddListener(SkillUpdate);
    }

    private void OnDisable()
    {
        OnTriggerEnter.RemoveListener(TriggerEnter);
        OnSkillUpdate.RemoveListener(SkillUpdate);
    }
    public void TriggerEnter()
    {
        Debug.Log("Fire Ring was trigger, damage is" + m_curStats.firstTargetDamage);
    }

    public void SkillUpdate()
    {
        Debug.Log("Fire Ring was updating");
    }
}
