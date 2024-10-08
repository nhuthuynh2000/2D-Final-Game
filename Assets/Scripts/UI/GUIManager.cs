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
    [SerializeField] private Text m_bulletCountingText;
    [SerializeField] private Dialog m_gunUpgradeDialog;
    [SerializeField] private Dialog m_gameOverDialog;
    [SerializeField] private Dialog m_instructionDialog;
    [SerializeField] private Dialog m_OptionsDialog;
    [SerializeField] private Dialog m_SkillDialog;
    [SerializeField] private Dialog m_ExitDialog;
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
    public void ShowInstructionDialog()
    {
        ShowDialog(m_instructionDialog);
    }
    public void CloseInstructionDialog()
    {
        if (m_instructionDialog != null) m_instructionDialog.Close();
    }
    public void ShowOptionsDialog()
    {
        ShowDialog(m_OptionsDialog);
    }
    public void CloseOptionsDialog()
    {
        if (m_OptionsDialog != null) m_OptionsDialog.Close();
    }

    public void ShowExitDialog()
    {
        Time.timeScale = 0f;
        ShowDialog(m_ExitDialog);
    }
    public void CloseExitDialog()
    {
        if (m_ExitDialog != null)
        {
            m_ExitDialog.Close();
            GameManager.Ins.ExitDialogIsOpen = false;
            Time.timeScale = 1f;
        }
    }
    public void UpdateLifeInfo(int life)
    {
        ClearLifeGrid();
        DrawLifeGrid(life);
    }
    public void ShowSkillsDialog()
    {
        ShowDialog(m_SkillDialog);
    }
    public void CloseSkillsDialog()
    {
        if (m_SkillDialog != null) m_SkillDialog.Close();
    }

    private void DrawLifeGrid(int life)
    {
        if (m_LifeGrid == null || m_lifeIconPrefab == null) return;
        for (int i = 0; i < life; i++)
        {
            var lifeIconClone = Instantiate(m_lifeIconPrefab, Vector3.zero, Quaternion.identity);
            lifeIconClone.transform.SetParent(m_LifeGrid);
            lifeIconClone.transform.localPosition = Vector3.zero;
            lifeIconClone.transform.localScale = new Vector3(1, 1, 1);
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
        m_levelProgressBar?.UpdateValue(curXp, levelUpXpNeed);
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

    public void UpdateBulletCouting(int curBullet, int bullet)
    {
        if (m_bulletCountingText)
            m_bulletCountingText.text = $"{curBullet.ToString("n0")}/{bullet.ToString("n0")}";
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
