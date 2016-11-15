using UnityEngine;

[RequireComponent(typeof(GravityCharacterController))]
public class PlayerInput : MonoBehaviour
{
  private GravityCharacterController m_GravityCharacterController;
  [SerializeField]
  private Transform m_View;

  private Vector2 m_InputDirection;

  private void Awake()
  {
    m_GravityCharacterController = GetComponent<GravityCharacterController>();
  }

  private void Update()
  {
    if (Input.GetButtonDown("Jump")) {
      m_GravityCharacterController.Jump();
    }
  }

  private void FixedUpdate()
  {
    m_InputDirection.x = Input.GetAxis("Horizontal");
    m_InputDirection.y = Input.GetAxis("Vertical");

    m_GravityCharacterController.Move(m_InputDirection);
  }
}