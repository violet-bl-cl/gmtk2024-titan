using UnityEngine;

public static class RaycastHelper
{
   //private bool CheckSlope(Vector2 direction, float radius, float distance, LayerMask _layerMask)
   //{
   //    RaycastHit2D hitInfo = Physics2D.CircleCast(transform.position, radius, direction, distance, _layerMask);
   //    if (hitInfo.collider != null)
   //    {
   //        _slopePerpendicular = Vector2.Perpendicular(hitInfo.normal).normalized;
   //        float angle = Vector2.Angle(hitInfo.normal, direction);
   //        Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red);
   //        Debug.DrawRay(hitInfo.point, _slopePerpendicular, Color.cyan);
   //        return angle != 0 && angle < _slopeMax;
   //    }
   //    _slopePerpendicular = Vector2.zero;
   //    return false;
   //}
    public static bool CheckCircleSide(Vector3 origin,Vector2 direction, float raidus, float distance, LayerMask layerMask)
    {
        RaycastHit2D hitInfo = Physics2D.CircleCast(origin, raidus, direction, distance,layerMask);
        return hitInfo.collider != null;
    }
    public static bool CheckBoxSide(Vector3 origin , Vector2 direction, float distance, Vector2 size, LayerMask layerMask)
    {
        RaycastHit2D hitInfo = Physics2D.BoxCast(origin, size, 0f, direction, distance, layerMask);
        return hitInfo.collider != null;
    }
    public static bool CheckLineSide(Vector3 origin, Vector2 direction, float distance, LayerMask layerMask)
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(origin, direction, distance , layerMask);
        return hitInfo.collider != null;
    }
}