using UnityEngine;
using UnityEngine.Events;

public class Skills : MonoBehaviour
{
    [Header("Commons: ")]
    public SkillStats statsData;
    protected Rigidbody2D m_rb;
    protected Animator m_anim;

    [Header("Events: ")]
    public UnityEvent OnCast;
    public UnityEvent OnDamaga;
    public UnityEvent OnOver;

    private Enemy m_enemy;

    protected virtual void Awake()
    {
        m_anim = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        InIt();
        OnCast?.Invoke();
    }

    public virtual void InIt()
    {

    }

    public virtual void DealDamageAndEffect(float damage)
    {

    }
}
