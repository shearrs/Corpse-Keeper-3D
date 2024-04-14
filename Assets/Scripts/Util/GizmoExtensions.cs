using UnityEngine;

public static class GizmoExtensions
{
    public static void DrawWireCylinder(Vector3 point1, Vector3 point2, float radius)
    {
        Gizmos.DrawWireSphere(point1, radius);
        Gizmos.DrawWireSphere(point2, radius);

        Gizmos.DrawLine(point1 + radius * Vector3.forward, point2 + radius * Vector3.forward);
        Gizmos.DrawLine(point1 + radius * Vector3.right, point2 + radius * Vector3.right);
        Gizmos.DrawLine(point1 + radius * Vector3.back, point2 + radius * Vector3.back);
        Gizmos.DrawLine(point1 + radius * Vector3.left, point2 + radius * Vector3.left);
    }

    public static void DrawBarbell(Vector3 point1, Vector3 point2, float radius, Color pointColor, Color lineColor)
    {
        Gizmos.color = pointColor;
        Gizmos.DrawSphere(point1, radius);
        Gizmos.DrawSphere(point2, radius);

        Gizmos.color = lineColor;
        Gizmos.DrawLine(point1, point2);
    }
}