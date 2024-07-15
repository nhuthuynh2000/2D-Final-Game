using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("Commons: ")]
    public WeaponStats statsData;
    [SerializeField] private Transform m_shootingPoint;
    [SerializeField] private GameObject m_bullet;
    [SerializeField] private GameObject m_muzzleFlash;
    private float m_curFireRate;
    private int m_curBullet;
    private float m_curReloadTime;
    private bool m_isShoot;
    private bool m_isReloading;

    [Header("Events: ")]
    public UnityEvent onShoot;
    public UnityEvent onReload;
    public UnityEvent onReloadDone;

    public float CurFireRate { get => m_curFireRate; set => m_curFireRate = value; }
    public float CurReloadTime { get => m_curReloadTime; set => m_curReloadTime = value; }
    public bool IsShoot { get => m_isShoot; set => m_isShoot = value; }

    private void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if (!statsData) return;
        statsData.Load();
        m_curBullet = statsData.bullets;
        CurFireRate = statsData.fireRate;
        CurReloadTime = statsData.reloadTime;
        GUIManager.Ins.UpdateBulletCouting(m_curBullet, statsData.bullets);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        ReduceFireRate();
        ReduceReloadTime();
    }

    private void ReduceReloadTime()
    {
        if (!m_isReloading) return;
        CurReloadTime -= Time.deltaTime;
        if (CurReloadTime > 0) return;
        LoadStats();
        m_isReloading = false;
        GUIManager.Ins.UpdateBulletCouting(m_curBullet, statsData.bullets);
        onReloadDone?.Invoke();
    }

    private void ReduceFireRate()
    {
        if (!IsShoot) return;
        CurFireRate -= Time.deltaTime;
        if (CurFireRate > 0) return;

        CurFireRate = statsData.fireRate;
        IsShoot = false;

    }
    public void Shoot()
    {
        if (IsShoot || !m_shootingPoint || m_curBullet <= 0) return;
        if (m_muzzleFlash)
        {
            var muzzleFlashClone = Instantiate(m_muzzleFlash, m_shootingPoint.position, transform.rotation);
            muzzleFlashClone.transform.SetParent(m_shootingPoint);
        }
        if (m_bullet)
        {
            var bulletClone = Instantiate(m_bullet, m_shootingPoint.position, transform.rotation);
            var projectilesComp = bulletClone.GetComponent<Projectiles>();
            if (projectilesComp)
            {
                projectilesComp.Damage = statsData.damage;
            }
        }
        m_curBullet--;
        GUIManager.Ins.UpdateBulletCouting(m_curBullet, statsData.bullets);
        IsShoot = true;
        if (m_curBullet <= 0)
        {
            Reload();
        }
        onShoot?.Invoke();
    }
    public void Reload()
    {
        m_isReloading = true;
        onReload?.Invoke();
    }
    public void ResetFireRate(float originalFireRate)
    {
        statsData.fireRate = originalFireRate;
        m_curFireRate = originalFireRate;
    }

    public void ResetReloadTime(float originalReloadTime)
    {
        statsData.reloadTime = originalReloadTime;
        m_curReloadTime = originalReloadTime;
    }
}
