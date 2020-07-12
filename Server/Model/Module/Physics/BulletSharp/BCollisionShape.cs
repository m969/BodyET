using BulletSharp;
using System;
using UnityEngine;
using ETModel;

namespace ETModel
{
    [System.Serializable]
    public abstract class BCollisionShape : Entity
    {
        public enum CollisionShapeType
        {
            // dynamic
            BoxShape = 0,
            SphereShape = 1,
            CapsuleShape = 2,
            CylinderShape = 3,
            ConeShape = 4,
            ConvexHull = 5,
            CompoundShape = 6,

            // static
            BvhTriangleMeshShape = 7,
            StaticPlaneShape = 8,
        };

        protected CollisionShape collisionShapePtr = null;
        public bool drawGizmo = true;


        public override void Dispose()
        {
            base.Dispose();
            if (collisionShapePtr != null)
            {
                collisionShapePtr.Dispose();
                collisionShapePtr = null;
            }
        }

        public abstract void OnDrawGizmosSelected();

        public abstract CollisionShape CopyCollisionShape();

        public abstract CollisionShape GetCollisionShape();

        protected Vector3 m_localScaling = Vector3.one;
        public Vector3 LocalScaling
        {
            get
            {
                if (collisionShapePtr != null)
                {
                    return collisionShapePtr.LocalScaling.ToUnity();
                }
                else
                {
                    return m_localScaling;
                }
            }
            set
            {
                m_localScaling = value;
                if (collisionShapePtr != null)
                {
                    collisionShapePtr.LocalScaling = value.ToBullet();
                }
            }
        }

        protected float m_Margin = 0.04f;
        public float Margin
        {
            get
            {
                if (collisionShapePtr != null)
                {
                    return collisionShapePtr.Margin;
                }
                else
                {
                    return m_Margin;
                }
            }
            set
            {
                m_Margin = value;
                if (collisionShapePtr != null)
                {
                    collisionShapePtr.Margin = value;
                }
            }
        }
    }
}


