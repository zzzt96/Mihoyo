using UnityEngine;

public class FireflyCameraFollow : MonoBehaviour
{
    [Header("Ŀ����ƫ��")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 1.5f, -4f);

    [Header("ƽ������")]
    public float positionSmoothness = 5f;   // ����������ƽ���̶�
    public float rotationSmoothness = 8f;   // ���������ƽ���̶�

    [Header("Ư�������")]
    public float swayAmount = 0.3f;         // ҡ�ڷ���
    public float swaySpeed = 2f;            // ҡ��Ƶ��
    public float followLag = 0.3f;          // ��ת�����ӳ�

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

        //����Ŀ�귽��Ĺ����ӳ� ----
        // �������������΢�ͺ���Ŀ�귽�����조����С�
        Vector3 targetForward = Vector3.Lerp(lastTargetForward, target.forward, Time.deltaTime / followLag);
        lastTargetForward = targetForward;

        //�����������λ�� ----
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        //�����΢ҡ�ڣ���������΢����----
        Vector3 swayOffset = new Vector3(
            Mathf.Sin(swayTimer) * swayAmount * 0.3f,
            Mathf.Sin(swayTimer * 0.7f) * swayAmount * 0.5f,
            Mathf.Cos(swayTimer * 0.5f) * swayAmount * 0.2f
        );
        desiredPosition += swayOffset;

        //ƽ���ƶ������ ----
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, 1f / positionSmoothness);

        //ƽ����ת��ʹ�俴��Ŀ�� ----
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothness);
    }
}
