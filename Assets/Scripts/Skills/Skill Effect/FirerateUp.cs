using UnityEngine;

public class FirerateUp : SkillsController
{
    private FirerateUpSkillSO m_curStats;
    private Weapon m_weapon;
    private Player m_player;

    private float m_onSkillFirerate;
    private float m_onSkillReload;

    private void Awake()
    {
        m_curStats = (FirerateUpSkillSO)skillStats;
    }
    private void OnEnable()
    {
        OnTriggerEnter.AddListener(TriggerEnter);
        OnSkillUpdate.AddListener(SkillUpdate);
    }

    private void OnDisable()
    {
        m_weapon.ResetFireRate(m_onSkillFirerate);
        m_weapon.ResetReloadTime(m_onSkillReload);
        OnTriggerEnter.RemoveListener(TriggerEnter);
        OnSkillUpdate.RemoveListener(SkillUpdate);
    }
    public void TriggerEnter()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_onSkillFirerate = m_weapon.statsData.fireRate;
        m_onSkillReload = m_weapon.statsData.reloadTime;
        m_weapon.statsData.fireRate -= m_curStats.firerateUp;
        m_weapon.statsData.reloadTime -= m_curStats.reloadSpeedDown;
    }

    public void SkillUpdate()
    {

    }
}
