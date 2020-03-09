using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SpaceColonizationTests {
        [Test]
        public void TestGetAttractors() {
            var a = new Vector2(0, 0);
            var b = new Vector2(0, 1);
            var c = new Vector2(0, 2);
            var d = new Vector2(0, 3);
            var veinNodes = new KDTree(a);
            veinNodes.Add(b);
            veinNodes.Add(c);
            veinNodes.Add(d);

            var v = new Vector2(-1, 0.75f);
            var w = new Vector2(1.75f, 2.25f);
            var x = new Vector2(-1.5f, 3.25f);
            var y = new Vector2(-1.5f, 5f);
            var z = new Vector2(2, 4.5f);
            var auxins = new KDTree(v);
            auxins.Add(w);
            auxins.Add(x);
            auxins.Add(y);
            auxins.Add(z);

            var colonizer = new SpaceColonization(100f, 1f, 1f, 0f, 90f, 1000);

            var attractors = colonizer.Attractors(veinNodes, auxins);
            Debug.Assert(!attractors.ContainsKey(a));

            Debug.Assert(attractors.ContainsKey(b));
            Debug.Assert(attractors[b].Count == 1);
            Debug.Assert(attractors[b][0] == v);

            Debug.Assert(attractors.ContainsKey(c));
            Debug.Assert(attractors[c].Count == 1);
            Debug.Assert(attractors[c][0] == w);

            Debug.Assert(attractors.ContainsKey(d));
            Debug.Assert(attractors[d].Count == 3);
            Debug.Assert(attractors[d][0] == x);
            Debug.Assert(attractors[d][1] == y);
            Debug.Assert(attractors[d][2] == z);
        }

        [Test]
        public void TestAvgPull() {
            var colonizer = new SpaceColonization(1f, 1f, 1f, 0f, 90f, 1000);
            var avg = colonizer.Avg(
                new List<Vector2>() {
                    new Vector2(-2, 2),
                    new Vector2(-1, 3),
                    new Vector2(0, 3),
                    new Vector2(1, 2)
                }
            );
            Debug.Assert(avg == new Vector2(-0.5f, 2.5f));
        }

        [Test]
        public void TestGrowEndpt() {
            var colonizer = new SpaceColonization(1f, 1f, 1f, 0f, 90f, 1000);
            var activeNodes = new List<Vector2>();
            activeNodes.Add(Vector2.zero);
            var auxin = new Vector2(1, 1);
            foreach (var node in activeNodes) {
                var grown = colonizer.GrowEndpt(node, new List<Vector2>(){ auxin });
                Debug.Assert(grown == auxin.normalized);
            }
        }

        [Test]
        public void TestKillAuxins() {
            var colonizer = new SpaceColonization(1f, 1f, 2f, 0f, 90f, 1000);
            var activeNodes = new List<Vector2>();
            activeNodes.Add(Vector2.zero);
            activeNodes.Add(new Vector2(-1, -1));
            var auxins = new KDTree(new Vector2(1, 1));
            auxins.Add(new Vector2(7, -2));
            Debug.Assert(auxins.Count == 2);
            colonizer.KillAuxins(activeNodes, auxins);
            Debug.Assert(auxins.Count == 1);
            activeNodes.AddRange(auxins.ToList());
            colonizer.KillAuxins(activeNodes, auxins);
            Debug.Assert(auxins.Count == 0);
        }

        [Test]
        public void TestEdgesToKDTree() {
            var colonizer = new SpaceColonization(1f, 1f, 2f, 0f, 90f, 1000);
            var veinNodes = new List<Vector2>(){
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(0, 2),
                new Vector2(0, 3),
                new Vector2(0, 4),
                new Vector2(0, 5),
            };
            var veinEdges = new List<Edge>();
            for (int i = 1; i < veinNodes.Count; i++) {
                veinEdges.Add(new Edge(veinNodes[i - 1], veinNodes[i]));
            }
            Debug.Assert(veinEdges.Count == 5);
            var tree = colonizer.EdgesToKDTree(veinEdges);
            Debug.Assert(tree.Count == veinNodes.Count);
            Debug.Assert(tree.Has(veinEdges[0].L));
            foreach (var edge in veinEdges) {
                Debug.Assert(tree.Has(edge.R));
            }
        }

        [Test]
        public void TestRotate() {
            var r = SpaceColonization.rotate(new Vector2(0, 1), 90f);
            Debug.Assert(r == Vector2.right);
        }
    }
}
