using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class TreeBlockGrow : MonoBehaviour
{
    [Header("���׸�Ʈ ����")]
    public GameObject segmentPrefab;
    public float segmentHeight = 1f;

    [Header("�ٺ�� ����")]
    public GameObject foliagePrefab;

    [Header("���� �ֱ� (��)")]
    public float growInterval = 5f;

    int waterCount = 0;
    Coroutine growRoutine;
    List<GameObject> segments = new List<GameObject>();
    GameObject foliageInstance;

    void Awake()
    {
        // �� ���� �� �ٺ�� �� �� ����
        if (foliagePrefab != null)
        {
            Vector3 pos = transform.position + Vector3.up * segmentHeight;
            foliageInstance = Instantiate(foliagePrefab, pos, Quaternion.identity, transform.parent);
        }
    }

    // ���� �浹�� �� ���� ����
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("�����"))
        {
            waterCount++;
            if (growRoutine == null)
                growRoutine = StartCoroutine(GrowLoop());
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("�����"))
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
        // ���� ����
        float y = segmentHeight * (segments.Count + 1);
        Vector3 p = transform.position + Vector3.up * y;
        var seg = Instantiate(segmentPrefab, p, Quaternion.identity, transform.parent);
        segments.Add(seg);

        // �ٺ�� �ֻ������ �ø���
        if (foliageInstance != null)
        {
            Vector3 leafPos = transform.position + Vector3.up * (segments.Count + 1) * segmentHeight;
            foliageInstance.transform.position = leafPos;
        }
    }
}
