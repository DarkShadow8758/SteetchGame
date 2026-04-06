using UnityEngine;

public class CameraTopDown : MonoBehaviour
{
  public Transform target;
    public float height = 20f;
    public float smooth = 5f;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
        offset.y = height;
    }

    void LateUpdate()
    {
        if (!target)return;

        Vector3 desiredPos = target.position + offset;
        desiredPos.y = height;
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smooth);

    }
}
