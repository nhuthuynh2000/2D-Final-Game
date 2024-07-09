using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunStatsDialog : MonoBehaviour
{
    [SerializeField] private Text m_statLabelText;
    [SerializeField] private Text m_statValueText;
    [SerializeField] private Text m_statUpValueText;
    public void UpdateStat(string label, string value, string upValue)
    {
        if (m_statLabelText) m_statLabelText.text = label;
        if (m_statValueText) m_statValueText.text = value;
        if (m_statUpValueText) m_statUpValueText.text = upValue;
    }
}
