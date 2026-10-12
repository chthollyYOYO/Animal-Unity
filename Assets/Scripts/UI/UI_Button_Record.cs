using Framework;

namespace UI
{
    public partial class UI_Button_Record
    {
        public void Update(Record record)
        {
            bool hasRecord = record != null;
            ctrl_hasRecord.SetSelectedBool(hasRecord);
            if (hasRecord)
            {
                txt_info.SetVar("date", record.date.ToString())
                    .SetVar("money", record.money.ToString())
                    .FlushVars();
            }
        }
    }
}
