using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillsManager : Singleton<SkillsManager>
{
    [SerializeField] private SkillsController[] m_skillControllers;

    private Dictionary<SkillType, int> m_skillCollecteds;

    public Dictionary<SkillType, int> SkillCollecteds { get => m_skillCollecteds; }
    public SkillsController[] SkillControllers { get => m_skillControllers; }

    public override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        m_skillCollecteds = new Dictionary<SkillType, int>();
        if (SkillControllers == null || SkillControllers.Length <= 0) return;

        for (int i = 0; i < SkillControllers.Length; i++)
        {
            var skillController = SkillControllers[i];
            if (skillController == null) continue;
            skillController.LoadStats();
            skillController.OnStopWithType.AddListener(RemoveSkill);
            m_skillCollecteds.Add(skillController.skillType, 0);
        }
    }

    public SkillsController GetSkillController(SkillType type)
    {
        var findeds = SkillControllers.Where(s => s.skillType == type).ToArray();
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
        if (SkillControllers == null || SkillControllers.Length <= 0) return;
        foreach (var skillController in SkillControllers)
        {
            if (skillController == null) continue;
            skillController.ForceStop();
        }
    }
}
