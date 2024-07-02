using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Huynh DEV/ Create Player Stats")]
public class PlayerStats : ActorStats
{
    [Header("Level Up Base: ")]
    public int level;
    public int maxLevel;
    public float xp;
    public float levelUpXPNeed;

    [Header("Level Up: ")]
    public float levelUpXPNeedUp;
    public float HpUp;

    public override bool isMaxLevel()
    {
        return level >= maxLevel;
    }

    public override void Load()
    {
        if (!string.IsNullOrEmpty(Prefs.player_datas))
        {
            JsonUtility.FromJsonOverwrite(Prefs.player_datas, this);
        }
    }

    public override void Save()
    {
        Prefs.player_datas = JsonUtility.ToJson(this);
    }

    public override void Upgrade(Action OnSuccess = null, Action OnFail = null)
    {
        while (xp >= levelUpXPNeed && !isMaxLevel())
        {
            level++;
            xp -= levelUpXPNeed;
            hp += HpUp * Helper.GetUpgradeFormula(level);
            levelUpXPNeed += levelUpXPNeedUp * Helper.GetUpgradeFormula(level);
            Save();
            OnSuccess?.Invoke();
        }
        if (xp < levelUpXPNeed && isMaxLevel())
        {
            OnFail?.Invoke();
        }
    }
}
