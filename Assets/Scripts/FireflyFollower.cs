using UnityEngine;

public class FireflyFollower : MonoBehaviour
{
    [Header("��������")]
    public Transform target;           // �����Ŀ�꣨��ө����ǰһ����ӣ�
    public float followDistance = 1.0f; // ������
    public float followSmooth = 3f;     // ����ƽ����

    [Header("Ư��Ч��")]
    public float floatAmplitude = 0.2f;
    public float floatFrequency = 2f;
    private float floatOffset;

    private Vector3 velocity;
    private Vector3 desiredPosition;

    void Start()
    {
        floatOffset = Random.Range(0f, 100f); // ��ÿ����ӵ�Ư����ͬ��
    }

    void Update()
    {
        if (target == null) return;

        //����Ŀ��λ�� ----
        Vector3 targetPos = target.position - target.forward * followDistance;

        //Ư��ƫ�� ----
        float yOffset = Mathf.Sin(Time.time * floatFrequency + floatOffset) * floatAmplitude;
        targetPos.y += yOffset;

        //ƽ���ƶ� ----
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1f / followSmooth);

        //����Ŀ�� ----
        Vector3 dir = (target.position - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), Time.deltaTime * followSmooth);
    }
}
