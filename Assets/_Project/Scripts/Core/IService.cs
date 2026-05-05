namespace ColorMaze.Core
{
    /// <summary>
    /// 모든 게임 서비스의 마커 인터페이스. 추후 ServiceLocator/DI 도입 시 활용.
    /// 인터페이스이므로 싱글톤 개념 없음 — 구현 클래스에서 적용 여부 결정.
    /// </summary>
    public interface IService
    {
        void Initialize();
        void Shutdown();
    }
}
