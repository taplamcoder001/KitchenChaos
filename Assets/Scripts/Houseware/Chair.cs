using UnityEngine;

public class Chair : MonoBehaviour
{
    [SerializeField] private Transform topPointTransform;
    public Transform TopPointTranform => topPointTransform;
}
