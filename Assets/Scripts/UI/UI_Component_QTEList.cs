using FairyGUI;

namespace UI
{
    public partial class UI_Component_QTEList
    {
        private (int, int)[] direction_styles;

        public void Update((int, int)[] direction_styles)
        {
            this.direction_styles = direction_styles;
            list_qte.itemRenderer = ItemRenderer;
            list_qte.numItems = direction_styles.Length;
        }

        private void ItemRenderer(int index, GObject obj)
        {
            var ds = direction_styles[index];
            (obj as UI_Component_QTE).Update(ds.Item1, ds.Item2);
        }
    }
}
