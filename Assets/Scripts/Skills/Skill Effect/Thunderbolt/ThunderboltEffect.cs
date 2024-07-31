
using System.Collections.Generic;

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
    [SerializeField] private GameObject m_thunderPrefab;
    private LayerMask m_enemyLayer;

    private Vector2 m_lastPosition;
    private RaycastHit2D m_raycastHit;

    public float FirstDamage { get => m_firstDamage; set => m_firstDamage = value; }
    public float PreviousDamage { get => m_previousDamage; set => m_previousDamage = value; }

    private float m_radius;


    private void OnEnable()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_thunderbolt = (Thunderbolt)SkillsManager.Ins.GetSkillController(SkillType.ThunderBolt);
        m_thunderStats = m_thunderbolt.CurStats;
        m_moveSpeed = m_thunderStats.moveSpeed;
        m_firstDamage = m_thunderStats.firstTargetDamage + m_weapon.statsData.damage;
        m_previousDamage = m_thunderStats.previousTargetDamage + m_weapon.statsData.damage;
        m_radius = m_thunderStats.radius;
        m_enemyLayer = m_thunderbolt.EnemyDetectionLayer;
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
            collider.gameObject.layer = LayerMask.NameToLayer("Default");
            DealDamageToEnemy(collider);
            DealDamageToNextEnemy(collider);
            Destroy(gameObject);
            collider.gameObject.layer = LayerMask.NameToLayer("Enemy");
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
    }

    private void DealDamageToNextEnemy(Collider2D collider)
    {
        DetectEnemy(collider);
    }

    private void DetectEnemy(Collider2D collider)
    {
        var enemyFindeds = Physics2D.OverlapCircleAll(collider.transform.position, m_radius, m_enemyLayer);

        var nearEnemy = FindEnemy(enemyFindeds);
        for (int i = 0; i < nearEnemy.Count; i++)
        {
            var m_enemyTargeted = nearEnemy[i];
            m_enemyTargeted.gameObject.layer = LayerMask.NameToLayer("ThunderNextEnemy");
            Vector2 enemyDir = m_enemyTargeted.transform.position - collider.transform.position;
            enemyDir.Normalize();
            float angle = Mathf.Atan2(enemyDir.y, enemyDir.x) * Mathf.Rad2Deg;
            Shoot(enemyDir, angle, collider);
            m_enemyTargeted.gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
    }
    public void Shoot(Vector3 targetDirection, float angle, Collider2D collider)
    {
        if (m_thunderPrefab)
        {
            var thunderPrefabClone = Instantiate(m_thunderPrefab, collider.transform.position, Quaternion.Euler(0f, 0f, angle));
        }
    }

    private List<Actor> FindEnemy(Collider2D[] enemyFindeds)
    {
        List<Actor> nearestEnemies = new List<Actor>();
        float minDistance = float.MaxValue;

        if (enemyFindeds == null || enemyFindeds.Length <= 0)
            return nearestEnemies;

        for (int i = 0; i < enemyFindeds.Length; i++)
        {
            Collider2D enemyFinded = enemyFindeds[i];
            if (enemyFinded == null)
                continue;

            float distanceTemp = Vector2.Distance(m_player.transform.position, enemyFinded.transform.position);
            if (distanceTemp <= minDistance)
            {
                if (distanceTemp == minDistance)
                {
                    nearestEnemies.Add(enemyFinded.GetComponent<Actor>());
                }
                else
                {
                    nearestEnemies.Clear();
                    nearestEnemies.Add(enemyFinded.GetComponent<Actor>());
                    minDistance = distanceTemp;
                }
            }
        }

        return nearestEnemies;
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
