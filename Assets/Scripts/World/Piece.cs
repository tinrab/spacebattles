using UnityEngine;

[RequireComponent(typeof (Collider))]
public class Piece : MonoBehaviour
{
  private void Awake()
  {
    Gravity.AddPiece(this);
  }
}