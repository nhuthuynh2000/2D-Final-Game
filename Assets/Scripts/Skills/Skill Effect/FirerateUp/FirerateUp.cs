using UnityEngine;

public class FirerateUp : SkillsController
{
    private FirerateUpSkillSO m_curStats;
    private Weapon m_weapon;
    private Player m_player;

    private float m_onSkillFirerate;
    private float m_onSkillReload;
    private float m_onSkillTriggerTime;

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
        OnTriggerEnter.RemoveListener(TriggerEnter);
        OnSkillUpdate.RemoveListener(SkillUpdate);
        m_weapon.statsData.fireRate = m_onSkillFirerate;
        m_weapon.statsData.reloadTime = m_onSkillReload;
    }
    public void TriggerEnter()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_onSkillFirerate = m_weapon.statsData.fireRate;
        m_onSkillReload = m_weapon.statsData.reloadTime;
        m_weapon.statsData.fireRate -= m_curStats.firerateUp;
        m_weapon.statsData.fireRate = Mathf.Clamp(m_weapon.statsData.fireRate, 0.1f, m_weapon.statsData.fireRate);
        m_weapon.statsData.reloadTime -= m_curStats.reloadSpeedDown;
        m_weapon.statsData.reloadTime = Mathf.Clamp(m_weapon.statsData.reloadTime, 0.1f, m_weapon.statsData.reloadTime);
        m_onSkillTriggerTime = m_curStats.timeTrigger;
        AudioController.Ins.PlaySound(m_curStats.triggerSoundFX);
    }

    public void SkillUpdate()
    {
        m_onSkillTriggerTime -= Time.deltaTime;
        if (m_onSkillTriggerTime > 0) return;
        m_weapon.statsData.fireRate = m_onSkillFirerate;
        m_weapon.statsData.reloadTime = m_onSkillReload;
    }
}
