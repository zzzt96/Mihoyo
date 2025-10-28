using UnityEngine;

public class FireflyCameraFollow : MonoBehaviour
{
    [Header("目标与偏移")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, -4f);

    [Header("平滑参数")]
    public float positionSmoothness = 5f;   // 摄像机跟随的平滑程度
    public float rotationSmoothness = 8f;   // 摄像机朝向平滑程度

    [Header("漂浮与惯性")]
    public float swayAmount = 0.3f;         // 摇摆幅度
    public float swaySpeed = 2f;            // 摇摆频率
    public float followLag = 0.3f;          // 旋转惯性延迟

    private Vector3 currentVelocity;
    private Vector3 smoothedVelocity;
    private Vector3 lastTargetForward;
    private float swayTimer = 0f;

    void Start()
    {
        if (target)
            lastTargetForward = target.forward;
    }

    void LateUpdate()
    {
        if (!target) return;

        swayTimer += Time.deltaTime * swaySpeed;

        //计算目标方向的惯性延迟 ----
        // 让摄像机方向稍微滞后于目标方向，制造“跟随感”
        Vector3 targetForward = Vector3.Lerp(lastTargetForward, target.forward, Time.deltaTime / followLag);
        lastTargetForward = targetForward;

        //计算基础跟随位置 ----
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        //添加轻微摇摆（上下左右微动）----
        Vector3 swayOffset = new Vector3(
            Mathf.Sin(swayTimer) * swayAmount * 0.3f,
            Mathf.Sin(swayTimer * 0.7f) * swayAmount * 0.5f,
            Mathf.Cos(swayTimer * 0.5f) * swayAmount * 0.2f
        );
        desiredPosition += swayOffset;

        //平滑移动摄像机 ----
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / positionSmoothness);

        //平滑旋转，使其看向目标 ----
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothness);
    }
}
