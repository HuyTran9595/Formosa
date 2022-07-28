public class PlantData : ItemData
{
    public float ProcessTime;
    public int ProductID;
    public TJayEnums.Biome biome;
    public TJayEnums.Genus genus;
    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if (des is PlantData)
        {
            (des as PlantData).ProcessTime = this.ProcessTime;
            (des as PlantData).ProductID = this.ProductID;
            (des as PlantData).biome = this.biome;
            (des as PlantData).genus = this.genus;
        }
    }
}
