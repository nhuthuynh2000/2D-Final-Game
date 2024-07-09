using UnityEngine;

public class Boss : Actor
{
    private Player m_Player;
    private BossStats m_bossStats;
    private float m_curDamage;

    public float CurDamage { get => m_curDamage; private set => m_curDamage = value; }

    public override void Init()
    {
        m_Player = GameManager.Ins.Player;
        if (statsData == null || m_Player == null) return;
        m_bossStats = (BossStats)statsData;
        m_bossStats.Load();
        StatsCalculate();
    }

    private void StatsCalculate()
    {
        var PlayerStats = m_Player.PlayerStats;
        if (PlayerStats == null) return;
        float hpUpgrade = (m_bossStats.hpUp + m_bossStats.bossHpUp) * Helper.GetUpgradeFormula(PlayerStats.level + 1);
        float damageUpgrade = (m_bossStats.damageUp + m_bossStats.bossDamageUp) * Helper.GetUpgradeFormula(PlayerStats.level + 1);
        CurHP = m_bossStats.hp + hpUpgrade;
        CurDamage = m_bossStats.damage + damageUpgrade;
    }

    protected override void Die()
    {
        base.Die();
        m_anim.SetTrigger(AnimConsts.ENEMY_DEAD_PARAM);
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
            m_rb.velocity = playerDir * m_bossStats.moveSpeed * Time.deltaTime;
            return;
        }
        m_rb.velocity = playerDir * -m_bossStats.knockbackForce * Time.deltaTime;
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

    public override void TakeDamage(float damage)
    {
        if (damage < 0 || m_isInvincible) return;
        CurHP -= m_Player.weapon.statsData.damage;
        Knockback();
        if (CurHP <= 0)
        {
            CurHP = 0;
            Die();
        }
        onTakeDamage?.Invoke();
    }

    private void OnDisable()
    {
        //Unlock phan thuong
    }
}
