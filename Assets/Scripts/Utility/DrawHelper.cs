using UnityEngine;

public static class DrawHelper {
    private static Transform objectTransform;
    
    public static void SetTransform(Transform targetTransform){
        objectTransform = targetTransform;
    }
    public static void DrawCapsule(float height, float sphereRadius){
        Gizmos.color = Color.blue;
        Vector2 centerPosition = (Vector2)objectTransform.position;
        Vector2 up = centerPosition + Vector2.up * (float)(height/2); 
        Vector2 down = centerPosition + Vector2.down * (float)(height/2); 
        Gizmos.DrawLine(centerPosition, up);
        Gizmos.DrawLine(centerPosition, down);
        Gizmos.DrawWireSphere(up, sphereRadius);
        Gizmos.DrawWireSphere(down,sphereRadius);
    }
    public static void DrawRayBox(Vector2 direction, float distance, Vector2 boxSize)
    {
        Gizmos.color = Color.green;
        Vector2 start = (Vector2)objectTransform.position;
        Vector2 end = start + direction * distance;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireCube(end, boxSize);
    }
    public static void DrawRaySphere(Vector2 direction, float distance, float sphereRadius, Color color)
    {
        Gizmos.color = color;
        Vector2 start = (Vector2)objectTransform.position;
        Vector2 end = start + direction * distance;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(end, sphereRadius);
    }
    public static void DrawyRayLine(Vector2 direction, float distance){
        Gizmos.color = Color.cyan;
        Vector2 start = (Vector2)objectTransform.position;
        Vector2 end = start + direction * distance;
        Gizmos.DrawLine(start,end);
    }
}