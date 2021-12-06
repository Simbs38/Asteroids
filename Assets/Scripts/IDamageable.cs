public interface IDamageable
{
    public void TakeDamage();

    public bool IsSucessfullHit(out int hitPoints);
}