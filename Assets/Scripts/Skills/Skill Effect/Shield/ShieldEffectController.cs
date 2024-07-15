using UnityEngine;

public class ShieldEffectController : MonoBehaviour
{
    private Player m_player;
    private Shield m_shield;
    private ShiledSkillSO m_shieldStats;
    private float m_curShieldValue;

    private void OnEnable()
    {
        m_player = GameManager.Ins.Player;
        m_shield = (Shield)SkillsManager.Ins.GetSkillController(SkillType.Shield);
        m_shieldStats = (ShiledSkillSO)m_shield.skillStats;
        m_curShieldValue = m_shieldStats.shieldValue;
        m_curShieldValue += m_shieldStats.shieldValueUp * Helper.GetUpgradeFormula(m_player.PlayerStats.level);
        Destroy(gameObject, m_shieldStats.timeTrigger);
        Invoke("ResetMoveSpeed", m_shieldStats.timeTrigger);
    }

    private void ResetMoveSpeed()
    {
        m_player.CurSpeed = m_player.statsData.moveSpeed;
    }

    public void Update()
    {
        transform.position = m_player.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                m_curShieldValue -= enemy.CurDamage;
                Debug.Log($"Shield value remaining: {m_curShieldValue}");
                if (m_curShieldValue <= 0)
                {
                    Destroy(gameObject);
                    m_player.CurSpeed = m_player.statsData.moveSpeed;
                    m_shield.ForceStop();
                }
            }
        }
    }
}