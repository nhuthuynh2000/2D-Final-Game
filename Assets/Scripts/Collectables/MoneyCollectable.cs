public class MoneyCollectable : Collectable
{
    public override void Trigger()
    {
        Prefs.coins += m_bonus;

        GUIManager.Ins.UpdateCoinsCounting(Prefs.coins);
        //Play Sound
    }
}
