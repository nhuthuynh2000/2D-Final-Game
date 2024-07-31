using UnityEngine;

public class Shield : SkillsController
{
    [SerializeField] private GameObject m_shieldEffectPrefab;
    private ShiledSkillSO m_curStats;
    private ShieldEffectController shieldEffectController;

    private GameObject m_shieldClone;

    private Player m_player;

    public ShiledSkillSO CurStats { get => m_curStats; private set => m_curStats = value; }

    private void Start()
    {
        CurStats = (ShiledSkillSO)skillStats;
        m_player = GameManager.Ins.Player;
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
        if (m_player == null || m_shieldEffectPrefab == null)
        {
            return;
        }
        m_shieldClone = Instantiate(m_shieldEffectPrefab, m_player.transform.position, Quaternion.identity);
        Destroy(shieldEffectController, CurStats.timeTrigger);
        shieldEffectController = m_shieldClone.AddComponent<ShieldEffectController>();
        m_player.CurSpeed -= CurStats.moveSpeedDown;
        AudioController.Ins.PlaySound(m_curStats.triggerSoundFX);
    }

    public void SkillUpdate()
    {
        if (shieldEffectController == null || m_player == null) return;
        shieldEffectController.Update();
    }
}
