using Framework;

namespace UI
{
    public partial class UI_Panel_Loading : ICreate, IShow, IHide, ISortable
    {
        public int SortingOrder => 100;

        public void OnCreate()
        {
            prog.max = 1;
        }

        public void OnShow()
        {
            LoadingManager.Instance.OnLoadingProcessChanged += OnLoadingProcessChanged;
            Update();
        }

        public void OnHide()
        {
            LoadingManager.Instance.OnLoadingProcessChanged -= OnLoadingProcessChanged;
        }

        private void OnLoadingProcessChanged()
        {
            Update();
        }

        private void Update()
        {
            prog.value = LoadingManager.Instance.LoadingProcess;
        }
    }
}