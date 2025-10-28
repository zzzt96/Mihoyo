using UnityEngine;
using System.Collections;

public class FireflyController : MonoBehaviour
{
    [Header("���п���")]
    public float moveSpeed = 10f;
    public float rotationSpeed = 2f;
    public float inertia = 0.9f;  // ����ϵ��
    public float brakeFactor = 0.5f;

    [Header("��̲���")]
    public float dashMultiplier = 2f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 3f;
    private bool canDash = true;
    private bool isDashing = false;

    [Header("��ײ����")]
    public float bounceDistance = 2f;
    public float bounceDuration = 0.5f;
    private bool isBouncing = false;

    private Vector3 currentVelocity = Vector3.zero;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isBouncing) return; // ��ײ���޷�����

        HandleRotation();
        HandleMovement();
        HandleDash();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // ˮƽ��ת��������Y�ᣩ
        transform.Rotate(Vector3.up * mouseX, Space.World);

        // ���ƴ�ֱ��ת
        Vector3 currentEuler = transform.localEulerAngles;

        float currentPitch = currentEuler.x;
        if (currentPitch > 180) currentPitch -= 360;
        
        currentPitch -= mouseY; 

        currentPitch = Mathf.Clamp(currentPitch, -45f, 45f);

        currentEuler.x = currentPitch;
        transform.localEulerAngles = new Vector3(currentPitch, transform.localEulerAngles.y, 0f);
    }

    void HandleMovement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.S))
        {
            currentVelocity *= brakeFactor;
        }
        else
        {
            Vector3 targetVelocity = transform.TransformDirection(input.normalized) * moveSpeed;
            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * (isDashing ? 8f : 3f));
        }

        transform.position += currentVelocity * Time.deltaTime;
        currentVelocity *= inertia; // ģ��Ư������
    }

    void HandleDash()
    {
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        float originalSpeed = moveSpeed;
        moveSpeed *= dashMultiplier;

        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalSpeed;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isBouncing) return;

        StartCoroutine(BounceBackCoroutine(collision.contacts[0].normal));
    }

    IEnumerator BounceBackCoroutine(Vector3 hitNormal)
    {
        isBouncing = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + hitNormal * bounceDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / bounceDuration;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        currentVelocity = Vector3.zero;
        isBouncing = false;
    }
}
