using UnityEngine;

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

            CreateLeafAbove(height);  // 초기 잎 생성
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

        // 위에 BounceBlock이 있다면 중단
        Collider2D hit = Physics2D.OverlapCircle(newSegmentPos, 0.1f);
        if (hit != null && hit.CompareTag("BounceBlock"))
        {
            hasStarted = false;
            return;
        }

        // 나무 세그먼트 생성
        Instantiate(treeSegmentPrefab, newSegmentPos, Quaternion.identity);
        height++;

        // 잎 처리
        CreateLeafAbove(height);
    }

    void CreateLeafAbove(int h)
    {
        Vector2 leafPos = (Vector2)transform.position + Vector2.up * h * BlockSize;

        // 이미 존재하는 잎이 있다면 위치만 옮기기
        if (currentLeaf != null)
        {
            currentLeaf.position = leafPos;
        }
        else if (leafPrefab != null && Physics2D.OverlapCircle(leafPos, 0.1f) == null)
        {
            GameObject leaf = Instantiate(leafPrefab, leafPos, Quaternion.identity);
            currentLeaf = leaf.transform;
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
