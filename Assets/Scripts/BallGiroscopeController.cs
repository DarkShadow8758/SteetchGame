using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallGiroscopeController : MonoBehaviour
{
    public float speed = 12f;
    public float Deadzone = 0.012f;
    public float sleepVel = 0.02f;
    public Vector3 rotFixEuler = new Vector3 (90, 0 , 0);
    public bool AutocalibrationOnStart = true;

    Rigidbody2D rb;
    Quaternion calib = Quaternion.identity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
        Input.gyro.enabled = true;

        if (AutocalibrationOnStart)
            calib = GetWorldAttitude();
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
        Quaternion worldRot = GetWorldAttitude();
        Quaternion rel = Quaternion.Inverse(calib) * worldRot;

        Vector3 fwd = rel * Vector3.forward;
        Vector2 dir2D = new Vector2(fwd.x, fwd.z);

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
