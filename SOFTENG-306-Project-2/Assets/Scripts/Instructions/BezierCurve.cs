using UnityEngine;

namespace Instructions
{
    public class BezierCurve : MonoBehaviour
    {
        private int segmentsCount = 0;
        private Vector3[] points;
        private LineRenderer render;

        public void Init(int segmentsCount, Vector3[] points, LineRenderer render)
        {
            this.segmentsCount = segmentsCount;
            this.points = points;
            this.render = render;
        }

        public void UpdatePoints(Vector3[] points)
        {
            this.points = points;
            Render();
        }

        public void Render()
        {
            for (int i = 0; i < segmentsCount; i++)
            {
                float t = (float) i / (float) (segmentsCount - 1);
                Vector3 point = CalculateBezierPoint(t, points[0], points[1], points[2], points[3]);
                render.SetPosition(i, point);
            }
        }

        Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;
            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;
            return p;
        }
    }
}