using UnityEngine;

public static class Prefs
{
    public static int coins
    {
        set => PlayerPrefs.SetInt(PrefConsts.COIN_KEY, value);
        get => PlayerPrefs.GetInt(PrefConsts.COIN_KEY, 0);
    }
    public static string player_datas
    {
        set => PlayerPrefs.SetString(PrefConsts.PLAYER_DATA_KEY, value);
        get => PlayerPrefs.GetString(PrefConsts.PLAYER_DATA_KEY);
    }
    public static string enemy_datas
    {
        set => PlayerPrefs.SetString(PrefConsts.ENEMY_DATA_KEY, value);
        get => PlayerPrefs.GetString(PrefConsts.ENEMY_DATA_KEY);
    }
    public static string boss_datas
    {
        set => PlayerPrefs.SetString(PrefConsts.BOSS_DATA_KEY, value);
        get => PlayerPrefs.GetString(PrefConsts.BOSS_DATA_KEY);
    }
    public static string weapons
    {
        set => PlayerPrefs.SetString(PrefConsts.WEAPON_KEY, value);
        get => PlayerPrefs.GetString(PrefConsts.WEAPON_KEY);
    }
    public static bool IsEnoughCoins(int coinToCheck)
    {
        return coins >= coinToCheck;
    }
}
