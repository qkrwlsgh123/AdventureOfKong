using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class TreeBlockGrow : MonoBehaviour
{
    [Header("세그먼트 설정")]
    [Tooltip("자라날 나무 블록 조각 Prefab")]
    public GameObject segmentPrefab;
    [Tooltip("블록 하나 높이(유닛)")]
    public float segmentHeight = 1f;

    [Header("잎블록 설정")]
    [Tooltip("장식용 잎블록 Prefab (Collider 없이 통과 가능)")]
    public GameObject foliagePrefab;

    [Header("성장 주기 (초)")]
    [Tooltip("물블록에 닿은 뒤 몇 초마다 자랄지")]
    public float growInterval = 5f;

    // 내부 상태
    private int waterTouchCount = 0;
    private Coroutine growRoutine;
    private List<GameObject> segments = new List<GameObject>();
    private GameObject foliageInstance;

    void Awake()
    {
        // Rigidbody2D를 Kinematic으로 세팅 (Trigger + Kinematic 조합으로 OnTrigger 호출)
        var rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        // 씬 시작 시 잎블록(Decoration) 한 번 생성
        if (foliagePrefab != null)
        {
            Vector3 startPos = transform.position + Vector3.up * segmentHeight;
            foliageInstance = Instantiate(
                foliagePrefab,
                startPos,
                Quaternion.identity,
                transform.parent
            );
        }
    }

    // Trigger Enter: 물블록(Trigger Collider)과 닿으면 성장 시작
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterTouchCount++;
            if (growRoutine == null)
                growRoutine = StartCoroutine(GrowLoop());
        }
    }

    // Trigger Exit: 물블록에서 떨어지면 성장 멈춤
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("물블록"))
        {
            waterTouchCount = Mathf.Max(0, waterTouchCount - 1);
            if (waterTouchCount == 0 && growRoutine != null)
            {
                StopCoroutine(growRoutine);
                growRoutine = null;
            }
        }
    }

    // 매 growInterval 초마다 새 조각과 잎블록 이동, 플레이어 처리
    IEnumerator GrowLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(growInterval);
            SpawnSegmentAndAdjustPlayer();
        }
    }

    // 새 세그먼트 생성, 잎블록 이동, 플레이어(콩) 끼임 방지
    void SpawnSegmentAndAdjustPlayer()
    {
        // 1) 새로운 나무 조각 생성
        int nextIndex = segments.Count + 1;
        Vector3 segPos = transform.position + Vector3.up * (segmentHeight * nextIndex);
        GameObject seg = Instantiate(
            segmentPrefab,
            segPos,
            Quaternion.identity,
            transform.parent
        );
        segments.Add(seg);

        // 2) 잎블록(장식) 최상단으로 이동
        if (foliageInstance != null)
        {
            Vector3 leafPos = transform.position + Vector3.up * (segmentHeight * (segments.Count + 1));
            foliageInstance.transform.position = leafPos;
        }

        // 3) 플레이어(콩)가 새 블록 사이에 끼면 위로 밀어 올리기
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Collider2D pCol = player.GetComponent<Collider2D>();
            Collider2D sCol = seg.GetComponent<Collider2D>();
            if (pCol != null && sCol != null && pCol.IsTouching(sCol))
            {
                // 새 블록의 상단 y 좌표
                float topY = sCol.bounds.max.y;
                // 콩 콜라이더 절반 높이
                float halfH = pCol.bounds.extents.y;
                // 콩을 블록 바로 위로 이동
                Vector3 newPos = new Vector3(
                    player.transform.position.x,
                    topY + halfH + 0.01f,
                    player.transform.position.z
                );
                player.transform.position = newPos;

                // 속도 초기화
                var prb = player.GetComponent<Rigidbody2D>();
                if (prb != null)
                    prb.linearVelocity = new Vector2(prb.linearVelocity.x, 0f);
            }
        }
    }
}
