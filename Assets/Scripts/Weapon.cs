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

    private void Start()
    {
        LoadStart();
    }

    private void LoadStart()
    {
        if (!statsData) return;
        statsData.Load();
        m_curBullet = statsData.bullets;
        m_curFireRate = statsData.fireRate;
        m_curReloadTime = statsData.reloadTime;
        GUIManager.Ins.UpdateBulletCouting(m_curBullet, statsData.bullets);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        ReduceFireRate();
        ReduceReloadTime();
    }

    private void ReduceReloadTime()
    {
        if (!m_isReloading) return;
        m_curReloadTime -= Time.deltaTime;
        if (m_curReloadTime > 0) return;
        LoadStart();
        m_isReloading = false;
        GUIManager.Ins.UpdateBulletCouting(m_curBullet, statsData.bullets);
        onReloadDone?.Invoke();
    }

    private void ReduceFireRate()
    {
        if (!m_isShoot) return;
        m_curFireRate -= Time.deltaTime;
        if (m_curFireRate > 0) return;
        m_curFireRate = statsData.fireRate;
        m_isShoot = false;
    }
    public void Shoot()
    {
        if (m_isShoot || !m_shootingPoint || m_curBullet <= 0) return;
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
        m_isShoot = true;
        if (m_curBullet <=0)
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
}
