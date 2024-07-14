using UnityEngine;
using UnityEngine.Events;

public class SkillsController : MonoBehaviour
{
    public SkillType skillType;
    public SkillSO skillStats;

    protected bool m_isTriggered;
    protected bool m_isCoolDowning;

    protected float m_triggerTime;
    protected float m_coolDownTime;

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnSkillUpdate;
    public UnityEvent OnCoolDown;
    public UnityEvent OnStop;
    public UnityEvent<SkillType, int> OnStopWithType;
    public UnityEvent OnCoolDownStop;

    public float coolDownProgress
    {
        get => CoolDownTime / skillStats.coolDownTime;
    }

    public float triggerProgress
    {
        get => m_triggerTime / skillStats.timeTrigger;
    }
    public bool IsTriggered { get => m_isTriggered; }
    public bool IsCoolDowning { get => m_isCoolDowning; }
    public float CoolDownTime { get => m_coolDownTime; }

    public virtual void LoadStats()
    {
        if (skillStats == null) return;
        m_coolDownTime = skillStats.coolDownTime;
        m_triggerTime = skillStats.timeTrigger;
    }

    public void Trigger()
    {
        if (m_isTriggered || m_isCoolDowning) return;
        m_isTriggered = true;
        m_isCoolDowning = true;
        OnTriggerEnter?.Invoke();
    }

    private void Update()
    {
        CoreHandle();
    }

    private void CoreHandle()
    {
        ReduceTriggerTime();
        ReduceCooldownTime();
    }

    private void ReduceTriggerTime()
    {
        if (!m_isTriggered) return;
        m_triggerTime -= Time.deltaTime;
        if (m_triggerTime <= 0)
        {
            Stop();
        }
        OnSkillUpdate?.Invoke();
    }

    private void ReduceCooldownTime()
    {
        if (!m_isCoolDowning) return;
        m_coolDownTime -= Time.deltaTime;
        OnCoolDown?.Invoke();
        if (m_coolDownTime > 0) return;
        m_isCoolDowning = false;
        OnCoolDownStop?.Invoke();
        m_coolDownTime = skillStats.coolDownTime;
    }

    public void Stop()
    {
        m_triggerTime = skillStats.timeTrigger;
        m_isTriggered = false;
        OnStopWithType?.Invoke(skillType, 1);
        OnStop?.Invoke();
    }

    public void ForceStop()
    {
        m_isCoolDowning = false;
        m_isTriggered = false;
        LoadStats();
    }
}
