using UnityEngine;
using UnityEngine.InputSystem;
public class DeviceOperator : MonoBehaviour {
    public Transform actionCenter;
    public Vector3 actionVolumeSize = new Vector3(1, 1.5f, 0.5f);
    public void OnOperate(InputAction.CallbackContext context) {
        if (context.performed) {
            var vertRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            Collider[] hitColliders = Physics.OverlapBox(
            actionCenter.position, actionVolumeSize/2, vertRotation);
            foreach (Collider hitCollider in hitColliders) {
                hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
    private void OnDrawGizmos() { // prueba también OnDrawGizmosSelected
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(actionCenter.position,
        Quaternion.Euler(0, transform.eulerAngles.y, 0), actionVolumeSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = Matrix4x4.identity;
    }
}