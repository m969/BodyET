using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KDTree {
    public Vector2 Pt;
    public int Count;
    KDTree Left;
    KDTree Right;
    const int k = 2;

    public KDTree(Vector2 pt) {
        this.Pt = pt;
        Count++;
    }

    public void Add(Vector2 pt) {
        add(pt, 0);
        Count++;
    }

    private void add(Vector2 toAdd, int depth) {
        if (this.Pt[depth % k] > toAdd[depth % k]) {
            if (Left == null) {
                Left = new KDTree(toAdd);
            } else {
                Left.add(toAdd, depth+1);
            }
        } else {
            if (Right == null) {
                Right = new KDTree(toAdd);
            } else {
                Right.add(toAdd, depth+1);
            }
        }
    }

    public bool Has(Vector2 query) {
        return has(query, 0);
    }

    private bool has(Vector2 query, int depth) {
        if (this.Pt == query) {
            return true;
        } else if (Pt[depth % k] > query[depth % k]) {
            if (Left == null) {
                return false;
            } else {
                return Left.has(query, depth+1);
            }
        } else {
            if (Right == null) {
                return false;
            } else {
                return Right.has(query, depth+1);
            }
        }
    }

    public KDTree Find(Vector2 query) {
        return find(query, 0);
    }

    private KDTree find(Vector2 query, int depth) {
        if (Pt == query) {
            return this;
        } else if (Pt[depth % k] > query[depth % k]) {
            if (Left == null) {
                return null;
            } else {
                return Left.find(query, depth+1);
            }
        } else {
            if (Right == null) {
                return null;
            } else {
                return Right.find(query, depth+1);
            }
        }
    }

    public Vector2 Min(int d) {
        return min(d, 0);
    }

    public Vector2 min(int dim, int depth) {
        Vector2 minleft = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        Vector2 minright = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        // Find left and right minimums, if applicable.
        if (dim == depth % k && Left == null) {
            minleft = Pt;
        } else if (dim == depth % k && Left != null) {
            minleft = Left.min(dim, depth+1);
        } else if (dim != depth % k) {
            if (Left != null)
                minleft = Left.min(dim, depth+1);
            if (Right != null)
                minright = Right.min(dim, depth+1);
        }
        // Return smaller of left or right.
        return minv(Pt, minv(minleft, minright, dim), dim);
    }

    private static Vector2 minv(Vector2 v, Vector2 u, int d) {
        if (v[d] < u[d]) {
            return v;
        } else {
            return u;
        }
    }

    public void Rm(Vector2 query) {
        rm(query, this, 0);
        Count--;
    }

    private void rm(Vector2 query, KDTree parent, int depth) {
        // If we're the node to be deleted...
        if (Pt == query) {
            var isleft = parent.Left == this;
            if (Leaf()) {
                if (isleft) {
                    parent.Left = null;
                } else {
                    parent.Right = null;
                }
            } else {
                if (Right != null) {
                    Pt = Right.Min(depth % k);
                    Right.rm(Pt, this, depth+1);
                } else if (Left != null) {
                    Pt = Left.Min(depth % k);
                    Left.rm(Pt, this, depth+1);
                    Right = Left;
                    Left = null;
                }
            }
        } else if (Pt[depth % k] > query[depth % k]) {
            if (Left == null) {
                return;
            } else {
                Left.rm(query, this, depth+1);
            }
        } else {
            if (Right == null) {
                return;
            } else {
                Right.rm(query, this, depth+1);
            }
        }
    }

    private bool matches(KDTree node, Vector2 query) {
        if (node == null) {
            return false;
        } else {
            return node.Pt == query;
        }
    }

    public bool Leaf() {
        return (Left == null && Right == null);
    }

    /// The input rect is NOT inclusive. A point on the line is excluded.
    public List<Vector2> NearestNeighbors(Rect searchArea) {
        List<Vector2> pts = new List<Vector2>();
        nns(searchArea, ref pts, 0);
        return pts;
    }

    public List<Vector2> NearestNeighbors(Vector2 center, float width) {
        return NearestNeighbors(new Rect(center.x - width, center.y - width, width * 2, width * 2));
    }

    private void nns(Rect r, ref List<Vector2> pts, int depth) {
        if (r.Contains(Pt)) {
            pts.Add(Pt);
        }
        int d = depth%k;
        if (Left != null && Pt[d] > r.min[d]) {
            Left.nns(r, ref pts, depth+1);
        }
        if (Right != null && Pt[d] < r.max[d]) {
            Right.nns(r, ref pts, depth+1);
        }
    }

    public List<Vector2> ToList() {
        List<Vector2> pts = new List<Vector2>();
        pts.Add(Pt);
        if (Left != null) {
            pts.AddRange(Left.ToList());
        }
        if (Right != null) {
            pts.AddRange(Right.ToList());
        }
        return pts;
    }
}
