using System.Collections;
using DEV;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Actor : MonoBehaviour
{
    [Header("Commons: ")]
    public ActorStats statsData;
    [LayerList]
    [SerializeField] private int m_invincibleLayer;
    [LayerList]
    [SerializeField] private int m_normalLayer;
    public Weapon weapon;
    protected bool m_isKnockback;
    protected bool m_isInvincible;
    private bool m_isDead;
    private float m_curHP;
    protected Rigidbody2D m_rb;
    protected Animator m_anim;

    [Header("Events: ")]
    public UnityEvent onIn;
    public UnityEvent onTakeDamage;
    public UnityEvent onDead;

    public bool IsDead { get => m_isDead; set => m_isDead = value; }
    public float CurHP { get => m_curHP; set => m_curHP = value; }

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        Init();
        onIn?.Invoke();
    }

    public virtual void Init()
    {

    }

    public virtual void TakeDamage(float damage)
    {
        if (damage < 0 || m_isInvincible) return;
        m_curHP -= damage;
        Knockback();
        if (m_curHP <= 0)
        {
            m_curHP = 0;
            Die();
        }
        onTakeDamage?.Invoke();
    }

    protected virtual void Die()
    {
        m_isDead = true;
        m_rb.velocity = Vector3.zero;
        onDead?.Invoke();
        Destroy(gameObject, 0.5f);
    }

    public void Knockback()
    {
        if (m_isInvincible || m_isKnockback || IsDead) return;
        m_isKnockback = true;
        StartCoroutine(StopKnockback());
    }

    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(statsData.knockbackTime);
        m_isKnockback = false;
        m_isInvincible = true;
        gameObject.layer = m_invincibleLayer;
        StartCoroutine(StopInvincible());
    }

    private IEnumerator StopInvincible()
    {
        yield return new WaitForSeconds(statsData.invicibleTime);
        m_isInvincible = false;
        gameObject.layer = m_normalLayer;
    }
    protected virtual void Move()
    {

    }
}
