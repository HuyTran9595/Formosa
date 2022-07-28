public class RecipeData : ItemData
{
    public float ProcessTime;
    public int FirstIngredient;
    public int SecondIngredient;
    public int ProductID;

    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if (des is RecipeData)
        {
            (des as RecipeData).ProcessTime = this.ProcessTime;
            (des as RecipeData).FirstIngredient = this.FirstIngredient;
            (des as RecipeData).SecondIngredient = this.SecondIngredient;
            (des as RecipeData).ProductID = this.ProductID;
        }
    }
}
