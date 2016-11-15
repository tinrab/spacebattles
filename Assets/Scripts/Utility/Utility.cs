using UnityEngine;

public static class Utility
{
  public static void SetLayerRecursively(this GameObject gameObject, int layer)
  {
    gameObject.layer = layer;
    var t = gameObject.transform;

    for (var i = 0; i < t.childCount; i++) {
      t.GetChild(i).gameObject.SetLayerRecursively(layer);
    }
  }

  // Credit: http://www.gamedev.net/topic/552906-closest-point-on-triangle/
  public static Vector3 ClosestPointOnTriangle(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 point)
  {
    var edge0 = p2 - p1;
    var edge1 = p3 - p1;
    var v0 = p1 - point;

    var a = Vector3.Dot(edge0, edge0);
    var b = Vector3.Dot(edge0, edge1);
    var c = Vector3.Dot(edge1, edge1);
    var d = Vector3.Dot(edge0, v0);
    var e = Vector3.Dot(edge1, v0);

    var det = a * c - b * b;
    var s = b * e - c * d;
    var t = b * d - a * e;

    if (s + t < det) {
      if (s < 0.0f) {
        if (t < 0.0f) {
          if (d < 0.0f) {
            s = Mathf.Clamp(-d / a, 0.0f, 1.0f);
            t = 0.0f;
          } else {
            s = 0.0f;
            t = Mathf.Clamp(-e / c, 0.0f, 1.0f);
          }
        } else {
          s = 0.0f;
          t = Mathf.Clamp(-e / c, 0.0f, 1.0f);
        }
      } else if (t < 0.0f) {
        s = Mathf.Clamp(-d / a, 0.0f, 1.0f);
        t = 0.0f;
      } else {
        var invDet = 1.0f / det;
        s *= invDet;
        t *= invDet;
      }
    } else {
      if (s < 0.0f) {
        var tmp0 = b + d;
        var tmp1 = c + e;
        if (tmp1 > tmp0) {
          var numer = tmp1 - tmp0;
          var denom = a - 2 * b + c;
          s = Mathf.Clamp(numer / denom, 0.0f, 1.0f);
          t = 1 - s;
        } else {
          t = Mathf.Clamp(-e / c, 0.0f, 1.0f);
          s = 0.0f;
        }
      } else if (t < 0.0f) {
        if (a + d > b + e) {
          var numer = c + e - b - d;
          var denom = a - 2 * b + c;
          s = Mathf.Clamp(numer / denom, 0.0f, 1.0f);
          t = 1 - s;
        } else {
          s = Mathf.Clamp(-e / c, 0.0f, 1.0f);
          t = 0.0f;
        }
      } else {
        var numer = c + e - b - d;
        var denom = a - 2 * b + c;
        s = Mathf.Clamp(numer / denom, 0.0f, 1.0f);
        t = 1.0f - s;
      }
    }

    return p1 + s * edge0 + t * edge1;
  }
}