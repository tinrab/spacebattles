using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Gravity : MonoBehaviour
{
  private static List<Collider> s_Pieces = new List<Collider>();
  [SerializeField] private float m_Multiplier;
  private Rigidbody m_RigidBody;
  private GravitationalParticle m_Particle;

  public Vector3 direction
  {
    get
    {
      return m_Particle.direction;
    }
  }

  public float multiplier
  {
    get
    {
      return m_Multiplier;
    }
    set
    {
      m_Multiplier = value;
    }
  }

  private void Start()
  {
    m_RigidBody = GetComponent<Rigidbody>();
  }

  private void Update()
  {
    CalculateParticle(transform.position, ref m_Particle);

    if (transform.parent != m_Particle.collider.transform) {
      transform.parent = m_Particle.collider.transform;
    }
  }

  private void FixedUpdate()
  {
    m_RigidBody.AddForce(m_Particle.direction * m_Multiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
  }

  public static void AddPiece(Piece piece)
  {
    s_Pieces.Add(piece.GetComponent<Collider>());
  }

  private static void CalculateParticle(Vector3 position, ref GravitationalParticle particle)
  {
    var distance = float.PositiveInfinity;

    for (var i = 0; i < s_Pieces.Count; i++) {
      var collider = s_Pieces[i];
      var type = collider.GetType();
      var point = default(Vector3);

      if (type == typeof (BoxCollider)) {
        var box = (BoxCollider) collider;

        point = box.ClosestPointToLocalPoint(box.transform.InverseTransformPoint(position));
        point = box.transform.TransformPoint(point);
      } else if (type == typeof (SphereCollider)) {
        var sphere = (SphereCollider) collider;
        var normal = (position - sphere.transform.position).normalized;

        normal.Scale(sphere.transform.lossyScale);

        point = sphere.transform.position + normal * sphere.radius;
      } else if (type == typeof (MeshCollider)) {
        var mesh = ((MeshCollider) collider).sharedMesh;
        var verts = mesh.vertices;
        var tris = mesh.triangles;
        var minDistance = float.PositiveInfinity;

        var pos = collider.transform.InverseTransformPoint(position);

        for (var j = 0; j < tris.Length; j += 3) {
          var closestToTriangle = Utility.ClosestPointOnTriangle(verts[tris[j]], verts[tris[j + 1]], verts[tris[j + 2]], pos);
          var dst = (pos - closestToTriangle).sqrMagnitude;

          if (dst < minDistance) {
            minDistance = dst;
            point = closestToTriangle;
          }
        }

        point = collider.transform.TransformPoint(point);
      }

      var d = (position - point).sqrMagnitude;

      if (d < distance) {
        distance = d;
        particle.direction = point - position;
        particle.collider = collider;
      }
    }
  }

  private struct GravitationalParticle
  {
    public Vector3 direction { get; set; }
    public Collider collider { get; set; }
  }
}