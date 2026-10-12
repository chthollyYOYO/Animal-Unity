using System;
using System.Collections.Generic;

namespace Framework
{
    public class EventCenter : Singleton<EventCenter>
    {
        private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

        public void AddEventListener(string name, Action action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventInfo(action));
            }
        }

        public void AddEventListener<T>(string name, Action<T> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventInfo<T>(action));
            }
        }

        public void AddEventListener<T1, T2>(string name, Action<T1, T2> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2>).actions += action;
            }
            else
            {
                eventDic.Add(name, new EventInfo<T1, T2>(action));
            }
        }

        public void RemoveEventListener(string name, Action action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions -= action;
            }
        }

        public void RemoveEventListener<T>(string name, Action<T> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions -= action;
            }
        }

        public void RemoveEventListener<T1, T2>(string name, Action<T1, T2> action)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2>).actions -= action;
            }
        }

        public void SendEvent(string name)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo).actions?.Invoke();
            }
        }

        public void SendEvent<T>(string name, T info)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T>).actions?.Invoke(info);
            }
        }

        public void SendEvent<T1, T2>(string name, T1 info1, T2 info2)
        {
            if (eventDic.ContainsKey(name))
            {
                (eventDic[name] as EventInfo<T1, T2>).actions?.Invoke(info1, info2);
            }
        }

        public void Clear()
        {
            eventDic.Clear();
        }
    }
}

