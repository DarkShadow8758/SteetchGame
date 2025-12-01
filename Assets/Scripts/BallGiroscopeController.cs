#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallGiroscopeController : MonoBehaviour
{
    public float speed = 12f;
    public float Deadzone = 0.012f;
    public float sleepVel = 0.02f;
    public Vector3 rotFixEuler = new Vector3(90, 0, 0);
    [Tooltip("Testar com acelerometros")] 
    public bool useAccelerometer = true;
    [Tooltip("Debug com teclado para o editor, ja que nn tem giros no pc")]
    public bool simulateInEditor = true;
    [Tooltip("Inverte Y se necessario")] public bool invertY = false;
    [Tooltip("Inverte X se necessario")] public bool invertX = false;
    public bool AutocalibrationOnStart = true;

    Rigidbody2D rb;
    Quaternion calib = Quaternion.identity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
        if (!useAccelerometer)
        {
            Input.gyro.enabled = true;
            if (AutocalibrationOnStart)
                calib = GetWorldAttitude();
        }
    }
    


    Quaternion GetWorldAttitude() 
    {
        var g = Input.gyro.attitude;
        var q = new Quaternion(g.x, g.y, -g.z, -g.w);
        return Quaternion.Euler(rotFixEuler) * q;
    }

    void ZeroMotion()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void FixedUpdate()
    {
        Vector2 dir2D = Vector2.zero;

        #if UNITY_EDITOR
        if (simulateInEditor)
        {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            float ex = 0f;
            float ey = 0f;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) ex -= 1f;
                if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) ex += 1f;
                if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) ey += 1f;
                if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) ey -= 1f;
            }
            dir2D = new Vector2(ex, ey);
#else
            float ex = Input.GetAxisRaw("Horizontal");
            float ey = Input.GetAxisRaw("Vertical");
            dir2D = new Vector2(ex, ey);
#endif
        }
        #endif

        if (useAccelerometer && dir2D == Vector2.zero)
        {
            Vector3 acc = Input.acceleration; 
            dir2D = new Vector2(acc.x, acc.y);
        }
       if (!useAccelerometer && dir2D == Vector2.zero)
        {
            Quaternion worldRot = GetWorldAttitude();
            Quaternion rel = Quaternion.Inverse(calib) * worldRot;
            Vector3 fwd = rel * Vector3.forward;
            dir2D = new Vector2(fwd.x, fwd.z);
        }

        dir2D.x = invertX ? -dir2D.x : dir2D.x;
        dir2D.y = invertY ? -dir2D.y : dir2D.y;

        if (dir2D.magnitude < Deadzone)
        {
            if (rb.linearVelocity.magnitude < sleepVel && Mathf.Abs(rb.angularVelocity) < sleepVel)
            {
                rb.Sleep();
            }
            return;
        }
        rb.WakeUp();
        rb.AddForce(dir2D.normalized * dir2D.magnitude * speed, ForceMode2D.Force);
    }
}
