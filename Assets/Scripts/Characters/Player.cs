using System.Runtime.CompilerServices;
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
    private Vector2 m_enemyTargetedDir;
    private PlayerStats m_playerStats;
    private ShiledSkillSO m_shieldStats;

    [Header("Player Events: ")]
    public UnityEvent OnAddXP;
    public UnityEvent onLevelUp;
    public UnityEvent onLostLife;

    public PlayerStats PlayerStats { get => m_playerStats; private set => m_playerStats = value; }
    public ShiledSkillSO ShieldStats { get => m_shieldStats; private set => m_shieldStats = value; }
    public float CurSpeed { get => m_curSpeed; set => m_curSpeed = value; }

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
        SkillTrigger();
    }

    public void FixedUpdate()
    {
        if (!GameManager.Ins.IsPlaying) return;
        Move();
        WeaponHandle();
    }

    private void WeaponHandle()
    {
        if (weapon == null) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = mousePos - (Vector2)weapon.transform.position;
        shootDir.Normalize();

        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        if (m_isKnockback) return;
    }

    protected override void Move()
    {
        if (IsDead) return;

        Vector2 movingDir = Vector2.zero;

        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        movingDir = new Vector2(xDir, yDir);
        if (!m_isKnockback)
        {
            if (movingDir != Vector2.zero)
            {
                Run(movingDir);
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
        CurSpeed -= m_accelerationSpeed * Time.deltaTime;
        CurSpeed = Mathf.Clamp(CurSpeed, 0f, CurSpeed);
        m_rb.velocity = Vector2.zero;
        m_anim.SetBool(AnimConsts.PLAYER_RUN_PARAM, false);
    }

    private void Run(Vector2 movingDir)
    {
        CurSpeed += m_accelerationSpeed * Time.deltaTime;
        CurSpeed = Mathf.Clamp(CurSpeed, 0f, PlayerStats.moveSpeed);
        float delta = CurSpeed * Time.deltaTime;
        m_rb.velocity = movingDir.normalized * delta;
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
        CurHP = Mathf.Clamp(CurHP, 0f, PlayerStats.hp);
        Knockback();
        onTakeDamage?.Invoke();
        if (CurHP > 0) return;
        GameManager.Ins.GameOverChecking(OnLostLifeDelegate, OnDeadDelegate);
    }

    private void OnLostLifeDelegate()
    {
        CurHP = m_playerStats.hp;
        if (m_stopKnockbackCo != null)
        {
            StopCoroutine(m_stopKnockbackCo);
        }
        if (m_invincibleCo != null)
        {
            StopCoroutine(m_invincibleCo);
        }
        Invincible(3.5f);
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
        else if (col.gameObject.CompareTag(TagConsts.COLLECTABLE_TAG))
        {
            Collectable collectable = col.gameObject.GetComponent<Collectable>();
            collectable?.Trigger();
            Destroy(collectable.gameObject);
        }
    }

    private void SkillTrigger()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            ButtonsTrigger.Ins.TriggerQSkill();
        }
        if (Input.GetKey(KeyCode.E))
        {
            ButtonsTrigger.Ins.TriggerESkill();
        }
        if (Input.GetKey(KeyCode.R))
        {
            ButtonsTrigger.Ins.TriggerRSkill();
        }
        if (Input.GetKey(KeyCode.F))
        {
            ButtonsTrigger.Ins.TriggerFSkill();
        }
    }
}
