public class TaskData : ItemData
{
    public int UnlockNextTaskID;
    public TJayEnums.TaskType TaskType;
    public int IDForPlant;
    public int ExpEarn;
    public bool isFinished;

    public override void Copy(out ItemData des)
    {
        base.Copy(out des);
        if (des is TaskData)
        {
            (des as TaskData).UnlockNextTaskID = this.UnlockNextTaskID;
            (des as TaskData).TaskType = this.TaskType;
            (des as TaskData).IDForPlant = this.IDForPlant;
            (des as TaskData).ExpEarn = this.ExpEarn;
        }
    }
}

