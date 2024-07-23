using Unity.VisualScripting;
using UnityEngine;

public class FireRingEffects : MonoBehaviour
{
    private Weapon m_weapon;
    private Player m_player;
    private FireRing m_fireRing;
    private FireRingSkillSO m_fireRingStats;

    private float m_damage;
    private void OnEnable()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_fireRing = (FireRing)SkillsManager.Ins.GetSkillController(SkillType.FireRing);
        m_fireRingStats = m_fireRing.CurStats;
        m_damage = m_fireRingStats.damage + m_weapon.statsData.damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Da va cham voi Enemy");
        if (collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.CurHP -= m_damage;
        }
    }
}
