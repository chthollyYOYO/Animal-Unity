namespace UI
{
    public partial class UI_Button_TableItems
    {
        public void Update(int tableId, bool isFull)
        {
            txt_id.SetVar("value", tableId.ToString()).FlushVars();
            ctrl_full.SetSelectedBool(isFull);
        }
    }
}