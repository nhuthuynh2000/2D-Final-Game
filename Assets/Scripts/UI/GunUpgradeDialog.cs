using UnityEngine;
using UnityEngine.UI;

public class GunUpgradeDialog : Dialog
{
    [SerializeField] private GunStatsDialog m_bulletStatUI;
    [SerializeField] private GunStatsDialog m_damageStatUI;
    [SerializeField] private GunStatsDialog m_firerateStatUI;
    [SerializeField] private GunStatsDialog m_reloadStatUI;
    [SerializeField] private Text m_upgradeButtonText;

    private Weapon m_weapon;
    private WeaponStats m_weaponStats;
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        Time.timeScale = 0f;
        m_weapon = GameManager.Ins.Player.weapon;
        m_weaponStats = m_weapon.statsData;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (m_weapon == null || m_weaponStats == null) return;
        if (titleText) titleText.text = $"LEVEL {m_weaponStats.level.ToString("00")}";
        if (m_upgradeButtonText) m_upgradeButtonText.text = $"UP [${m_weaponStats.upgradePrice.ToString("n0")}]";
        if (m_bulletStatUI)
        {
            m_bulletStatUI.UpdateStat("Bullets: ", m_weaponStats.bullets.ToString("n0"), $"( +{m_weaponStats.bulletsUpInfo.ToString("n0")})");
        }
        if (m_damageStatUI)
        {
            m_damageStatUI.UpdateStat("Damage: ", m_weaponStats.damage.ToString("F2"), $"( +{m_weaponStats.damageUpInfo.ToString("F3")})");
        }
        if (m_firerateStatUI)
        {
            m_firerateStatUI.UpdateStat("Firerate: ", m_weaponStats.fireRate.ToString("F2"), $"( -{m_weaponStats.fireRateDownInfo.ToString("F3")})");
        }
        if (m_reloadStatUI)
        {
            m_reloadStatUI.UpdateStat("Reload: ", m_weaponStats.reloadTime.ToString("F2"), $"( -{m_weaponStats.reloadTimeDownInfo.ToString("F3")})");
        }
    }

    public void UpgradeGun()
    {
        if (m_weaponStats == null) return;
        m_weaponStats.Upgrade(OnUpgradeSuccess, OnUpgradeFail);
    }

    private void OnUpgradeSuccess()
    {
        UpdateUI();
        GUIManager.Ins.UpdateCoinsCounting(Prefs.coins);
        AudioController.Ins.PlaySound(AudioController.Ins.upgradeSuccess);
    }

    private void OnUpgradeFail()
    {
        Debug.Log("Upgrade Failed");
    }
    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }
}
