using UnityEngine;

public static class Extensions
{
  public static string ToHex(this Color32 color)
  {
    return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
  }

  public static Vector3 ClosestPointToLocalPoint(this BoxCollider box, Vector3 position)
  {
    var closest = position;

    var hs = box.size / 2.0f;
    var center = box.center;

    var min = center - hs;
    var max = center + hs;

    if (closest.x < min.x) {
      closest.x = min.x;
    } else if (closest.x > max.x) {
      closest.x = max.x;
    }

    if (closest.y < min.y) {
      closest.y = min.y;
    } else if (closest.y > max.y) {
      closest.y = max.y;
    }

    if (closest.z < min.z) {
      closest.z = min.z;
    } else if (closest.z > max.z) {
      closest.z = max.z;
    }

    return closest;
  }
}