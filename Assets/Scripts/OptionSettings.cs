using UnityEngine;
using UnityEngine.UI;

public class OptionSettings : MonoBehaviour
{
    [SerializeField] private AudioSource m_musicSource;
    [SerializeField] private AudioSource m_soundSource;

    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private Slider m_soundVolumeSlider;

    private float m_musicVolume;
    private float m_soundVolume;

    private void Awake()
    {
        m_musicSource = AudioController.Ins.musicAus;
        m_soundSource = AudioController.Ins.sfxAus;
    }
    private void SetMusicVolume()
    {
        m_musicSource.volume = m_musicVolume;
    }
    private void GetMusicVolume()
    {
        m_musicVolume = m_musicVolumeSlider.value;
    }
    private void SetSoundVolume()
    {
        m_soundSource.volume = m_soundVolume;
    }
    private void GetSoundVolume()
    {
        m_soundVolume = m_soundVolumeSlider.value;
    }
}
