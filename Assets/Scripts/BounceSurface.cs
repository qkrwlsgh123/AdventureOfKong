using UnityEngine;

public class BounceSurface : MonoBehaviour
{
    [Tooltip("튕길 때 위로 가해지는 힘")]
    public float bounceForce = 10f;

    public float GetBounceForce()
    {
        return bounceForce;
    }
}
