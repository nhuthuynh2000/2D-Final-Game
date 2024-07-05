using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    [SerializeField] private GameObject m_homeGUI;
    [SerializeField] private GameObject m_gameGUI;
    [SerializeField] private Transform m_LifeGrid;
    [SerializeField] private GameObject m_lifeIconPrefab;
    [SerializeField] private ImageFilled m_levelProgressBar;
    [SerializeField] private ImageFilled m_hpBar;
    [SerializeField] private Text m_levelCountingText;
    [SerializeField] private Text m_xpCountingText;
    [SerializeField] private Text m_hpCountingText;
    [SerializeField] private Text m_coinCountingText;
    [SerializeField] private Text m_reloadStateText;
    [SerializeField] private Dialog m_gunUpgradeDialog;
    [SerializeField] private Dialog m_gameOverDialog;
    private Dialog m_activeDialog;

    public Dialog ActiveDialog { get => m_activeDialog; private set => m_activeDialog = value; }

    public override void Awake()
    {
        MakeSingleton(false);
    }
    public void showGameGUI(bool isShow)
    {
        if (m_gameGUI) m_gameGUI.SetActive(isShow);
        if (m_homeGUI) m_homeGUI.SetActive(!isShow);
    }
    private void ShowDialog(Dialog dialog)
    {
        if (dialog == null) return;
        m_activeDialog = dialog;
        m_activeDialog.Show(true);
    }
    public void ShowGunUpgradeDialog()
    {
        ShowDialog(m_gunUpgradeDialog);
    }
    public void ShowGameOverDialog()
    {
        ShowDialog(m_gameOverDialog);
    }
    public void UpdateLifeInfo(int life)
    {
        ClearLifeGrid();
        DrawLifeGrid(life);
    }

    private void DrawLifeGrid(int life)
    {
        if (m_LifeGrid == null || m_lifeIconPrefab == null) return;
        for (int i = 0; i < life; i++)
        {
            var lifeIconClone = Instantiate(m_lifeIconPrefab, Vector3.zero, Quaternion.identity);
            lifeIconClone.transform.SetParent(m_LifeGrid);
            lifeIconClone.transform.localPosition = Vector3.zero;
            lifeIconClone.transform.localScale = Vector3.zero;
        }
    }

    private void ClearLifeGrid()
    {
        if (m_LifeGrid == null) return;
        int lifeItemCounting = m_LifeGrid.childCount;
        for (int i = 0; i < lifeItemCounting; i++)
        {
            var lifeItem = m_LifeGrid.GetChild(i);
            if (lifeItem == null) continue;
            Destroy(lifeItem.gameObject);
        }
    }

    public void UpdateLevelInfo(int curLevel, float curXp, float levelUpXpNeed)
    {
        m_levelProgressBar?.UpdateValue(curLevel, levelUpXpNeed);
        if (m_levelCountingText) m_levelCountingText.text = $"LEVEL {curLevel.ToString("00")}";
        if (m_xpCountingText) m_xpCountingText.text = $"{curXp.ToString("00")}/{levelUpXpNeed.ToString("00")}";
    }

    public void UpdateHPInfo(float curHP, float maxHP)
    {
        m_hpBar?.UpdateValue(curHP, maxHP);
        if (m_coinCountingText) m_hpCountingText.text = $"{curHP.ToString("00")}/ {maxHP.ToString("00")} ";
    }

    public void UpdateCoinsCounting(int coin)
    {
        if (m_coinCountingText) m_coinCountingText.text = coin.ToString("n0");
    }

    public void ShowReloadText(bool isShow)
    {
        if (m_reloadStateText) m_reloadStateText.gameObject.SetActive(isShow);
    }
}
