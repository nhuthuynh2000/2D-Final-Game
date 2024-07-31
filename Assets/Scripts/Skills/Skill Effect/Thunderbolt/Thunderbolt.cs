using UnityEngine;

public class Thunderbolt : SkillsController
{

    [SerializeField] private GameObject m_thunderPrefab;
    private ThunderboltSkillSO m_curStats;
    private Player m_player;
    private Weapon m_weapon;
    private Transform m_spawnPos;


    [SerializeField] private LayerMask m_enemyDetectionLayer;

    private float m_speed;
    private float m_spawnRate;
    private float m_numberOfTargets;
    private Actor m_enemyTargeted;
    private float m_radius;
    private bool m_isShooted = false;

    public ThunderboltSkillSO CurStats { get => m_curStats; set => m_curStats = value; }
    public GameObject ThunderPrefab { get => m_thunderPrefab; }
    public LayerMask EnemyDetectionLayer { get => m_enemyDetectionLayer; }
    public Actor EnemyTargeted { get => m_enemyTargeted; }

    private void Awake()
    {
        CurStats = (ThunderboltSkillSO)skillStats;
    }
    private void OnEnable()
    {
        OnTriggerEnter.AddListener(TriggerEnter);
        OnSkillUpdate.AddListener(SkillUpdate);
    }

    private void OnDisable()
    {
        OnTriggerEnter.RemoveListener(TriggerEnter);
        OnSkillUpdate.RemoveListener(SkillUpdate);
    }
    public void TriggerEnter()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_spawnPos = m_weapon.ShootingPoint;
        m_speed = CurStats.moveSpeed;
        m_spawnRate = CurStats.spanwRate;
        m_numberOfTargets = CurStats.numberOfTargets;
        m_radius = CurStats.radius;
    }

    public void SkillUpdate()
    {
        DetectEnemy();
        ReduceFirerate();
    }

    private void ReduceFirerate()
    {
        if (!m_isShooted) return;
        m_spawnRate -= Time.deltaTime;

        if (m_spawnRate > 0) return;
        m_spawnRate = CurStats.spanwRate;
        m_isShooted = false;
    }

    public void Shoot(Vector3 targetDirection, float angle)
    {
        if (m_isShooted || m_spawnPos == null) return;
        if (ThunderPrefab)
        {
            var thunderPrefabClone = Instantiate(ThunderPrefab, m_spawnPos.transform.position, Quaternion.Euler(0f, 0f, angle));
        }
        m_isShooted = true;
    }

    private void DetectEnemy()
    {
        var enemyFindeds = Physics2D.OverlapCircleAll(m_player.transform.position, m_radius, EnemyDetectionLayer);

        var finalEnemy = FindNearestEnemy(enemyFindeds);
        if (finalEnemy == null) return;
        m_enemyTargeted = finalEnemy;
        Vector2 enemyDir = EnemyTargeted.transform.position - m_player.transform.position;
        enemyDir.Normalize();
        float angle = Mathf.Atan2(enemyDir.y, enemyDir.x) * Mathf.Rad2Deg;
        Shoot(enemyDir, angle);
    }

    private Actor FindNearestEnemy(Collider2D[] enemyFindeds)
    {
        float minDistance = 0f;
        Actor finalEnemy = null;
        if (enemyFindeds == null || enemyFindeds.Length <= 0) return null;
        for (int i = 0; i < enemyFindeds.Length; i++)
        {
            var enemyFinded = enemyFindeds[i];
            if (enemyFinded == null) continue;
            if (finalEnemy == null)
            {
                minDistance = Vector2.Distance(m_player.transform.position, enemyFinded.transform.position);
            }
            else
            {
                float distanceTemp = Vector2.Distance(m_player.transform.position, enemyFinded.transform.position);
                if (distanceTemp > minDistance) continue;
                minDistance = distanceTemp;
            }
            finalEnemy = enemyFinded.GetComponent<Actor>();
        }
        return finalEnemy;
    }
}
