using System.Collections.Generic;
using UnityEngine;

namespace Instructions
{
    public class BezierLines
    {
        [SerializeField] private int segmentsCount = 100;
        private List<BezierCurve> _curves;
        private static BezierLines instance;

        public static BezierLines Instance
        {
            get { return instance ?? (instance = new BezierLines()); }
        }

        private BezierLines()
        {
            _curves = new List<BezierCurve>();
        }

        public BezierCurve AddCurve(Vector3[] points, Color? startColor = null, Color? endColor = null,
            float startWidth = 0.2f, float endWidth = 0.2f)
        {
            Color beginColor = startColor ?? Color.red;
            Color finishColor = endColor ?? Color.red;
            GameObject line = new GameObject("Line-" + _curves.Count);
            line.transform.position = Vector3.zero;
            line.transform.localPosition = Vector3.zero;
            line.transform.localScale = Vector3.one;
            line.AddComponent<RectTransform>();
            line.AddComponent<LineRenderer>();
            LineRenderer render = line.GetComponent<LineRenderer>();
            render.useWorldSpace = true;
            render.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            render.startColor = beginColor;
            render.endColor = finishColor;
            render.startWidth = startWidth;
            render.endWidth = endWidth;
            render.positionCount = segmentsCount;

            var curve = line.AddComponent<BezierCurve>();
            curve.Init(segmentsCount, points, render);
            curve.Render();

            _curves.Add(curve);

            return curve;
        }

//        public void RemoveCurves()
//        {
//            _curves.Clear();
//            int childs = transform.childCount;
//            for (int i = childs - 1; i >= 0; i--)
//                Destroy(transform.GetChild(i).gameObject);
//        }
    }
}