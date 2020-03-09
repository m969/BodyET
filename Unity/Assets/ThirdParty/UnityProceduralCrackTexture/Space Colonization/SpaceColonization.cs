using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceColonization {
    float searchRadius;
    float growDist;
    float killDist;
    float splitChance;
    float branchAngle;
    int iters;
    float decayPerTurn;
    float growChance;

    public SpaceColonization(
      float searchRadius,
      float growDist,
      float killDist,
      float splitChance,
      float branchAngle,
      int iters,
      float splitChanceDecay = 0f,
      float growChance = 1f
    ) {
        this.searchRadius = searchRadius;
        this.growDist = growDist;
        this.killDist = killDist;
        this.splitChance = splitChance;
        this.branchAngle = branchAngle;
        this.iters = iters;
        this.decayPerTurn = splitChanceDecay / iters;
        this.growChance = growChance;
    }

    /// <summary>
    /// Generate a list of random Vector2s.
    /// </summary>
    /// <param name="minx">The smallest X coordinate an auxin can have.</param>
    /// <param name="miny">The smallest y coordinate an auxin can have.</param>
    /// <param name="maxx">The largest y coordinate an auxin can have.</param>
    /// <param name="maxy">The largest y coordinate an auxin can have.</param>
    /// <param name="numAuxins">The total number of auxins to generate.</param>
    /// <returns>A list of all auxins generated.</returns>
    public List<Vector2> GenAuxins(float minx, float miny, float maxx, float maxy, int numAuxins) {
        var seed = System.DateTime.Now.Millisecond;
        var auxins = new List<Vector2>();
        for (int i = 0; i < numAuxins; i++) {
            seed++;
            Random.InitState(seed);
            auxins.Add(new Vector2(Random.Range(minx, maxx), Random.Range(miny, maxy)));
        }
        return auxins;
    }

    /// <summary>
    /// For each auxin, calculate the nearest vein node.
    /// </summary>
    /// <param name="veinNodes">The current set of vein nodes, as a KDTree.</param>
    /// <param name="auxins">All available auxins, as a KDTree.</param>
    /// <returns>A mapping from vein nodes to it's attractors. If a vein node
    /// was not the closest node to any provided auxin, then it will not be a
    /// key in the dictionary.</returns>
    public Dictionary<Vector2, List<Vector2>> Attractors(KDTree veinNodes, KDTree auxins) {
        Dictionary<Vector2, List<Vector2>> attractors = new Dictionary<Vector2, List<Vector2>>();
        foreach (Vector2 auxin in auxins.ToList()) {
            var neighbors = veinNodes.NearestNeighbors(auxin, searchRadius);
            if (neighbors.Count == 0) {
                continue;
            }
            var nearest = NearestVector(auxin, neighbors);
            if (attractors.ContainsKey(nearest)) {
                attractors[nearest].Add(auxin);
            } else {
                attractors[nearest] = new List<Vector2>(){auxin};
            }
        }
        return attractors;
    }

    /// <summary>
    /// Given a single point, v, and all nearby points, searchSpace, return the
    /// closest s in searchSpace to v.
    /// </summary>
    /// <param name="v">An arbitrary point in space.</param>
    /// <param name="searchSpace">A set of points near v.</param>
    /// <returns>The pt in searchSpace closest to the query point, v.</returns>
    private static Vector2 NearestVector(Vector2 v, List<Vector2> searchSpace) {
        float minDist = float.PositiveInfinity;
        Vector2 nearest = searchSpace[0];
        foreach (Vector2 candidate in searchSpace) {
            float dist = Vector2.Distance(v, candidate);
            if (dist < minDist) {
                minDist = dist;
                nearest = candidate;
            }
        }
        return nearest;
    }

    /// <param name="pts">A list of points.</param>
    /// <returns>The cneter of pts.</returns>
    public Vector2 Avg(List<Vector2> pts) {
        var avg = Vector2.zero;
        foreach (var pt in pts) {
            avg += pt;
        }
        return avg / pts.Count;
    }

    /// <param name="activeNode">A node that you would like to grow towards some auxins.</param>
    /// <param name="attractors">A set of attractors for activeNode.</param>
    /// <returns>The next vein node after growing activeNode.</returns>
    public Vector2 GrowEndpt(Vector2 activeNode, List<Vector2> attractors) {
        var attractionPt = Avg(attractors);
        var toAttraction = attractionPt - activeNode;
        return toAttraction.normalized * growDist + activeNode;
    }

    /// <summary>The growth phase of growing nodes, killing auxins, and
    /// creating braches. </summary>
    /// <param name="activeNodes">A list of nodes / endpts / buds that are
    /// growing towards auxins.</param>
    /// <param name="veinNodes">activeNodes, but as a KDTree instead of a list.</param>
    /// <param name="auxins">All auxins as a KDTree.</param>
    /// <param name="completedVeins">All vein edges that have been grown.</param>
    public void GrowNodes(List<Vector2> activeNodes, KDTree veinNodes, KDTree auxins, List<Edge> completedVeins) {
        var attractorDict = Attractors(veinNodes, auxins);
        var newActiveNodes = new List<Vector2>();
        while (activeNodes.Count > 0) {
            var node = activeNodes[0];
            activeNodes.RemoveAt(0);
            if (attractorDict.ContainsKey(node)) {
                if (Random.value < growChance) {
                    var nextEndpt = GrowEndpt(node, attractorDict[node]);
                    completedVeins.Add(new Edge(node, nextEndpt));
                    newActiveNodes.Add(nextEndpt);
                    veinNodes.Add(nextEndpt);
                }
                else {
                    newActiveNodes.Add(node);
                }
            }
        }
        activeNodes.AddRange(newActiveNodes);
    }

    /// <summary>
    /// Get rid of any auxins within KillDist of an actively growing node.
    /// </summary>
    /// <param name="activeNodes">A list of actively growing nodes.</param>
    /// <param name="auxins">A KDTree of a all auxins.</param>
    public void KillAuxins(List<Vector2> activeNodes, KDTree auxins) {
        foreach (var node in activeNodes) {
            foreach (var auxin in auxins.NearestNeighbors(node, killDist)) {
                auxins.Rm(auxin);
            }
        }
    }

    /// <summary>
    /// Convert a list of Edges into a KDTree.
    /// </summary>
    public KDTree EdgesToKDTree(List<Edge> begVein) {
        if (begVein.Count == 0) {
            Debug.LogError("Empty List passed to EdgesToKDTree.");
        }
        var veinNodes = new KDTree(begVein[0].L);
        foreach (var veinNode in begVein) {
            veinNodes.Add(veinNode.R);
        }
        return veinNodes;
    }

    /// <summary>
    /// Convert a list of Vector2s into a KDTree.
    /// </summary>
    public KDTree ListToKDTree(List<Vector2> vs) {
        if (vs.Count == 0) {
            Debug.LogError("Empty List passed to ListToKDTree.");
        }
        var tree = new KDTree(vs[0]);
        for (int i = 1; i < vs.Count; i++) {
            tree.Add(vs[i]);
        }
        return tree;
    }

    /// <summary>
    /// Convert a list of Vector2s into a list of Edges.
    /// </summary>
    public List<Edge> ListToEdges(List<Vector2> pts) {
        var edges = new List<Edge>();
        for (int i = 1; i < pts.Count; i++) {
            edges.Add(new Edge(pts[i - 1], pts[i]));
        }
        return edges;
    }

    /// <summary>
    /// Execute the space colonization algorithm, yielding each completed vein
    /// edge as it goes.
    /// </summary>
    /// <param name="begVein">An beginning set of points to grow off of.</param>
    /// <param name="auxins">All auxins.</param>
    /// <param name="activeNodes">
    /// Which nodes in the begVein are considered actively growing.
    /// </param>
    /// <returns></returns>
    public IEnumerable<Edge> YieldGrow(List<Vector2> begVein, KDTree auxins, List<Vector2> activeNodes) {
        var veinNodes = ListToKDTree(begVein);
        var completedVeins = ListToEdges(activeNodes);
        completedVeins = (from pt in completedVeins
                          where !activeNodes.Contains(pt.R)
                          select pt).ToList();
        int edgePlace = 0;
        for (int i = 0; i < iters; i++) {
            GrowNodes(activeNodes, veinNodes, auxins, completedVeins);
            KillAuxins(activeNodes, auxins);
            Branch(veinNodes, auxins, activeNodes, completedVeins);
            for (edgePlace++; edgePlace < completedVeins.Count; edgePlace++) {
                yield return completedVeins[edgePlace];
            }
            splitChance -= decayPerTurn;
        }
    }

    /// <summary>
    /// Rotate a Vector2 around the origin.
    /// </summary>
    public static Vector2 rotate(Vector2 v, float theta) {
        theta *= -Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(theta) - v.y * Mathf.Sin(theta),
            v.x * Mathf.Sin(theta) + v.y * Mathf.Cos(theta)
        );
    }

    /// <summary>
    /// Branch a list of active veins in the main grow veins, kill auxins,
    /// create branches cycle.
    /// </summary>
    public void Branch(KDTree veinNodes, KDTree auxins, List<Vector2> activeNodes, List<Edge> completedVeins) {
        var attractors = Attractors(veinNodes, auxins);
        var copy = new List<Vector2>();
        copy.AddRange(activeNodes);
        foreach (var node in copy) {
            if (Random.value < splitChance && attractors.ContainsKey(node)) {
                var branchedNode = (Avg(attractors[node]) - node).normalized * growDist;
                branchedNode = rotate(branchedNode, branchAngle);
                branchedNode += node;
                completedVeins.Add(new Edge(node, branchedNode));
                activeNodes.Add(branchedNode);
                veinNodes.Add(branchedNode);
            }
        }
    }
}
