public class LifeCollectable : Collectable
{
    public override void Trigger()
    {
        GameManager.Ins.CurLife += 1;

        GUIManager.Ins.UpdateLifeInfo(GameManager.Ins.CurLife);
        AudioController.Ins.PlaySound(AudioController.Ins.lifePickup);
    }
}
