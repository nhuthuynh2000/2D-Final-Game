using UnityEngine;
using Cinemachine;

public class CineController : Singleton<CineController>
{
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2.0f;

    private float m_shakeElapsedTime = 0f;

    public CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin m_virtualcameraNoise;

    public override void Awake()
    {
        MakeSingleton(false);
    }

    public override void Start()
    {
        base.Start();
        if (!virtualCamera)
            m_virtualcameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        ShakeListener();
    }

    private void ShakeListener()
    {
        if (virtualCamera && m_virtualcameraNoise)
        {
            if (m_shakeElapsedTime > 0)
            {
                m_virtualcameraNoise.m_AmplitudeGain = shakeAmplitude;
                m_virtualcameraNoise.m_FrequencyGain = shakeFrequency;

                m_shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                m_virtualcameraNoise.m_AmplitudeGain = 0f;
                m_shakeElapsedTime = 0f;
            }
        }
    }

    public void ShakeTrigger()
    {
        m_shakeElapsedTime = shakeDuration;
    }
}
