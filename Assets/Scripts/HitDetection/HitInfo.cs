public class HitInfo
{
    public int Id;
    public string TargetName;
    public float Damage;
    public HitInfo(int id, string targetName, float damage)
    {
        this.Id = id; 
        this.TargetName = targetName;
        this.Damage = damage;
    }
}