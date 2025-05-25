using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class TreeBlockGrow : MonoBehaviour
{
    [Header("세그먼트 설정")]
    public GameObject segmentPrefab;
    public float segmentHeight = 1f;

    [Header("잎블록 설정")]
    public GameObject foliagePrefab;

    [Header("성장 주기 (초)")]
    public float growInterval = 5f;

    int waterCount = 0;
    Coroutine growRoutine;
    List<GameObject> segments = new List<GameObject>();
    GameObject foliageInstance;

    void Awake()
    {
        // 씬 시작 시 잎블록 한 번 생성
        if (foliagePrefab != null)
        {
            Vector3 pos = transform.position + Vector3.up * segmentHeight;
            foliageInstance = Instantiate(foliagePrefab, pos, Quaternion.identity, transform.parent);
        }
    }

    // 경계면 충돌로 물 닿음 감지
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("물블록"))
        {
            waterCount++;
            if (growRoutine == null)
                growRoutine = StartCoroutine(GrowLoop());
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("물블록"))
        {
            waterCount = Mathf.Max(0, waterCount - 1);
            if (waterCount == 0 && growRoutine != null)
            {
                StopCoroutine(growRoutine);
                growRoutine = null;
            }
        }
    }

    IEnumerator GrowLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(growInterval);
            SpawnSegment();
        }
    }

    void SpawnSegment()
    {
        // 나무 조각
        float y = segmentHeight * (segments.Count + 1);
        Vector3 p = transform.position + Vector3.up * y;
        var seg = Instantiate(segmentPrefab, p, Quaternion.identity, transform.parent);
        segments.Add(seg);

        // 잎블록 최상단으로 올리기
        if (foliageInstance != null)
        {
            Vector3 leafPos = transform.position + Vector3.up * (segments.Count + 1) * segmentHeight;
            foliageInstance.transform.position = leafPos;
        }
    }
}
