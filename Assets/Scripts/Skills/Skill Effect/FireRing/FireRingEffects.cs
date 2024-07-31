using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FireRingEffects : MonoBehaviour
{
    private Weapon m_weapon;
    private Player m_player;
    private FireRing m_fireRing;
    private FireRingSkillSO m_fireRingStats;

    private bool m_canBeDamaged;

    private float m_damage;
    private void OnEnable()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_fireRing = (FireRing)SkillsManager.Ins.GetSkillController(SkillType.FireRing);
        m_fireRingStats = m_fireRing.CurStats;
        m_damage = m_fireRingStats.damage + m_weapon.statsData.damage * 10 / 100;
        m_canBeDamaged = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (m_canBeDamaged)
            {
                StartCoroutine(DealDamage(enemy));
            }
        }
    }
    private void Update()
    {
        Debug.Log(m_damage);
    }
    private IEnumerator DealDamage(Enemy enemy)
    {
        enemy.CurHP -= m_damage;
        m_canBeDamaged = false;
        yield return new WaitForSeconds(m_fireRingStats.delayTime);
        m_canBeDamaged = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
