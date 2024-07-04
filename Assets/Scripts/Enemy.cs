
using UnityEditor;
using UnityEngine;

public class Enemy : Actor
{
    private Player m_Player;
    private EnemyStats m_enemyStats;
    private float m_curDamage;
    private float m_xpBonus;

    public float CurDamage { get => m_curDamage; private set => m_curDamage = value; }

    public override void Init()
    {
        m_Player = GameManager.Ins.Player;
        if (statsData == null || m_Player == null) return;
        m_enemyStats = (EnemyStats)statsData;
        m_enemyStats.Load();
        StatsCalculate();
        onDead.AddListener(() => onSpawnCollectables(transform.position));
        onDead.AddListener(() => onAddXpToPlayer(m_xpBonus));
    }

    private void StatsCalculate()
    {
        var PlayerStats = m_Player.PlayerStats;
        if (PlayerStats == null) return;
        float hpUpgrade = m_enemyStats.hp * Helper.GetUpgradeFormula(PlayerStats.level + 1);
        float damageUpgrade = m_enemyStats.damageUp * Helper.GetUpgradeFormula(PlayerStats.level + 1);
        float randomXPBonus = Random.Range(m_enemyStats.minXPBonus, m_enemyStats.maxXPBonus);
        CurHP = m_enemyStats.hp + hpUpgrade;
        CurDamage = m_enemyStats.damage + damageUpgrade;
        m_xpBonus = randomXPBonus * Helper.GetUpgradeFormula(PlayerStats.level + 1);
    }

    private void onSpawnCollectables(Vector3 spawnPos)
    {

    }

    private void onAddXpToPlayer(float xp)
    {

    }
}
