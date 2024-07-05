public class LifeCollectable : Collectable
{
    public override void Trigger()
    {
        GameManager.Ins.CurLife += m_bonus;

        GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);
        //Play sound
    }
}
