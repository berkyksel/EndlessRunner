using UnityEngine;

public class RotationMovementComp : MonoBehaviour
{
    [SerializeField] float RotationSpeed = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
    }
}
