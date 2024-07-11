using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Skill Stats", menuName = "Huynh DEV/ Create Skill Stats")]
public class SkillStats : Stats
{
    [Header("Base Stats: ")]
    public float damage;
    public float coolDown;

    [Header("Upgrade: ")]
    public int level;
    public int maxLevel;
    public float damageUp;
    public float coolDownDown;
    public int upgradePrice;
    public int upgradePriceUp;

    [Header("Limit: ")]
    public float minCoolDown = 0.1f;

    //Upgrade Info

    public float damageUpInfo
    {
        get => damageUp * Helper.GetUpgradeFormula(level + 1);
    }

    public float coolDownDownInfo
    {
        get => coolDownDown * Helper.GetUpgradeFormula(level + 1);
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
            damage += damageUp * Helper.GetUpgradeFormula(level);
            coolDown -= coolDownDown * Helper.GetUpgradeFormula(level);
            upgradePrice += upgradePriceUp * level;
            Save();
            OnSuccess?.Invoke();
            return;
        }
        OnFail?.Invoke();
    }
}
