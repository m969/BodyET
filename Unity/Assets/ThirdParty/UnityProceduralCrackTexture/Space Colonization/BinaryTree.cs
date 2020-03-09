using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryTree {
    public Vector2 value;
    public BinaryTree L = null;
    public BinaryTree R = null;

    public BinaryTree(Vector2 pt) {
        value = pt;
    }

    public void Add(Vector2 toAdd) {
        if (L == null && R == null) {
            value = toAdd;
        }
        else if (L == null) {
            L = new BinaryTree(toAdd);
        }
        else if (R == null) {
            R = new BinaryTree(toAdd);
        }
        else {
            Debug.LogError("Cannot add node - node is full.");
        }
    }


    public List<Vector2> Leafs() {
        if (L == null && R == null) {
            return new List<Vector2>() { value };
        }
        else if (L != null && R != null) {
            var leafs = new List<Vector2>();
            leafs.AddRange(L.Leafs());
            leafs.AddRange(R.Leafs());
            return leafs;
        }
        else if (L != null) {
            return L.Leafs();
        }
        else if (R != null) {
            return R.Leafs();
        } else {
            Debug.LogError("Invalid state reached in BinaryTree.Leafs().");
            return null;
        }
    }
}
