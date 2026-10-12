namespace UI
{
    //---------- UI生命周期 ----------

    /// <summary>
    /// 创建时调用，建议初始化内部组件、订阅内部组件的消息、加载和创建外部资源
    /// 注意：组件用于列表项时不会调用此方法
    /// </summary>
    public interface ICreate
    {
        void OnCreate();
    }

    /// <summary>
    /// 销毁时调用，建议销毁和卸载外部资源
    /// 注意：组件用于列表项时不会调用此方法
    /// </summary>
    public interface IDestroy
    {
        void OnDestroy();
    }

    /// <summary>
    /// 显示时调用，建议更新内部组件、订阅外部消息
    /// 注意：组件用于列表项时不会调用此方法
    /// </summary>
    public interface IShow
    {
        void OnShow();
    }

    public interface IShow<T>
    {
        void OnShow(T param);
    }

    /// <summary>
    /// 隐藏时调用，建议取消订阅外部消息
    /// 注意：组件用于列表项时不会调用此方法
    /// </summary>
    public interface IHide
    {
        void OnHide();
    }

    //---------- UI生命周期End ----------

    /// <summary>
    /// 可以设置UI渲染顺序，大值面板覆盖在小值面板上
    /// 默认值为0，同值面板按照添加顺序排列
    /// </summary>
    public interface ISortable
    {
        int SortingOrder { get; }
    }

    /// <summary>
    /// 显式声明面板为全屏，与INotFullScreen互斥
    /// 如果面板分辨率与设计分辨率一致，则默认为全屏
    /// </summary>
    public interface IFullScreen
    {

    }

    /// <summary>
    /// 显式声明面板为非全屏，与IFullScreen互斥
    /// 如果面板分辨率与设计分辨率一致，则默认为全屏
    /// </summary>
    public interface INotFullScreen
    {

    }

    /// <summary>
    /// 声明面板为主面板
    /// 显示主面板A时，当前主面板B会被隐藏并存入栈中；待A隐藏时，B会被弹出并显示
    /// </summary>
    public interface IMainPanel
    {

    }

    /// <summary>
    /// 声明面板禁用深度调整
    /// </summary>
    public interface INotFairyBatching
    {

    }

    /// <summary>
    /// 声明面板禁用缓存
    /// </summary>
    public interface INotCache
    {

    }
}