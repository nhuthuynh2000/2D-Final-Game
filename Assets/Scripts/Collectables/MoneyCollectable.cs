public class MoneyCollectable : Collectable
{
    public override void Trigger()
    {
        Prefs.coins += m_bonus;
        //Update UI
        //Play Sound
    }
}
