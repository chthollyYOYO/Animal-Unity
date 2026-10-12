namespace Framework
{
    public partial class Record
    {
        public long recordId;
        public long lastSaveTime;

        public static Record Create()
        {
            Record record = new();
            record.OnCreate();
            return record;
        }

        partial void OnCreate();
    }
}
