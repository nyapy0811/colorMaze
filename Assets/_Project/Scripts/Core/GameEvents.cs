namespace ColorMaze.Core
{
    // 상태 전이
    public readonly struct GameStateChangedEvent
    {
        public readonly GameState From;
        public readonly GameState To;
        public GameStateChangedEvent(GameState from, GameState to)
        {
            From = from; To = to;
        }
    }

    // 플레이어
    public readonly struct PlayerDiedEvent { /* TODO: 사망 위치 등 */ }
    public readonly struct PlayerRespawnedEvent { /* TODO */ }

    // 퍼즐
    public readonly struct PuzzleSolvedEvent
    {
        public readonly string PuzzleId;
        public PuzzleSolvedEvent(string id) { PuzzleId = id; }
    }

    // 트랩
    public readonly struct TrapTriggeredEvent
    {
        public readonly string TrapId;
        public TrapTriggeredEvent(string id) { TrapId = id; }
    }

    // 레벨
    public readonly struct LevelClearedEvent { /* TODO */ }
}
