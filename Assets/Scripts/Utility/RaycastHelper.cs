using UnityEngine;

public static class RaycastHelper
{
    public static RaycastHit2D GetCircleHit(Vector3 origin,Vector2 direction, float raidus, float distance, LayerMask layerMask)
    {
        return Physics2D.CircleCast(origin, raidus, direction, distance,layerMask);
    }
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