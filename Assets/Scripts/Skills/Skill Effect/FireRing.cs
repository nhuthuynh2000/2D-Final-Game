using UnityEngine;

public class FireRing : SkillsController
{
    private FireRingSkillSO m_curStats;

    private void Awake()
    {
        m_curStats = (FireRingSkillSO)skillStats;
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
        Debug.Log("Fire Ring was trigger, damage is" + m_curStats.damage);
    }

    public void SkillUpdate()
    {
        Debug.Log("Fire Ring was updating");
    }
}
