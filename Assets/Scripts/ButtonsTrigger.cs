using UnityEngine;

public class ButtonsTrigger : MonoBehaviour
{
    private SkillButtons m_qSkill;
    private SkillButtons m_eSkill;
    private SkillButtons m_rSkill;
    private SkillButtons m_fSkill;
    private SkillButtonsDrawers m_ButtonsDrawers;

    private void Start()
    {
        
    }

    public void LoadInput()
    {
        m_ButtonsDrawers = GameManager.Ins.SkillButtonsDrawer;
        m_qSkill = SkillsManager.Ins.SkillCollecteds[SkillType.FireRing,0];
        m_eSkill = m_ButtonsDrawers.Skillbuttons[1];
        m_rSkill = m_ButtonsDrawers.Skillbuttons[2];
        m_fSkill = m_ButtonsDrawers.Skillbuttons[3];
    }
    void Update()
    {
        Debug.Log(m_ButtonsDrawers);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_qSkill.TriggerSkill();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_eSkill.TriggerSkill();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_rSkill.TriggerSkill();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_fSkill.TriggerSkill();
        }
    }
}
