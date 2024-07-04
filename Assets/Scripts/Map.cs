using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform playerSpawnPoint;
    [SerializeField] private Transform[] m_enemySpawnPoints;

    public Transform randomEnemySpawnPoint
    {
        get
        {
            if (m_enemySpawnPoints == null || m_enemySpawnPoints.Length <= 0) return null;
            int randomIndex = Random.Range(0, m_enemySpawnPoints.Length);
            return m_enemySpawnPoints[randomIndex];
        }
    }
}
