using System;
//using BulletSharp;
//using BulletSharp.Math;
using System.Collections;
using ETModel;
using UnityEngine;

namespace ETModel.Primitives
{
    /// <summary>
    /// BCylinder
    /// </summary>
    // [RequireComponent(typeof(BRigidBody))]
    // [RequireComponent(typeof(BCapsuleShape))]
    public class BCapsule : BPrimitive
    {

        // public BCapsuleMeshSettings meshSettings = new BCapsuleMeshSettings();
        public float height = 2;
        public float radius = 0.3f;
        public BCapsuleShape.CapsuleAxis upAxis;
        
        public static Unit CreateNew(Vector3 position, Quaternion rotation)
        {
            Unit go = new Unit();
            BCapsule bCylinder = go.AddComponent<BCapsule>();
            CreateNewBase(go, position, rotation);
            bCylinder.BuildMesh();
            go.name = "BCapsule";
            return go;
        }

        public override void BuildMesh()
        {
            BCapsuleShape cs =this.GetParent<Unit>().GetComponent<BCapsuleShape>();
            cs.Height = this.height;
            cs.Radius = this.radius;
            cs.UpAxis = this.upAxis;
        }


    }
}
