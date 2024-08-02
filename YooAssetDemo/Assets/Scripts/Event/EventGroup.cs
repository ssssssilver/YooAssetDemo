using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace UniFramework.Event
{
    public class EventGroup
    {

        public class EventInfo
        {
            public object uid;
            public List<MethodInfo> _cachedListener;
        }

        public class MethodInfo
        {
            public System.Type type;
            public UnityAction<IEventMessage> unityAction;
        }


        private static readonly Dictionary<object, EventInfo> eventInfos = new Dictionary<object, EventInfo>();

        /// <summary>
        /// 添加一个监听
        /// </summary>
        public static void AddListener<TEvent>(object uid, Type type, UnityAction<TEvent> listener) where TEvent : IEventMessage
        {
            if (eventInfos.TryGetValue(uid, out var info))
            {
                var findValue = info._cachedListener.Find(_ => _.type == type);
                if (findValue != null)
                {
                    Debug.LogWarning($"Event listener is exist : {type}");
                    return;
                }
                else
                {
                    info._cachedListener.Add(new MethodInfo()
                    {
                        type = type,
                        unityAction = (message) =>
                        {
                            listener?.Invoke((TEvent)message);
                        }

                    });
                }
            }
            else
            {
                eventInfos.Add(uid, new EventInfo()
                {
                    uid = uid,
                    _cachedListener = new List<MethodInfo>()
                });
                eventInfos[uid]._cachedListener.Add(new MethodInfo()
                {
                    type = type,
                    unityAction = (message) =>
                    {
                        listener?.Invoke((TEvent)message);
                    }
                });
            }

        }

        public static void RemoveListener(object uid, System.Type type)
        {
            if (eventInfos.TryGetValue(uid, out var info))
            {
                var findValue = info._cachedListener.Find(_ => _.type == type);
                if (findValue != null)
                {
                    info._cachedListener.Remove(findValue);
                }
            }
        }

        public static void RemoveListener(object uid)
        {
            if (eventInfos.TryGetValue(uid, out var info))
            {
                foreach (var item in info._cachedListener)
                {
                    item.unityAction = null;
                }

                info._cachedListener.Clear();
            }

        }

        /// <summary>
        /// 移除所有缓存的监听
        /// </summary>
        public static void RemoveAllListener()
        {
            foreach (var pair in eventInfos)
            {
                foreach (var item in pair.Value._cachedListener)
                {
                    item.unityAction = null;
                }
                pair.Value._cachedListener.Clear();
            }
            eventInfos.Clear();
        }

        public static void SendMessage(IEventMessage eventMessage)
        {
            foreach (var pair in eventInfos)
            {
                foreach (var item in pair.Value._cachedListener)
                {
                    if (item.type == eventMessage.GetType())
                    {
                        item.unityAction?.Invoke(eventMessage);
                    }
                }
            }
        }
    }

    public static class EventExtend
    {
        public static void AddListener<TEvent>(this object target, UnityAction<TEvent> listener) where TEvent : IEventMessage
        {
            EventGroup.AddListener(target, typeof(TEvent), listener);
        }

        public static int Event<T>(this T target)
        {
            return target.GetHashCode();
        }

        public static void RemoveListener<T>(this T target)
        {
            EventGroup.RemoveListener(target.GetHashCode());
        }
        public static void RemoveAllListener<T>(this T target)
        {
            EventGroup.RemoveAllListener();
        }


    }

}