using UnityEngine;
using UnityEngine.UI;

public class SkillButtons : MonoBehaviour
{
    [SerializeField] private Image m_skillIcon;
    [SerializeField] private Image m_cooldownOverlay;
    [SerializeField] private Image m_timeTriggerFilled;
    [SerializeField] private Text m_amountText;
    [SerializeField] private Text m_cooldownText;
    [SerializeField] private Button m_btnComp;

    private SkillType m_skillType;
    private SkillsController m_skillController;
    private int m_currentAmount;
    #region EVENTS
    private void RegisterEvents()
    {
        if (m_skillController == null) return;
        m_skillController.OnCoolDown.AddListener(UpdateCooldownText);
        m_skillController.OnSkillUpdate.AddListener(UpdateTimeTrigger);
        m_skillController.OnCoolDownStop.AddListener(UpdateUI);
    }

    private void UnRegisterEvents()
    {
        if (m_skillController == null) return;
        m_skillController.OnCoolDown.RemoveListener(UpdateCooldownText);
        m_skillController.OnSkillUpdate.RemoveListener(UpdateTimeTrigger);
        m_skillController.OnCoolDownStop.RemoveListener(UpdateUI);
    }

    #endregion

    public void Initialize(SkillType type)
    {
        m_skillType = type;
        m_skillController = SkillsManager.Ins.GetSkillController(type);
        m_timeTriggerFilled.transform.parent.gameObject.SetActive(false);

        UpdateUI();

        if (m_btnComp != null)
        {
            m_btnComp.onClick.RemoveAllListeners();
            m_btnComp.onClick.AddListener(TriggerSkill);
        }
        RegisterEvents();
    }

    private void UpdateUI()
    {
        if (m_skillController == null) return;
        if (m_skillIcon)
        {
            m_skillIcon.sprite = m_skillController.skillStats.skillIcon;
        }
        UpdateAmountText();
        UpdateCooldownText();
        UpdateTimeTrigger();
        bool canActiveMe = m_currentAmount > 0 || m_skillController.IsCoolDowning;
        gameObject.SetActive(canActiveMe);
    }

    private void UpdateTimeTrigger()
    {
        if (m_skillController == null || m_timeTriggerFilled == null) return;
        float triggerProgress = m_skillController.triggerProgress;
        m_timeTriggerFilled.fillAmount = triggerProgress;
        m_timeTriggerFilled.transform.parent.gameObject.SetActive(m_skillController.IsTriggered);
    }

    private void UpdateCooldownText()
    {
        if (m_cooldownText)
        {
            m_cooldownText.text = m_skillController.CoolDownTime.ToString("f1");
        }
        float coolDownProgress = m_skillController.coolDownProgress;

        if (m_cooldownOverlay)
        {
            m_cooldownOverlay.fillAmount = coolDownProgress;
            m_cooldownOverlay.gameObject.SetActive(m_skillController.IsCoolDowning);
        }
    }

    private void UpdateAmountText()
    {
        m_currentAmount = SkillsManager.Ins.GetSkillAmount(m_skillType);
        if (m_amountText)
        {
            m_amountText.text = $"x{m_currentAmount}";
        }
    }

    public void TriggerSkill()
    {
        if (m_skillController == null) return;
        m_skillController.Trigger();
    }

    private void OnDestroy()
    {
        UnRegisterEvents();
    }
}
