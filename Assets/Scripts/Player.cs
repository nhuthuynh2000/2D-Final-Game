using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : Actor
{
    [Header("Player settings: ")]
    [SerializeField] private float m_accelerationSpeed;
    [SerializeField] private float m_maxMousePosDistance;
    [SerializeField] private Vector2 m_velocityLimit;

    [SerializeField] private float m_enemyDetectionRad;
    [SerializeField] private LayerMask m_enemyDetectionLayer;

    private float m_curSpeed;
    private Actor m_enemyTargeted;
    private Vector2 m_enemyTargetedDir;
    private PlayerStats m_playerStats;

    [Header("Player Events: ")]
    public UnityEvent OnAddXP;
    public UnityEvent onLevelUp;
    public UnityEvent onLostLife;

    public PlayerStats PlayerStats { get => m_playerStats; private set => m_playerStats = value; }

    public override void Init()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if (!statsData) return;
        m_playerStats = (PlayerStats)statsData;
        m_playerStats.Load();
        CurHP = m_playerStats.hp;
    }

    private void Update()
    {
        Move();
    }

    public void FixedUpdate()
    {
        DetectEnemy();
    }

    private void DetectEnemy()
    {
        var enemyFindeds = Physics2D.OverlapCircleAll(transform.position, m_enemyDetectionRad, m_enemyDetectionLayer);
        var finalEnemy = FindNearestEnemy(enemyFindeds);
        if (finalEnemy == null) return;
        m_enemyTargeted = finalEnemy;
        WeaponHandle();
    }

    private void WeaponHandle()
    {
        if (m_enemyTargeted == null || weapon == null) return;
        m_enemyTargetedDir = m_enemyTargeted.transform.position - weapon.transform.position;
        m_enemyTargetedDir.Normalize();
        float angle = Mathf.Atan2(m_enemyTargetedDir.y, m_enemyTargetedDir.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        if (m_isKnockback) return;
        weapon.Shoot(m_enemyTargetedDir);
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
                minDistance = Vector2.Distance(transform.position, enemyFinded.transform.position);
            }
            else
            {
                float distanceTemp = Vector2.Distance(transform.position, enemyFinded.transform.position);
                if (distanceTemp > minDistance) continue;
                minDistance = distanceTemp;
            }
            finalEnemy = enemyFinded.GetComponent<Actor>();
        }
        return finalEnemy;
    }

    protected override void Move()
    {
        if (IsDead) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 movingDir = mousePos - (Vector2)transform.position;
        movingDir.Normalize();
        if (!m_isKnockback)
        {
            if (Input.GetMouseButton(0))
            {
                Run(mousePos, movingDir);
            }
            else
            {
                BackToIdle();
            }
            return;
        }
        m_rb.velocity = m_enemyTargetedDir * -statsData.knockbackForce * Time.deltaTime;
        m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
    }

    private void BackToIdle()
    {
        m_curSpeed -= m_accelerationSpeed * Time.deltaTime;
        m_curSpeed = Mathf.Clamp(m_curSpeed, 0f, m_curSpeed);
        m_rb.velocity = Vector2.zero;
        m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
    }

    private void Run(Vector2 mousePos, Vector2 movingDir)
    {
        m_curSpeed += m_accelerationSpeed * Time.deltaTime;
        m_curSpeed = Mathf.Clamp(m_curSpeed, 0f, PlayerStats.moveSpeed);
        float delta = m_curSpeed * Time.deltaTime;
        float distanceToMousePos = Vector2.Distance(transform.position, mousePos);
        distanceToMousePos = Mathf.Clamp(distanceToMousePos, 0f, m_maxMousePosDistance / 3);
        delta *= distanceToMousePos;
        m_rb.velocity = movingDir * delta;
        float velocityLimitX = Mathf.Clamp(m_rb.velocity.x, -m_velocityLimit.x, m_velocityLimit.y);
        float velocityLimitY = Mathf.Clamp(m_rb.velocity.y, -m_velocityLimit.y, m_velocityLimit.y);
        m_rb.velocity = new Vector2(velocityLimitX, velocityLimitY);
        m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, true);
    }

    public void AddXP(float xpBonus)
    {
        if (m_playerStats == null) return;
        m_playerStats.xp += xpBonus;
        m_playerStats.Upgrade(OnUpgradeStats);

        OnAddXP?.Invoke();

        m_playerStats.Save();
    }

    private void OnUpgradeStats()
    {
        onLevelUp?.Invoke();
    }
    public override void TakeDamage(float damage)
    {
        if (damage <= 0 || m_isInvincible) return;
        CurHP -= damage;
        Knockback();
        onTakeDamage?.Invoke();
        if (CurHP > 0) return;
        GameManager.Ins.GameOverChecking(OnLostLifeDelegate,OnDeadDelegate);
    }

    private void OnLostLifeDelegate()
    {
        CurHP = m_playerStats.hp;
        onLostLife?.Invoke();
    }

    private void OnDeadDelegate()
    {
        CurHP = 0;
        Die();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                TakeDamage(enemy.CurDamage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(133, 250, 47, 50);
        Gizmos.DrawSphere(transform.position, m_enemyDetectionRad);
    }
}
