using UnityEngine;

public static class GameplayUtils{

    public static Vector3 GetDirCam(Transform camTransform, float v, float h){
        Vector3 camFoward = Vector3.Scale(camTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 move = v*camFoward + h* camTransform.right;
        return move;
	}

    public static bool Rotate360(Transform player, float delayRotate, ref float timeRotate, ref float curAngleX, ref Vector3 lastFwd,ref float sign) {
        if (Time.time - timeRotate > delayRotate) {
            curAngleX = 0;
            timeRotate = Time.time;
            return false;
        }
        Vector3 curFwd = player.forward;
        // measure the angle rotated since last frame:
        float ang = Vector3.Angle(curFwd, lastFwd);
        if (ang > 0.01) { // if rotated a significant angle...
            if (Vector3.Cross(curFwd, lastFwd).y < 0) {
                ang = -1 * ang;
                sign = -1;
            } else sign = 1;
            curAngleX += ang; // accumulate in curAngleX...
            lastFwd = curFwd; // and update lastFwd
        }
        return Mathf.Abs(curAngleX) >= 360;
    }

    public static void TurnRotation(Vector3 move, Transform transform, float rotationSpeed){
        if(move.magnitude <=0f || ((transform.position+move) - transform.position) == Vector3.zero) return;
		Quaternion targetRot = Quaternion.LookRotation((transform.position+move) - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, move.magnitude * rotationSpeed*Time.deltaTime);
    }

    public static void Move(Vector3 move, Rigidbody rigd, float velMoviment = 1, Animator animator = null, string param = "Vel"){
        if (move.magnitude > 1f)
            move.Normalize();
        animator?.SetFloat(param, move.magnitude);
        rigd.velocity = move*velMoviment;
    }

    public static void MoveFoward(Vector3 move, Rigidbody rigd, float velMoviment = 1, Animator animator = null, string param = "Vel"){
        if (move.magnitude > 1f)
            move.Normalize();
        animator?.SetFloat(param, move.magnitude);
        rigd.velocity = rigd.transform.forward * move.magnitude *velMoviment;
    }
}

public static class GameObjectExtensions  {

    public static bool ContainsLayer(this LayerMask layermask, GameObject gameObject) {
        return (1 << gameObject.layer | layermask) == layermask;
    }

    public static GameObject InstantiateTransform(this Transform transform, GameObject original) {
        return MonoBehaviour.Instantiate(original, transform.position, transform.rotation);
    }
}