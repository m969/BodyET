using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class KDTreeTests
    {
        [Test]
        public void KDTreeTestsAddAndHas() {
            var root = new KDTree(new Vector2(0, 0));
            root.Add(new Vector2(1, 2));
            root.Add(new Vector2(3, -4));
            root.Add(new Vector2(0, -2));
            Debug.Assert(root.Has(new Vector2(1, 2)));
            Debug.Assert(root.Has(new Vector2(3, -4)));
            Debug.Assert(root.Has(new Vector2(0, -2)));
            Debug.Assert(!root.Has(new Vector2(1, -4)));
            Debug.Assert(!root.Has(new Vector2(1, -2)));
            Debug.Assert(!root.Has(new Vector2(-1, -2)));
        }

        [Test]
        public void KDTreeTestsFind() {
            var root = new KDTree(new Vector2(0, 0));
            List<Vector2> vs = new List<Vector2>{
                new Vector2(1, 2),
                new Vector2(3, -4),
                new Vector2(0, -2),
                new Vector2(1, -4),
                new Vector2(1, -2),
                new Vector2(-1, -2),
            };

            foreach (Vector2 v in vs) {
                root.Add(v);
            }

            for (int i = 0; i < vs.Count; i++) {
                var found = root.Find(vs[i]);
                Debug.Assert(found != null);
                Debug.Assert(found.Pt == vs[i]);
            }
            Debug.Assert(root.Find(new Vector2(-123, 123)) == null);
        }

        [Test]
        public void KDTreeMin() {
            var origin = new Vector2(0, 0);
            var root = new KDTree(origin);
            var v = new Vector2(1, 2);
            root.Add(v);
            Debug.Assert(root.Min(0) == origin);
            Debug.Assert(root.Min(1) == origin);
            v = new Vector2(-1, 2);
            root.Add(v);
            Debug.Assert(root.Min(0) == v);
            Debug.Assert(root.Min(1) == origin);
            v = new Vector2(-1, -2);
            root.Add(v);
            Debug.Assert(root.Min(0)[0] == -1);
            Debug.Assert(root.Min(1) == v);
        }

        [Test]
        public void KDTreeRm() {
            var root = new KDTree(new Vector2(0, 0));
            List<Vector2> vs = new List<Vector2>{
                new Vector2(1, 2),
                new Vector2(3, -4),
                new Vector2(0, -2),
                new Vector2(1, -4),
                new Vector2(1, -2),
                new Vector2(-1, -2),
            };

            foreach (Vector2 v in vs) {
                root.Add(v);
            }

            foreach (Vector2 v in vs) {
                Debug.Assert(root.Has(v));
                root.Rm(v);
                Debug.Assert(!root.Has(v));
            }
        }

        [Test]
        public void KDTreeNearestNeighbor() {
            var root = new KDTree(new Vector2(0, 0));
            root.Add(new Vector2(0, 1));
            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    if (i != 0 && j != 0)
                        root.Add(new Vector2(i, j));
                }
            }
            List<Vector2> pts = root.NearestNeighbors(new Rect(0.95f, 0.95f, 1.1f, 1.1f));
            Debug.Assert(pts.Count == 4);
            pts = root.NearestNeighbors(new Rect(0.95f, 0.95f, 2.1f, 2.1f));
            Debug.Assert(pts.Count == 9);
        }
    }
}
