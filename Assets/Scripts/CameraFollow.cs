using UnityEngine;

public class CameraFollowCenterTriggerWithBounds : MonoBehaviour
{
    public Transform target;            // 따라갈 콩
    public float smoothSpeed = 5f;      // 부드럽게 따라오는 속도
    public float minX = 0f;             // 카메라 이동 최소 X (맵 왼쪽 경계)
    public float maxX = 100f;           // 카메라 이동 최대 X (맵 오른쪽 경계)

    private float fixedY;               // 상하는 고정
    private bool hasStartedFollowing = false;
    private float startCamX;            // 처음 카메라 중심선

    void Start()
    {
        fixedY = transform.position.y;
        startCamX = transform.position.x; // 처음 카메라 중심선
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (!hasStartedFollowing)
        {
            // ✅ 카메라 중심선을 넘어갔을 때 추적 시작
            if (target.position.x > startCamX)
                hasStartedFollowing = true;
        }

        if (hasStartedFollowing)
        {
            Vector3 camPos = transform.position;
            Vector3 desiredPos = new Vector3(target.position.x, fixedY, camPos.z);

            // 📍 Lerp로 부드럽게 이동
            Vector3 newPos = Vector3.Lerp(camPos, desiredPos, smoothSpeed * Time.deltaTime);

            // ✅ 카메라 이동 제한 (Clamp)
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

            transform.position = newPos;
        }
    }
}
