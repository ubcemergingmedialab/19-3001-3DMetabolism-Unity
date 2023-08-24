using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EMLMeshMath
{
    //public Transform targetObject;
    //public MeshFilter meshFilter;

    public static Vector3 GetClosestPoint(Transform targetObject, MeshFilter meshFilter)
    {
        if (meshFilter != null && targetObject != null)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Vector3 closestPoint = Vector3.zero;
            float closestDistanceSqr = float.MaxValue;

            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                Vector3 v1 = mesh.vertices[mesh.triangles[i]];
                Vector3 v2 = mesh.vertices[mesh.triangles[i + 1]];
                Vector3 v3 = mesh.vertices[mesh.triangles[i + 2]];

                Vector3 faceCenter = (v1 + v2 + v3) / 3;
                float distanceSqr = (faceCenter - targetObject.position).sqrMagnitude;

                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;

                    // Calculate closest point on the triangle
                    Vector3 closestPointOnFace = CalculateClosestPointOnTriangle(targetObject.position, v1, v2, v3);
                    closestPoint = closestPointOnFace;
                }
            }

            Debug.DrawLine(targetObject.position, closestPoint, Color.red, 1000f);

            return closestPoint;
        }

        else
            return new Vector3(0, 0, 0);
    }

    // Helper function to calculate the closest point on a triangle to a given point
    private static Vector3 CalculateClosestPointOnTriangle(Vector3 point, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // Calculate the triangle's normal
        Vector3 triangleNormal = Vector3.Cross(v2 - v1, v3 - v1).normalized;

        // Calculate the projected point onto the plane defined by the triangle
        Vector3 projectedPoint = point - Vector3.Dot(point - v1, triangleNormal) * triangleNormal;

        // Clamp the projected point within the triangle's boundaries
        Vector3 closestPoint = Vector3.zero;
        float distance = float.MaxValue;

        float s, t;
        if (PointInTriangle(projectedPoint, v1, v2, v3, out s, out t))
        {
            // Projected point is within the triangle's boundaries
            closestPoint = projectedPoint;
            distance = Vector3.Distance(point, closestPoint);
        }
        else
        {
            // Find the closest vertex of the triangle
            float distV1 = Vector3.Distance(point, v1);
            float distV2 = Vector3.Distance(point, v2);
            float distV3 = Vector3.Distance(point, v3);

            if (distV1 < distV2 && distV1 < distV3)
            {
                closestPoint = v1;
                distance = distV1;
            }
            else if (distV2 < distV3)
            {
                closestPoint = v2;
                distance = distV2;
            }
            else
            {
                closestPoint = v3;
                distance = distV3;
            }
        }

        return closestPoint;
    }

    // Helper function to determine if a point is inside a triangle
    private static bool PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c, out float s, out float t)
    {
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        Vector3 ap = p - a;

        float d1 = Vector3.Dot(ab, ap);
        float d2 = Vector3.Dot(ac, ap);

        if (d1 <= 0.0f && d2 <= 0.0f)
        {
            s = 0.0f;
            t = 0.0f;
            return true;
        }

        Vector3 bp = p - b;
        float d3 = Vector3.Dot(ab, bp);
        float d4 = Vector3.Dot(ac, bp);

        if (d3 >= 0.0f && d4 <= d3)
        {
            s = 1.0f;
            t = 0.0f;
            return true;
        }

        float vc = d1 * d4 - d3 * d2;
        if (vc <= 0.0f && d1 >= 0.0f && d3 <= 0.0f)
        {
            s = d1 / (d1 - d3);
            t = 1.0f - s;
            return true;
        }

        Vector3 cp = p - c;
        float d5 = Vector3.Dot(ab, cp);
        float d6 = Vector3.Dot(ac, cp);

        if (d6 >= 0.0f && d5 <= d6)
        {
            s = 0.0f;
            t = 1.0f;
            return true;
        }

        float vb = d5 * d2 - d1 * d6;
        if (vb <= 0.0f && d2 >= 0.0f && d6 <= 0.0f)
        {
            t = d2 / (d2 - d6);
            s = 1.0f - t;
            return true;
        }

        float va = d3 * d6 - d5 * d4;
        if (va <= 0.0f && (d4 - d3) >= 0.0f && (d5 - d6) >= 0.0f)
        {
            t = (d4 - d3) / ((d4 - d3) + (d5 - d6));
            s = 1.0f - t;
            return true;
        }

        s = va / (va + vb + vc);
        t = vb / (va + vb + vc);

        return (s >= 0.0f) && (t >= 0.0f) && (s + t <= 1.0f);
    }
}