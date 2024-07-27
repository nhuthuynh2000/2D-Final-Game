using System.Collections.Generic;
using UnityEngine;

public class SkillButtonsDrawers : MonoBehaviour
{
    [SerializeField] private Transform m_gridRoot;
    [SerializeField] private SkillButtons m_skillbuttonPrefab;

    private Dictionary<SkillType, int> m_skillCollecteds;
    private int count = 0;
    public void DrawSkillButton()
    {
        Helper.ClearChilds(m_gridRoot);
        m_skillCollecteds = SkillsManager.Ins.SkillCollecteds;
        if (m_skillCollecteds == null || m_skillCollecteds.Count <= 0) return;
        foreach (var skillCollected in m_skillCollecteds)
        {
            var skillButtonClone = Instantiate(m_skillbuttonPrefab);
            Helper.AssignToRoot(m_gridRoot, skillButtonClone.transform, Vector3.zero, Vector3.one);
            if (count == 0)
            {
                skillButtonClone.skillButtonSpirte = ButtonsTrigger.Ins.m_qSkillImage;
                count++;
            }
            else if (count == 1)
            {
                skillButtonClone.skillButtonSpirte = ButtonsTrigger.Ins.m_eSkillImage;
                count++;
            }
            else if (count == 2)
            {
                skillButtonClone.skillButtonSpirte = ButtonsTrigger.Ins.m_rSkillImage;
                count++;
            }
            else
            {
                skillButtonClone.skillButtonSpirte = ButtonsTrigger.Ins.m_fSkillImage;
            }
            skillButtonClone.Initialize(skillCollected.Key);
            ButtonsTrigger.Ins.AddToSkillButtonsArray(skillButtonClone);
        }
    }
}
