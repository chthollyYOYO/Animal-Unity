using System;

namespace Framework
{
    public interface IEventInfo
    {

    }

    public class EventInfo : IEventInfo
    {
        public Action actions;

        public EventInfo(Action action)
        {
            actions += action;
        }
    }

    public class EventInfo<T> : IEventInfo
    {
        public Action<T> actions;

        public EventInfo(Action<T> action)
        {
            actions += action;
        }
    }

    public class EventInfo<T1, T2> : IEventInfo
    {
        public Action<T1, T2> actions;

        public EventInfo(Action<T1, T2> action)
        {
            actions += action;
        }
    }
}