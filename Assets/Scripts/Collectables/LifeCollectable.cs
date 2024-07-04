public class LifeCollectable : Collectable
{
    public override void Trigger()
    {
        GameManager.Ins.CurLife += m_bonus;
        //Update UI
        //Play sound
    }
}
