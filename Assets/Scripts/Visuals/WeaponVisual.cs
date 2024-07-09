using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private AudioClip m_shootingSound;
    [SerializeField] private AudioClip m_reloadSound;

    public void OnShoot()
    {
        AudioController.Ins.PlaySound(m_shootingSound);
        CineController.Ins.ShakeTrigger();
    }

    public void OnReload()
    {
        GUIManager.Ins.ShowReloadText(true);
    }

    public void OnReloadDone()
    {
        AudioController.Ins.PlaySound(m_reloadSound);
        GUIManager.Ins.ShowReloadText(false);
    }
}
