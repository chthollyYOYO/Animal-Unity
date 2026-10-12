using UnityEngine;

namespace UI
{
    public partial class UI_Panel_DialogueImageBg : IShow<Texture2D>, ISortable
    {
        int ISortable.SortingOrder => 1;

        public void OnShow(Texture2D texture)
        {
            lod.texture = new(texture);
        }
    }
}