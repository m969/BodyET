using System;
using BulletSharp;
//using BulletSharp.Math;
using System.Collections;
using UnityEngine;

namespace ETModel.Primitives
{
    /// <summary>
    /// Base class for UnityBullet primatives
    /// </summary>
    // [RequireComponent(typeof(MeshFilter))]
    // [RequireComponent(typeof(MeshRenderer))]
    [System.Serializable]
    public abstract class BPrimitive : Entity
    {
        public string info = "Information about this BPriitive";  //display in inspector
        
        

        public void Start()
        {

        }


        public static void CreateNewBase(Unit go, Vector3 position, Quaternion rotation)
        {
            go.Position = position;
            go.Quaternion = rotation;

            // // MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
            // // UnityEngine.Material material = new UnityEngine.Material(Shader.Find("Standard"));
            // meshRenderer.sharedMaterial = material;
        }

        /// <summary>
        /// Build object mesh and collider
        /// </summary>
        public virtual void BuildMesh()
        {

        }



    }
}
