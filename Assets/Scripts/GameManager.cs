using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public enum GameStates
{
    STARTING,
    PLAYING,
    PAUSING,
    GAMEOVER
}

public class GameManager : Singleton<GameManager>
{
    public static GameStates State;
    [Header("Camera Setting: ")]
    public CinemachineVirtualCamera mainCamera;
    public float cameraDistance = 5f;
    [SerializeField] private SkillButtonsDrawers m_skillButtonsDrawer;
    [SerializeField] private Map m_mapPrefab;
    [SerializeField] private Player m_playerPrefab;
    [SerializeField] private Enemy[] m_enemyPrefab;
    [SerializeField] private Boss[] m_bossPrefab;
    [SerializeField] private GameObject m_enemySpawnVFX;
    [SerializeField] private float m_enemySpawnTime;
    [SerializeField] private int m_playerMaxLife;
    [SerializeField] private int m_playerStartingLife;
    private Map m_map;
    private Player m_player;
    private PlayerStats m_playerStats;
    private int m_curLife;
    private bool m_isPlaying;
    public bool ExitDialogIsOpen = false;

    public Player Player { get => m_player; private set => m_player = value; }
    public int CurLife
    {
        get => m_curLife;
        set
        {
            m_curLife = value;
            m_curLife = Mathf.Clamp(m_curLife, 0, m_playerMaxLife);
        }
    }

    public bool IsPlaying { get => m_isPlaying; set => m_isPlaying = value; }
    public SkillButtonsDrawers SkillButtonsDrawer { get => m_skillButtonsDrawer; }

    public override void Start()
    {
        Init();
        SkillsManager.Ins?.AddSkill(SkillType.FireRing, 99);
        SkillsManager.Ins?.AddSkill(SkillType.Shield, 99);
        SkillsManager.Ins?.AddSkill(SkillType.ThunderBolt, 99);
        SkillsManager.Ins?.AddSkill(SkillType.FireRateUp, 99);
        SkillButtonsDrawer?.DrawSkillButton();
    }

    private void Init()
    {
        State = GameStates.STARTING;
        m_curLife = m_playerStartingLife;
        SpawnMap_Player();
        IsPlaying = false;
        GUIManager.Ins.showGameGUI(false);
    }
    private void SpawnMap_Player()
    {
        if (m_mapPrefab == null || m_playerPrefab == null) return;
        m_map = Instantiate(m_mapPrefab, Vector3.zero, Quaternion.identity);
        m_player = Instantiate(m_playerPrefab, m_map.playerSpawnPoint.position, Quaternion.identity);
        mainCamera.Follow = m_player.transform;
    }

    public void PlayGame()
    {
        IsPlaying = true;
        State = GameStates.PLAYING;
        m_playerStats = m_player.PlayerStats;
        if (m_player)
        {

        }
        SpawnEnemy();
        if (m_player == null || m_playerStats == null) return;
        GUIManager.Ins.showGameGUI(true);
        GUIManager.Ins.UpdateLifeInfo(m_curLife);
        GUIManager.Ins.UpdateCoinsCounting(Prefs.coins);
        GUIManager.Ins.UpdateHPInfo(m_player.CurHP, m_playerStats.hp);
        GUIManager.Ins.UpdateLevelInfo(m_playerStats.level, m_playerStats.xp, m_playerStats.levelUpXPNeed);
    }

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ExitDialogIsOpen == false)
            {
                ExitDialogIsOpen = true;
                GUIManager.Ins.ShowExitDialog();
                Time.timeScale = 0f;
            }
            else if (ExitDialogIsOpen == true)
            {
                ExitDialogIsOpen = false;
                Time.timeScale = 1f;
                GUIManager.Ins.CloseExitDialog();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Prefs.coins += 10000;
            GUIManager.Ins.UpdateCoinsCounting(Prefs.coins);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_player.PlayerStats.level += 100;
            GUIManager.Ins.UpdateLevelInfo(m_player.PlayerStats.level, 0, m_player.PlayerStats.levelUpXPNeed);
        }
    }

    private void SpawnEnemy()
    {
        var randomEnemy = GetRandomEnemy();
        if (randomEnemy == null || m_map == null) return;
        StartCoroutine(SpawnEnemy_Coroutine(randomEnemy));
    }

    private Enemy GetRandomEnemy()
    {
        if (m_enemyPrefab == null || m_bossPrefab.Length <= 0) return null;
        int randomIndex = UnityEngine.Random.Range(0, m_bossPrefab.Length);
        return m_enemyPrefab[randomIndex];
    }

    private IEnumerator SpawnEnemy_Coroutine(Enemy randomEnemy)
    {
        yield return new WaitForSeconds(3f);
        while (State == GameStates.PLAYING)
        {
            if (m_map.randomEnemySpawnPoint == null) break;
            Vector3 spawnPoint = m_map.randomEnemySpawnPoint.position;
            if (m_enemySpawnVFX) Instantiate(m_enemySpawnVFX, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(randomEnemy, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(m_enemySpawnTime);
        }
        yield return null;
    }

    public void GameOverChecking(Action OnLostLife = null, Action OnDead = null)
    {
        if (m_curLife <= 0) return;
        m_curLife--;
        OnLostLife?.Invoke();
        if (m_curLife <= 0)
        {
            IsPlaying = false;
            State = GameStates.GAMEOVER;
            OnDead?.Invoke();
        }
    }
}
