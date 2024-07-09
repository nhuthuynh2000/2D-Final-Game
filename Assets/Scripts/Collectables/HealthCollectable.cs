using UnityEngine;

public class HealthCollectable : Collectable
{
    public override void Trigger()
    {
        if (m_player == null) return;
        m_player.CurHP += m_bonus;
        m_player.CurHP = Mathf.Clamp(m_player.CurHP, 0, m_player.PlayerStats.hp);

        GUIManager.Ins.UpdateHPInfo(m_player.CurHP, m_player.PlayerStats.hp);
        AudioController.Ins.PlaySound(AudioController.Ins.healthPickup);
    }
}
