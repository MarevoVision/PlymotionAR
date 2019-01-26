using UnityEngine;

public class RotateController : MonoBehaviour
{
    private Transform target;
    public float speedRotateX = 5;
    void Start()
    {
        target = GetComponent<Transform>();
    }

    void Update()
    {
        if (!Input.GetMouseButton(1))
            return;

        float rotX = Input.GetAxis("Horizontal") * speedRotateX * Mathf.Deg2Rad;
        target.Rotate(target.up, -rotX);
    }
}