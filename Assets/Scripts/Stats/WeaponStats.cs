using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Huynh DEV/ Create Weapon Stats")]
public class WeaponStats : Stats
{
    [Header("Base Stats: ")]
    public int bullets;
    public float fireRate;
    public float reloadTime;
    public float damage;

    [Header("Upgrade: ")]
    public int level;
    public int maxLevel;
    public int bulletsUp;
    public float fireRateDown;
    public float reloadTimeDown;
    public float damageUp;
    public int upgradePrice;
    public int upgradePriceUp;

    [Header("Limit: ")]
    public float minFireRate = 0.1f;
    public float minreloadTime = 0.1f;

    //Upgrade Info
    public int bulletsUpInfo
    {
        get => bulletsUp * (level + 1);
    }
    public float fireRateDownInfo
    {
        get => fireRateDown * Helper.GetUpgradeFormula(level + 1);
    }
    public float reloadTimeDownInfo
    {
        get => reloadTimeDown * Helper.GetUpgradeFormula(level + 1);
    }
    public float damageUpInfo
    {
        get => damageUp * Helper.GetUpgradeFormula(level + 1);
    }
    public override bool isMaxLevel()
    {
        return level >= maxLevel;
    }

    public override void Load()
    {
        if (!string.IsNullOrEmpty(Prefs.weapons))
        {
            JsonUtility.FromJsonOverwrite(Prefs.weapons, this);
        }
    }

    public override void Save()
    {
        Prefs.weapons = JsonUtility.ToJson(this);
    }

    public override void Upgrade(Action OnSuccess = null, Action OnFail = null)
    {
        if (Prefs.IsEnoughCoins(upgradePrice) && !isMaxLevel())
        {
            Prefs.coins -= upgradePrice;
            level++;
            bullets += bulletsUp * level;
            fireRate -= fireRateDown * Helper.GetUpgradeFormula(level);
            fireRate = Mathf.Clamp(fireRate, minFireRate, fireRate);
            reloadTime -= reloadTimeDown * Helper.GetUpgradeFormula(level);
            reloadTime = Mathf.Clamp(reloadTime, minreloadTime, reloadTime);
            damage += damageUp * Helper.GetUpgradeFormula(level);
            upgradePrice += upgradePriceUp * level;
            Save();
            OnSuccess?.Invoke();
            return;
        }
        OnFail?.Invoke();
    }
}
