using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceColonizationTextureDrawer : MonoBehaviour
{
    public float searchRadius = 25f;
    public float growDist = 1f;
    public float killDist = 10f;
    public float splitChance = 0.2f;
    public float branchAngle = 30f;
    public int numAuxins = 600;
    public int width = 200;
    public int height = 200;
    public int iters = 100;
    public int drawSpeed = 10;
    public float splitChanceDecay = 0.1f;
    public float growChance = 0.5f;
    public bool drawOnStart = true;
    private SpaceColonization colony;
    private Texture2D texture;
    private List<Vector2> startingVein = new List<Vector2>();

    void Start() {
        if (drawOnStart) {
            Generate();
        }
    }

    public void Generate() {
        texture = new Texture2D(width, height, TextureFormat.Alpha8, false);
        Color[] cs = new Color[width * height];
        for (int i = 0; i < width * height; i++) {
            cs[i] = Color.clear;
        }
        texture.SetPixels(0, 0, width, height, cs);
        texture.alphaIsTransparency = true;
        texture.Apply();
        GetComponent<Renderer>().material.mainTexture = texture;
        startingVein = new List<Vector2>() {
            new Vector2(width/2 + 0, height/2 + 0),
            new Vector2(width/2 + 1, height/2 + 1),
            new Vector2(width/2 + 1, height/2 - 1),
            new Vector2(width/2 - 1, height/2 + 1),
            new Vector2(width/2 - 1, height/2 - 1),
        };
        colony = new SpaceColonization(
            searchRadius,
            growDist,
            killDist,
            splitChance,
            branchAngle,
            iters,
            splitChanceDecay,
            growChance
        );
        DrawVeins(colony.ListToEdges(startingVein));
        var activeNodes = new List<Vector2>();
        activeNodes.AddRange(startingVein);
        var auxins = colony.GenAuxins(0, 0, width, height, numAuxins);
        StartCoroutine(
            DrawThread(
                startingVein,
                colony.ListToKDTree(auxins),
                activeNodes
            )
        );
    }

    // For debugging purposes.
    void DrawAuxins(List<Vector2> auxins) {
        foreach (var auxin in auxins) {
            var x = Mathf.RoundToInt(auxin.x);
            var y = Mathf.RoundToInt(auxin.y);
            texture.SetPixel(x, y, Color.red);
        }
        texture.Apply();
    }

    void DrawVeins(List<Edge> edges) {
        foreach (var edge in edges) {
            DrawLine(edge.L, edge.R, Color.black);
        }
    }

    // Draws a line as an asynchronous process.
    IEnumerator DrawThread(List<Vector2> startingVein, KDTree auxins, List<Vector2> activeNodes) {
        int i = 0;
        foreach (var vein in colony.YieldGrow(startingVein, auxins, startingVein)) {
            DrawLine(vein.L, vein.R, Color.black);
            i++;
            if (i > drawSpeed) {
                i = 0;
                yield return null;
            }
        }
    }

    // Adapted from wiki.unity3d.com:
    void DrawLine(Vector2 v0, Vector2 v1, Color col) {
        Vector2 diff = v1 - v0;
        int stepX = 0;
        int stepY = 0;
     
        if (diff.y < 0) {
            diff.y *= -1;
            stepY--;
        } else {
            stepY = 1;
        }

        if (diff.x < 0) {
            diff.x *= -1;
            stepX--;
        } else {
            stepX = 1;
        }

        diff *= 2;
     
        float fraction = 0;
     
        texture.SetPixel(Mathf.RoundToInt(v0.x), Mathf.RoundToInt(v0.y), col);
        if (diff.x > diff.y) {
            fraction = diff.y - (diff.x / 2);
            while (Mathf.Abs(v0.x - v1.x) > 1) {
                if (fraction >= 0) {
                    v0.y += stepY;
                    fraction -= diff.x;
                }
                v0.x += stepX;
                fraction += diff.y;
                texture.SetPixel(Mathf.RoundToInt(v0.x), Mathf.RoundToInt(v0.y), col);
            }
        }
        else {
            fraction = diff.x - (diff.y / 2);
            while (Mathf.Abs(v0.y - v1.y) > 1) {
                if (fraction >= 0) {
                    v0.x += stepX;
                    fraction -= diff.y;
                }
                v0.y += stepY;
                fraction += diff.x;
                texture.SetPixel(Mathf.RoundToInt(v0.x), Mathf.RoundToInt(v0.y), col);
            }
        }
        texture.Apply();
    }
}
