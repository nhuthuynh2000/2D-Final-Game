using UnityEngine;

[System.Serializable]
public class CollectableItems
{
    [Range(0f, 1f)]
    public float spawnRate;
    public int amount;
    public Collectable collectablePrefab;
}
public class CollectablesManager : Singleton<CollectablesManager>
{
    [SerializeField] private CollectableItems[] m_items;

    public void Spawn(Vector3 position)
    {
        if (m_items == null || m_items.Length <= 0) return;
        float spawnRateChecking = Random.value;
        for (int i = 0; i < m_items.Length; i++)
        {
            var item = m_items[i];
            if (item == null || item.spawnRate < spawnRateChecking) continue;
            CreateCollectable(position, item);
        }
    }

    private void CreateCollectable(Vector3 spawnPos, CollectableItems collectableItems)
    {
        if (collectableItems == null) return;
        for (int i = 0; i < collectableItems.amount; i++)
        {
            Instantiate(collectableItems.collectablePrefab, spawnPos, Quaternion.identity);
        }
    }
    public override void Awake()
    {
        MakeSingleton(false);
    }
}
