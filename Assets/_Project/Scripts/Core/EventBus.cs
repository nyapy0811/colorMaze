using System;
using System.Collections.Generic;
using UnityEngine;

namespace ColorMaze.Core
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _handlers = new();

        #if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnPlay()
        {
            _handlers.Clear();
        }
        #endif

        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            if (handler == null) return;

            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var existing))
                _handlers[type] = Delegate.Combine(existing, handler);
            else
                _handlers[type] = handler;
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            if (handler == null) return;

            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var existing)) return;

            var remaining = Delegate.Remove(existing, handler);
            if (remaining == null)
                _handlers.Remove(type);
            else
                _handlers[type] = remaining;
        }

        public static void Publish<T>(T evt) where T : struct
        {
            if (!_handlers.TryGetValue(typeof(T), out var del)) return;
            if (del is not Action<T> typed) return;

            // GetInvocationList로 개별 호출 → 한 핸들러가 throw해도 나머지 진행
            foreach (Action<T> h in typed.GetInvocationList())
            {
                try { h.Invoke(evt); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
        }

        public static void Clear()
        {
            _handlers.Clear();
        }
    }
}