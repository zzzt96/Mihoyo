using UnityEngine;

public class FireflyFollower : MonoBehaviour
{
    [Header("跟随设置")]
    public Transform target;           // 跟随的目标（主萤火虫或前一个随从）
    public float followDistance = 1.0f; // 跟随间距
    public float followSmooth = 3f;     // 跟随平滑度

    [Header("漂浮效果")]
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 2f;
    private float floatOffset;

    private Vector3 velocity;
    private Vector3 desiredPosition;

    void Start()
    {
        floatOffset = Random.Range(0f, 100f); // 让每个随从的漂浮不同步
    }

    void Update()
    {
        if (target == null) return;

        //基础目标位置 ----
        Vector3 targetPos = target.position - target.forward * followDistance;

        //漂浮偏移 ----
        float yOffset = Mathf.Sin(Time.time * floatFrequency + floatOffset) * floatAmplitude;
        targetPos.y += yOffset;

        //平滑移动 ----
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1f / followSmooth);

        //朝向目标 ----
        Vector3 dir = (target.position - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), Time.deltaTime * followSmooth);
    }
}
