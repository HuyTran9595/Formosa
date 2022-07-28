public class DryHerbData : ItemData
{
    public TJayEnums.Biome biome;
    public TJayEnums.Genus genus;

    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if (des is DryHerbData)
        {
            (des as DryHerbData).biome = this.biome;
            (des as DryHerbData).genus = this.genus;
        }
    }
}
