using UnityEngine;

public class TreeBlockGrower : MonoBehaviour
{
    public GameObject treeSegmentPrefab; // �ڶ󳪴� ���� ���׸�Ʈ
    public GameObject leafPrefab;        // �� prefab
    public float growInterval = 5f;      // ���� ����

    private float timer = 0f;
    private bool hasStarted = false;
    private Transform currentLeaf;
    private int height = 1;

    private const float BlockSize = 0.75f; // ��� ���� ����

    void Start()
    {
        if (IsNextToWater())
        {
            hasStarted = true;
            timer = growInterval;

            // �ʱ� �� ���� (���� ������� ��)
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
        Instantiate(treeSegmentPrefab, newSegmentPos, Quaternion.identity);

        height++;

        if (currentLeaf != null)
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
            if (hit != null && hit.CompareTag("�����"))
                return true;
        }
        return false;
    }
}
