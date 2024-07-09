using UnityEngine;

[RequireComponent(typeof(Actor))]
public class ActorVisual : MonoBehaviour
{
    private FlashVfx m_flashVFX;
    protected Actor m_actor;
    protected virtual void Awake()
    {
        m_flashVFX = GetComponent<FlashVfx>();
        m_actor = GetComponent<Actor>();
    }

    public virtual void OnTakeDamage()
    {
        if (m_flashVFX == null || m_actor == null || m_actor.IsDead) return;
        m_flashVFX.Flash(m_actor.statsData.knockbackTime);
    }
}
