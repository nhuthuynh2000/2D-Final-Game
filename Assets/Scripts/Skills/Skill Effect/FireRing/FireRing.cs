using UnityEngine;

public class FireRing : SkillsController
{
    [SerializeField] private GameObject m_firePrefab;
    private GameObject m_firePrefabClone;
    private FireRingSkillSO m_curStats;
    private Player m_player;
    private Weapon m_weapon;
    private Transform m_spawnPos;

    private float m_onTriggerTime;

    public FireRingSkillSO CurStats { get => m_curStats; set => m_curStats = value; }

    private void Awake()
    {
        CurStats = (FireRingSkillSO)skillStats;
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
        m_onTriggerTime = m_curStats.timeTrigger;
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_spawnPos = m_weapon.ShootingPoint;
        if (m_spawnPos == null || m_firePrefab == null) return;
        m_firePrefabClone = Instantiate(m_firePrefab, m_spawnPos.transform.position, m_weapon.transform.rotation);
    }

    public void SkillUpdate()
    {
        m_onTriggerTime -= Time.deltaTime;
        if (m_onTriggerTime <= 0) Destroy(m_firePrefabClone);
        m_firePrefabClone.transform.rotation = m_weapon.transform.rotation;
        m_firePrefabClone.transform.position = m_spawnPos.transform.position;
    }
}
