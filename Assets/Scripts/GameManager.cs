using System;
using System.Collections;
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
    public Camera mainCamera;
    public float cameraDistance = 5f;
    [SerializeField] private Map m_mapPrefab;
    [SerializeField] private Player m_playerPrefab;
    [SerializeField] private Enemy[] m_enemyPrefab;
    [SerializeField] private GameObject m_enemySpawnVFX;
    [SerializeField] private float m_enemySpawnTime;
    [SerializeField] private int m_playerMaxLife;
    [SerializeField] private int m_playerStartingLife;

    private Map m_map;
    private Player m_player;
    private int m_curLife;


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

    public override void Start()
    {
        Init();
    }

    private void Init()
    {
        State = GameStates.PLAYING; //PLAYING for testing
        m_curLife = m_playerStartingLife;

        SpawnMap_Player();
        PlayGame(); // FOR TESTING
    }

    public override void Awake()
    {
        MakeSingleton(false);
    }

    private void SpawnMap_Player()
    {
        if (m_mapPrefab == null || m_playerPrefab == null) return;
        m_map = Instantiate(m_mapPrefab, Vector3.zero, Quaternion.identity);
        m_player = Instantiate(m_playerPrefab, m_map.playerSpawnPoint.position, Quaternion.identity);
        PositionCameraBehindPlayer();
    }

    private void PositionCameraBehindPlayer()
    {
        // Tính vị trí camera dựa trên vị trí của player
        Vector3 cameraPosition = m_player.transform.position;
        cameraPosition += -m_player.transform.forward * cameraDistance;

        // Cập nhật vị trí camera
        mainCamera.transform.position = cameraPosition;

        // Xoay camera để hướng về phía player
        mainCamera.transform.LookAt(m_player.transform);
    }

    private void LateUpdate()
    {
        PositionCameraBehindPlayer();
    }

    public void PlayGame()
    {
        if (m_player)
        {

        }
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        var randomEnemy = GetRandomEnemy();
        if (randomEnemy == null || m_map == null) return;
        StartCoroutine(SpawnEnemy_Coroutine(randomEnemy));
    }

    private Enemy GetRandomEnemy()
    {
        if (m_enemyPrefab == null || m_enemyPrefab.Length <= 0) return null;
        int randomIndex = UnityEngine.Random.Range(0, m_enemyPrefab.Length);
        return m_enemyPrefab[randomIndex];
    }

    private IEnumerator SpawnEnemy_Coroutine(Enemy randomEnemy)
    {
        yield return new WaitForSeconds(3);
        State = GameStates.PLAYING;
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
            State = GameStates.GAMEOVER;
            OnDead?.Invoke();
            Debug.Log("GameOver!!!!");
        }
    }
}
