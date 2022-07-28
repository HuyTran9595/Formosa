public class PotionData : ItemData
{
    public TJayEnums.PotionType PotionType;
    public float Duration;
    public int BoostSeedID;

    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if (des is PotionData)
        {
            (des as PotionData).PotionType = this.PotionType;
            (des as PotionData).Duration = this.Duration;
            (des as PotionData).BoostSeedID = this.BoostSeedID;
        }
    }
}
