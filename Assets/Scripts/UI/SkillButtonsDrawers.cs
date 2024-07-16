using System.Collections.Generic;
using UnityEngine;

public class SkillButtonsDrawers : MonoBehaviour
{
    [SerializeField] private Transform m_gridRoot;
    [SerializeField] private SkillButtons m_skillbuttonPrefab;

    private Dictionary<SkillType, int> m_skillCollecteds;
    private List<SkillButtons> m_skillbuttons;

    public List<SkillButtons> Skillbuttons { get => m_skillbuttons; }

    public void DrawSkillButton()
    {
        Helper.ClearChilds(m_gridRoot);
        m_skillCollecteds = SkillsManager.Ins.SkillCollecteds;
        if (m_skillCollecteds == null || m_skillCollecteds.Count <= 0) return;
        foreach (var skillCollected in m_skillCollecteds)
        {
            var skillButtonClone = Instantiate(m_skillbuttonPrefab);
            Helper.AssignToRoot(m_gridRoot, skillButtonClone.transform, Vector3.zero, Vector3.one);
            //Skillbuttons.Add(skillButtonClone);
            skillButtonClone.Initialize(skillCollected.Key);
        }
    }
}
