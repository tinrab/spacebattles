using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof (CapsuleCollider))]
public class GravityCharacterController : MonoBehaviour
{
  // distance for checking if the controller is grounded
  private static readonly float s_GroundCheckDistance = 0.05f;
  // stops the character
  private static readonly float s_StickToGroundHelperDistance = 0.6f;
  // rate at which the controller comes to a stop when there is no input
  private static readonly float s_SlowDownRate = 40.0f;
  //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
  private static readonly float s_ShellOffset = 0.0f;
  private static float s_RotationSpeed = 5.0f;
  private CapsuleCollider m_CapsuleCollider;
  private float m_CurrentTargetSpeed;
  [SerializeField] private float m_Drag;
  private Vector3 m_GroundContactNormal;
  private Vector2 m_InputDirection;
  private bool m_Jump, m_Jumping;
  [SerializeField] private float m_JumpPower;
  private bool m_PreviouslyGrounded;
  private Rigidbody m_RigidBody;

  public AnimationCurve m_SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f),
    new Keyframe(90.0f, 0.0f));

  [Header("Properties")] [SerializeField] private float m_Speed;
  private Quaternion m_TargetRotation;
  [SerializeField] private Transform m_View;
  public bool grounded { get; private set; }

  public bool isRunning
  {
    get
    {
      return false;
    }
  }

  public Vector3 velocity
  {
    get
    {
      return m_RigidBody.velocity;
    }
  }

  private void Awake()
  {
    m_RigidBody = GetComponent<Rigidbody>();
    m_CapsuleCollider = GetComponent<CapsuleCollider>();
  }

  private void Start()
  {
    m_TargetRotation = transform.rotation;
  }

  public void Jump()
  {
    if (!m_Jump) {
      m_Jump = true;
    }
  }

  public void Move(Vector2 direction)
  {
    m_InputDirection = direction;
  }

  private void FixedUpdate()
  {
    GroundCheck();

    UpdateDesiredTargetSpeed(m_InputDirection);

    if (Mathf.Abs(m_InputDirection.x) > float.Epsilon || Mathf.Abs(m_InputDirection.y) > float.Epsilon) {
      var desiredMove = m_View.transform.forward * m_InputDirection.y + m_View.transform.right * m_InputDirection.x;
      desiredMove = Vector3.ProjectOnPlane(desiredMove, Physics.gravity.normalized).normalized;

      desiredMove.x = desiredMove.x * m_CurrentTargetSpeed;
      desiredMove.z = desiredMove.z * m_CurrentTargetSpeed;
      desiredMove.y = desiredMove.y * m_CurrentTargetSpeed;

      if (m_RigidBody.velocity.sqrMagnitude < m_CurrentTargetSpeed * m_CurrentTargetSpeed) {
        m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
      }
    }

    if (grounded) {
      m_RigidBody.drag = m_Drag;

      if (m_Jump) {
        m_RigidBody.drag = 0.0f;
        //m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0.0f, m_RigidBody.velocity.z);
        m_RigidBody.AddForce(-Physics.gravity.normalized * m_JumpPower * 50.0f, ForceMode.Impulse);
        m_Jumping = true;
      }

      if (!m_Jumping && Mathf.Abs(m_InputDirection.x) < float.Epsilon && Mathf.Abs(m_InputDirection.y) < float.Epsilon &&
          m_RigidBody.velocity.magnitude < 1.0f) {
        m_RigidBody.Sleep();
      }
    } else {
      m_RigidBody.drag = 0.0f;

      if (m_PreviouslyGrounded && !m_Jumping) {
        StickToGroundHelper();
      }
    }

    m_Jump = false;
    m_InputDirection = Vector2.zero;
  }

  private float SlopeMultiplier()
  {
    var angle = Vector3.Angle(m_GroundContactNormal, -Physics.gravity.normalized);

    return m_SlopeCurveModifier.Evaluate(angle);
  }

  private void StickToGroundHelper()
  {
    RaycastHit hitInfo;
    if (Physics.SphereCast(transform.position, m_CapsuleCollider.radius * (1.0f - s_ShellOffset),
      Physics.gravity.normalized, out hitInfo,
      m_CapsuleCollider.height / 2f - m_CapsuleCollider.radius + s_StickToGroundHelperDistance, ~0,
      QueryTriggerInteraction.Ignore)) {
      if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f) {
        m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
      }
    }
  }

  private void GroundCheck()
  {
    m_PreviouslyGrounded = grounded;
    RaycastHit hitInfo;
    if (Physics.SphereCast(transform.position, m_CapsuleCollider.radius * (1.0f - s_ShellOffset),
      Physics.gravity.normalized, out hitInfo,
      m_CapsuleCollider.height / 2.0f - m_CapsuleCollider.radius + s_GroundCheckDistance, ~0,
      QueryTriggerInteraction.Ignore)) {
      grounded = true;
      m_GroundContactNormal = hitInfo.normal;
    } else {
      grounded = false;
      m_GroundContactNormal = Vector3.up;
    }
    if (!m_PreviouslyGrounded && grounded && m_Jumping) {
      m_Jumping = false;
    }
  }

  private void UpdateDesiredTargetSpeed(Vector2 input)
  {
    if (input == Vector2.zero) {
      return;
    }

    if (input.x > 0.0f || input.x < 0.0f) {
      // strafe
      m_CurrentTargetSpeed = m_Speed * 4.0f;
    }

    if (input.y < 0.0f) {
      // backwards
      m_CurrentTargetSpeed = m_Speed * 4.0f;
    }

    if (input.y > 0.0f) {
      // forwards
      m_CurrentTargetSpeed = m_Speed * 8.0f;
    }
  }
}