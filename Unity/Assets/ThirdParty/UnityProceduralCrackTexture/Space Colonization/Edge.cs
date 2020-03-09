using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Edge
{
    public Vector2 L, R;
    public Edge(Vector2 L, Vector2 R) {
        this.L = L;
        this.R = R;
    }
}
