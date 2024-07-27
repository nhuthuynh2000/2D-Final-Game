using System.Collections.Generic;
using UnityEngine;


public class ButtonsTrigger : Singleton<ButtonsTrigger>
{

    public override void Awake()
    {
        MakeSingleton(false);
    }
    public List<SkillButtons> SkillButtons;
    private SkillButtons m_qSkill { get => SkillButtons[0]; }
    private SkillButtons m_eSkill { get => SkillButtons[1]; }
    private SkillButtons m_rSkill { get => SkillButtons[2]; }
    private SkillButtons m_fSkill { get => SkillButtons[3]; }
    public Sprite m_qSkillImage;
    public Sprite m_eSkillImage;
    public Sprite m_rSkillImage;
    public Sprite m_fSkillImage;
    public void AddToSkillButtonsArray(SkillButtons SkillButton)
    {
        SkillButtons.Add(SkillButton);
    }
    public void TriggerQSkill()
    {
        m_qSkill.TriggerSkill();
    }
    public void TriggerESkill()
    {
        m_eSkill.TriggerSkill();
    }
    public void TriggerRSkill()
    {
        m_rSkill.TriggerSkill();
    }
    public void TriggerFSkill()
    {
        m_fSkill.TriggerSkill();
    }
}
