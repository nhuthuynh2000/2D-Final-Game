using UnityEngine;

public class PlayerVisual : ActorVisual
{
    [SerializeField] private GameObject m_deathVFXPrefab;

    private Player m_player;
    private PlayerStats m_playerStats;
    private void Start()
    {
        m_player = (Player)m_actor;
        m_playerStats = m_player.PlayerStats;
    }
    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        GUIManager.Ins.UpdateHPInfo(m_actor.CurHP, m_actor.statsData.hp);
    }
    public void OnLostLife()
    {
        if (m_player == null || m_playerStats == null) return;
        AudioController.Ins.PlaySound(AudioController.Ins.lostLife);
        GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);
        GUIManager.Ins.UpdateHPInfo(m_player.CurHP, m_playerStats.hp);
    }
    public void OnDead()
    {
        if (m_deathVFXPrefab)
        {
            Instantiate(m_deathVFXPrefab, transform.position, Quaternion.identity);
        }
        AudioController.Ins.PlaySound(AudioController.Ins.playerDeath);
        GUIManager.Ins.ShowGameOverDialog();
    }
    public void OnAddXP()
    {
        if (m_playerStats == null) return;
        GUIManager.Ins.UpdateLevelInfo(m_playerStats.level, m_playerStats.xp, m_playerStats.levelUpXPNeed);
    }
    public void OnLevelUp()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.levelUp);
    }
}
