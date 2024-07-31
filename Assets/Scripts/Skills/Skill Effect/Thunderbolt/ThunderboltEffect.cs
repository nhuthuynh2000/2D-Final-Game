using System;
using System.Collections;
using System.Collections.Generic;
using DigitalRuby.LightningBolt;
using UnityEngine;

public class ThunderboltEffect : MonoBehaviour
{
    public GameObject thunderboltPrefab;
    private Weapon m_weapon;
    private Player m_player;
    private List<Enemy> affectedEnemies;
    private List<GameObject> spawnedThunderbolts;
    private Thunderbolt m_thunderbolt;
    private ThunderboltSkillSO m_thunderStats;
    private LightningBoltScript m_script;
    private float m_moveSpeed;
    private float m_firstDamage;
    private float m_previousDamage;
    private LayerMask m_enemyLayer;

    private float m_radius;


    private void OnEnable()
    {
        m_player = GameManager.Ins.Player;
        m_weapon = m_player.weapon;
        m_thunderbolt = (Thunderbolt)SkillsManager.Ins.GetSkillController(SkillType.ThunderBolt);
        m_thunderStats = m_thunderbolt.CurStats;
        m_moveSpeed = m_thunderStats.moveSpeed;
        m_firstDamage = m_thunderStats.firstTargetDamage + m_weapon.statsData.damage / 2;
        m_previousDamage = m_thunderStats.previousTargetDamage + m_weapon.statsData.damage / 2;
        m_radius = m_thunderStats.radius;
        m_enemyLayer = m_thunderbolt.EnemyDetectionLayer;
        affectedEnemies = new List<Enemy>();
        spawnedThunderbolts = new List<GameObject>();
    }

    private void Update()
    {
        transform.Translate(transform.right * m_moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnDisable()
    {
        DamageAffectedEnemies();
        DestroySpawnedThunderbolts();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.CurHP -= m_firstDamage;
            affectedEnemies.Add(enemy);
            SpreadThunderbolt(enemy);
            Destroy(gameObject);
        }
    }

    private void SpreadThunderbolt(Enemy firstEnemy)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_radius, m_enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null && !affectedEnemies.Contains(enemy))
            {
                enemy.CurHP -= m_previousDamage;
                affectedEnemies.Add(enemy);
                var thunderbolt = Instantiate(thunderboltPrefab, collider.transform.position, Quaternion.identity);
                m_script = thunderbolt.GetComponent<LightningBoltScript>();
                Debug.Log(m_script);
                m_script.StartObject = firstEnemy.gameObject;
                m_script.EndObject = enemy.gameObject;
                spawnedThunderbolts.Add(thunderbolt);
            }
        }
    }

    private void DamageAffectedEnemies()
    {
        foreach (Enemy enemy in affectedEnemies)
        {
            if (enemy != null && enemy.isActiveAndEnabled)
            {
                enemy.CurHP -= m_previousDamage;
            }
        }
        affectedEnemies.RemoveAll(enemy => enemy == null || !enemy.isActiveAndEnabled);
    }

    private void DestroySpawnedThunderbolts()
    {
        foreach (GameObject thunderbolt in spawnedThunderbolts)
        {
            if (thunderbolt != null && thunderbolt.activeInHierarchy)
            {
                Destroy(thunderbolt);
            }
        }
        spawnedThunderbolts.RemoveAll(thunderbolt => thunderbolt == null || !thunderbolt.activeInHierarchy);
    }
}
