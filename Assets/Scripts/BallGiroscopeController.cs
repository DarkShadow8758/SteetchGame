using UnityEngine;

public class BallGiroscopeController : MonoBehaviour
{
    public float speed = 12f;
    public float Deadzone = 0.012f;
    public float sleepVel = 0.02f;
    public Vector3 rotFixEuler = new Vector3 (90, 0 , 0);
    public bool AutocalibrationOnStart = true;

    Rigidbody rb;
    Quaternion calib = Quaternion.identity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Quaternion worldRot = GetWorldAttitude();
        Quaternion rel = Quaternion.Inverse(calib) * worldRot;

        Vector3 fwd = rel * Vector3.forward;
        Vector3 dir = new Vector3(fwd.x, 0f, fwd.z);

        if (dir.magnitude < Deadzone)
        {
            if (rb.linearVelocity.magnitude < sleepVel && rb.angularVelocity.magnitude < sleepVel)
            {
                rb.Sleep();
               
            }
            return;

          
        }
        rb.WakeUp();
        rb.AddForce(dir.normalized * dir.magnitude * speed, ForceMode.Acceleration);
    }
}
