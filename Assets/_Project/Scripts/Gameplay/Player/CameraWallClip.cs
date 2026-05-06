using UnityEngine;

namespace ColorMaze.Gameplay.Player
{
    /// <summary>
    /// Main Camera에 부착. Cinemachine이 카메라 위치를 정한 뒤 LateUpdate에서
    /// 벽 충돌을 검사해 벽 앞쪽으로 카메라를 끌어당김.
    /// 반드시 CinemachineBrain보다 늦게 실행되어야 함 (DefaultExecutionOrder).
    /// </summary>
    [DefaultExecutionOrder(10000)]
    public class CameraWallClip : MonoBehaviour
    {
        [SerializeField] private Transform target;        // CameraTarget (Player 자식)
        [SerializeField] private float radius = 0.3f;
        [SerializeField] private float wallBuffer = 0.1f; // 벽에서 떨어질 거리
        [SerializeField] private LayerMask obstacleLayers;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 origin = target.position;
            Vector3 toCam = transform.position - origin;
            float dist = toCam.magnitude;
            if (dist < 0.001f) return;

            Vector3 dir = toCam / dist;

            if (Physics.SphereCast(origin, radius, dir, out RaycastHit hit, dist, obstacleLayers))
            {
                float newDist = Mathf.Max(0f, hit.distance - wallBuffer);
                transform.position = origin + dir * newDist;
            }
        }
    }
}
