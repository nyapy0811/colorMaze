using UnityEngine;

namespace ColorMaze.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public GameState CurrentState { get; private set; }

        protected override void OnAwake()
        {
            
        }

        public void ChangeState(GameState next)
        {
            if (CurrentState == next) return;
            var prev = CurrentState;
            OnStateExit(CurrentState);
            CurrentState = next;
            OnStateEnter(next);
            EventBus.Publish(new GameStateChangedEvent(prev, next));
        }

        private void OnStateEnter(GameState s)
        {
            switch (s)
            {
                case GameState.Playing:
                case GameState.Menu:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                case GameState.Cleared:
                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;

                case GameState.Booting:
                    // 부팅 중에는 timeScale 손대지 않음
                    break;
            }
        }

        private void OnStateExit(GameState s)
        {
            // 현재는 특별한 정리 로직 없음.
            // 추후 상태별 리소스 해제가 필요할 때 채워넣음.
        }

        // TODO: StartGame(), PauseGame(), ResumeGame(), LoadLevel(string sceneName) 등
    }
}