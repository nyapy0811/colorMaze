using UnityEngine;

namespace ColorMaze.Core
{
    /// <summary>
    /// MonoBehaviour 싱글톤 베이스. 상속만 하면 자동 싱글톤화.
    /// 사용 예: public class AudioManager : MonoSingleton&lt;AudioManager&gt; { ... }
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _appQuitting;

        public static T Instance
        {
            get
            {
                if (_appQuitting) return null;

                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    // 씬에서 먼저 검색 (순서 무관, 어차피 1개만 존재해야 함)
                    _instance = FindAnyObjectByType<T>();
                    if (_instance != null) return _instance;

                    // 없으면 자동 생성
                    var go = new GameObject($"[{typeof(T).Name}]");
                    _instance = go.AddComponent<T>();
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        /// <summary>상속 클래스에서 추가 초기화가 필요할 때 오버라이드.</summary>
        protected virtual void OnAwake() { }

        protected virtual void OnApplicationQuit()
        {
            _appQuitting = true;
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this) _instance = null;
        }
    }
}
