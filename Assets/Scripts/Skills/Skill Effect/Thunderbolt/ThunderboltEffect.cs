using UnityEngine;

public class ThunderboltEffect : MonoBehaviour
{
    private Weapon m_weapon;
    private Player m_player;
    private Thunderbolt m_thunderbolt;
    private ThunderboltSkillSO m_thunderStats;
    private float m_moveSpeed;
    private float m_firstDamage;
    private float m_previousDamage;

    private Vector2 m_lastPosition;
    private RaycastHit2D m_raycastHit;

    private void OnEnable()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_thunderbolt = (Thunderbolt)SkillsManager.Ins.GetSkillController(SkillType.ThunderBolt);
        m_thunderStats = m_thunderbolt.CurStats;
        m_moveSpeed = m_thunderStats.moveSpeed;
        m_firstDamage = m_thunderStats.firstTargetDamage + m_weapon.statsData.damage;
        m_previousDamage = m_thunderStats.previousTargetDamage + m_weapon.statsData.damage;
        RefreshLastPos();
    }

    private void Update()
    {
        transform.Translate(transform.right * m_moveSpeed * Time.deltaTime, Space.World);
        DealDamamge();
        RefreshLastPos();
    }

    private void DealDamamge()
    {
        Vector2 rayDirection = (Vector2)transform.position - m_lastPosition;
        m_raycastHit = Physics2D.Raycast(m_lastPosition, rayDirection, rayDirection.magnitude);
        var collider = m_raycastHit.collider;
        if (!m_raycastHit || !m_raycastHit.collider) return;
        if (collider.CompareTag(TagConsts.ENEMY_TAG))
        {
            DealDamageToEnemy(collider);
        }
    }

    private void DealDamageToEnemy(Collider2D collider)
    {
        Actor actorComp = collider.GetComponent<Actor>();
        actorComp?.TakeDamage(m_firstDamage);
        /*if (m_bodyHitPrefab)
        {
            Instantiate(m_bodyHitPrefab, (Vector3)m_raycastHit.point, Quaternion.identity);
        }*/

        Destroy(gameObject);
    }

    private void DealDamageToNextEnemy()
    {

    }

    private void RefreshLastPos()
    {
        m_lastPosition = (Vector2)transform.position;
    }

    private void OnDisable()
    {
        m_raycastHit = new RaycastHit2D();
    }
}
