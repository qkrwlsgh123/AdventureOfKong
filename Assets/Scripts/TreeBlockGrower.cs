﻿using UnityEngine;

public class TreeBlockGrower : MonoBehaviour
{
    public GameObject treeSegmentPrefab; // 자라나는 나무 세그먼트
    public GameObject leafPrefab;        // 잎 prefab
    public float growInterval = 5f;      // 성장 간격

    private float timer = 0f;
    private bool hasStarted = false;
    private Transform currentLeaf;
    private int height = 1;

    private const float BlockSize = 0.75f; // 블록 간격 단위

    void Start()
    {
        if (IsNextToWater())
        {
            hasStarted = true;
            timer = growInterval;

            // 초기 잎 생성 (위에 비어있을 때)
            Vector2 top = (Vector2)transform.position + Vector2.up * BlockSize;
            if (leafPrefab != null && Physics2D.OverlapCircle(top, 0.1f) == null)
            {
                GameObject leaf = Instantiate(leafPrefab, top, Quaternion.identity);
                currentLeaf = leaf.transform;
            }
        }
    }

    void Update()
    {
        if (!hasStarted) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = growInterval;
            GrowTreeSegment();
        }
    }

    void GrowTreeSegment()
    {
        Vector2 newSegmentPos = (Vector2)transform.position + Vector2.up * height * BlockSize;

        // 성장 위치에 이미 다른 블록이 있는지 검사 (단, 잎 또는 Player는 예외)
        Collider2D[] hits = Physics2D.OverlapCircleAll(newSegmentPos, 0.1f);
        foreach (var hit in hits)
        {
            if (hit != null && hit.tag != "Leaf" && hit.tag != "Player")
            {
                hasStarted = false;
                return;
            }
        }

        Instantiate(treeSegmentPrefab, newSegmentPos, Quaternion.identity);
        height++;

        if (currentLeaf != null && hasStarted)
        {
            currentLeaf.position = (Vector2)transform.position + Vector2.up * height * BlockSize;
        }
    }

    bool IsNextToWater()
    {
        Vector2[] directions = new Vector2[] { Vector2.left, Vector2.right };
        foreach (Vector2 dir in directions)
        {
            Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + dir, 0.1f);
            if (hit != null && hit.CompareTag("물블록"))
                return true;
        }
        return false;
    }
}
