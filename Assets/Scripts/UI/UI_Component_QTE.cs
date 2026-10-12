namespace UI
{
    public partial class UI_Component_QTE
    {
        public void Update(int direction, int style)
        {
            ctrl_direction.selectedIndex = direction;
            ctrl_style.selectedIndex = style;
        }
    }
}
