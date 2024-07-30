using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private Slider m_musicVolumeSlider;
    [SerializeField] private Slider m_soundVolumeSlider;
    public AudioSource m_musicSource;
    public AudioSource m_soundSource;

    private float m_musicVolume
    {
        get { return m_musicVolumeSlider.value; }
        set { m_musicSource.volume = value; }
    }
    private float m_soundVolume
    {
        get { return m_soundVolumeSlider.value; }
        set { m_soundSource.volume = value; }
    }

    void Start()
    {
        m_musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        m_soundVolumeSlider.onValueChanged.AddListener(UpdateSoundVolume);
    }

    void UpdateMusicVolume(float value)
    {
        m_musicVolume = value;
    }

    void UpdateSoundVolume(float value)
    {
        m_soundVolume = value;
    }
}
