using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class GizmosDebug: MonoBehaviour
    {
        public static GizmosDebug Instance { get; private set; }

        public List<Vector3> Path { get; set; } = new List<Vector3>() { Vector3.zero, Vector3.zero };
        public Vector3 From { get; set; }
        public Vector3 Direction { get; set; }
        public Ray Ray { get; set; } = new Ray(Vector3.zero, Vector3.zero);

        private void Awake()
        {
            Instance = this;
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawRay(From, Direction);
            Gizmos.DrawRay(Ray.origin, Ray.direction);
            if (this.Path.Count < 2)
            {
                return;
            }
            for (int i = 0; i < Path.Count - 1; ++i)
            {
                Gizmos.DrawLine(Path[i], Path[i + 1]);
            }
        }
    }
}