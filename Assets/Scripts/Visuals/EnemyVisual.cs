public class EnemyVisual : ActorVisual
{
    public void OnDead()
    {
        AudioController.Ins.PlaySound(AudioController.Ins.aiDeath);
        CineController.Ins.ShakeTrigger();
    }
}
