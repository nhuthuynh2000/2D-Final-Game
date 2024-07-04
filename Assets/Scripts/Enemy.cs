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
        onDead.AddListener(() => onSpawnCollectables());
        onDead.AddListener(() => onAddXpToPlayer());
    }

    private void StatsCalculate()
    {
        var PlayerStats = m_Player.PlayerStats;
        if (PlayerStats == null) return;
        float hpUpgrade = m_enemyStats.hpUp * Helper.GetUpgradeFormula(PlayerStats.level + 1);
        float damageUpgrade = m_enemyStats.damageUp * Helper.GetUpgradeFormula(PlayerStats.level + 1);
        float randomXPBonus = Random.Range(m_enemyStats.minXPBonus, m_enemyStats.maxXPBonus);
        CurHP = m_enemyStats.hp + hpUpgrade;
        CurDamage = m_enemyStats.damage + damageUpgrade;
        m_xpBonus = randomXPBonus * Helper.GetUpgradeFormula(PlayerStats.level + 1);
    }

    protected override void Die()
    {
        base.Die();
        m_anim.SetTrigger(AnimConsts.ENEMY_DEAD_PARAM);
    }

    private void onSpawnCollectables()
    {
        CollectablesManager.Ins.Spawn(transform.position);
    }

    private void onAddXpToPlayer()
    {

    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (IsDead || m_Player == null) return;
        Vector2 playerDir = m_Player.transform.position - transform.position;
        playerDir.Normalize();
        if (!m_isKnockback)
        {
            Flip(playerDir);
            m_rb.velocity = playerDir * m_enemyStats.moveSpeed * Time.deltaTime;
            return;
        }
        m_rb.velocity = playerDir * -m_enemyStats.knockbackForce * Time.deltaTime;
    }

    private void Flip(Vector2 playerDir)
    {
        if (playerDir.x > 0)
        {
            if (transform.localScale.x > 0) return;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            if (transform.localScale.x < 0) return;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDisable()
    {
        onDead.RemoveListener(onSpawnCollectables);
        onDead.RemoveListener(onAddXpToPlayer);
    }
}
