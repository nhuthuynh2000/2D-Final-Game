using UnityEngine;

public class Thunderbolt : SkillsController
{

    [SerializeField] private GameObject m_thunderPrefab;
    private GameObject m_thunderPrefabClone;
    private ThunderboltSkillSO m_curStats;
    private Player m_player;
    private Weapon m_weapon;
    private Transform m_spawnPos;


    public ThunderboltSkillSO CurStats { get => m_curStats; set => m_curStats = value; }

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
        InvokeRepeating("SpawnThunder", 1f, m_curStats.spanwRate);
    }

    public void SkillUpdate()
    {

    }

    private void SpawnThunder()
    {
        if (m_thunderPrefab == null || m_weapon == null || m_spawnPos == null) return;
        m_thunderPrefabClone = Instantiate(m_thunderPrefab, m_spawnPos.transform.position, m_weapon.transform.rotation);
    }
}
