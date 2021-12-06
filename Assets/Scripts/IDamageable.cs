public interface IDamageable
{
    public void TakeDamage();

    public bool CreateExplosion();

    public bool IsSucessfullHit(out int hitPoints);
}