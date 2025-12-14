using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //Variables
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothSpeed = 5f;
    private Transform target;

    //sets the transform to target the set object, player in this case
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    //lateupdate to make sure the player has moved before the camera moves with the player - did this as the camera was moving first and it looked strange
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}