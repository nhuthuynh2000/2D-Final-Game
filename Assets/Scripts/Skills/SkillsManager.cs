using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillsManager : Singleton<SkillsManager>
{
    [SerializeField] private SkillsController[] m_skillControllers;

    private Dictionary<SkillType, int> m_skillCollecteds;

    public Dictionary<SkillType, int> SkillCollecteds { get => m_skillCollecteds; }

    public override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        m_skillCollecteds = new Dictionary<SkillType, int>();
        if (m_skillControllers == null || m_skillControllers.Length <= 0) return;

        for (int i = 0; i < m_skillControllers.Length; i++)
        {
            var skillController = m_skillControllers[i];
            if (skillController == null) continue;
            skillController.LoadStats();
            skillController.OnStopWithType.AddListener(RemoveSkill);
            m_skillCollecteds.Add(skillController.skillType, 0);
        }
    }

    public SkillsController GetSkillController(SkillType type)
    {
        var findeds = m_skillControllers.Where(s => s.skillType == type).ToArray();
        if (findeds == null || findeds.Length <= 0) return null;
        return findeds[0];
    }

    public int GetSkillAmount(SkillType type)
    {
        if (!IsSkillExist(type)) return 0;
        return m_skillCollecteds[type];
    }

    public void AddSkill(SkillType type, int amount = 1)
    {
        if (IsSkillExist(type))
        {
            var currentAmount = m_skillCollecteds[type];
            currentAmount += amount;
            m_skillCollecteds[type] = currentAmount;
        }
        else
        {
            m_skillCollecteds.Add(type, amount);
        }
    }

    public void RemoveSkill(SkillType type, int amount = 1)
    {
        if (!IsSkillExist(type)) return;
        var currentAmount = m_skillCollecteds[type];
        currentAmount -= amount;
        m_skillCollecteds[type] = currentAmount;
        if (currentAmount > 0) return;
        m_skillCollecteds.Remove(type);
    }

    public bool IsSkillExist(SkillType type)
    {
        return m_skillCollecteds.ContainsKey(type);
    }

    public void StopSkill(SkillType type)
    {
        var skillController = GetSkillController(type);
        if (skillController == null) return;
        skillController.Stop();
    }

    public void StopAllSkill()
    {
        if (m_skillControllers == null || m_skillControllers.Length <= 0) return;
        foreach (var skillController in m_skillControllers)
        {
            if (skillController == null) continue;
            skillController.ForceStop();
        }
    }
}
