using UnityEngine;

public class BounceSurface : MonoBehaviour
{
    [Tooltip("ƨ�� �� ���� �������� ��")]
    public float bounceForce = 10f;

    public float GetBounceForce()
    {
        return bounceForce;
    }
}
