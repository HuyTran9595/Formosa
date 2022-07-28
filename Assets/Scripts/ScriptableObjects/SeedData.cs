public class SeedData : ItemData
{
    public int IdealTemperature;
    public float ProcessTime;
    public int ProductID;
    public TJayEnums.Biome biome;
    public TJayEnums.Genus genus;

    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if(des is SeedData)
        {
            (des as SeedData).IdealTemperature = this.IdealTemperature;
            (des as SeedData).ProcessTime = this.ProcessTime;
            (des as SeedData).ProductID = this.ProductID;
            (des as SeedData).biome = this.biome;
            (des as SeedData).genus = this.genus;
        }
    }
}
