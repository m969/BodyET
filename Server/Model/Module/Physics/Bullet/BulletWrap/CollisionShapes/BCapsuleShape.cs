using System;
using System.Collections;
using BulletSharp;
using ETModel;
using UnityEngine;

namespace ETModel 
{

    [ObjectSystem]
    public class BCapsuleShapeComponentAwakeSystem : AwakeSystem<BCapsuleShape,float,float,int>
    {
        public override void Awake(BCapsuleShape self,float radius,float height,int upaxis)
        {
            self.GetParent<Unit>().GetComponent<BCollisionShape>().CopyCollisionShape= self.thisCopyCollisionShape();
            self.GetParent<Unit>().GetComponent<BCollisionShape>().GetCollisionShape= self.thisGetCollisionShape();
            self.Radius = radius;
            self.Height = height;
            if (upaxis == 0)
            {
                self.UpAxis = BCapsuleShape.CapsuleAxis.x;
            }
            else if(upaxis == 1)
            {
                self.UpAxis = BCapsuleShape.CapsuleAxis.y;  
            }
 
            else if (upaxis == 2)
            {
                self.UpAxis = BCapsuleShape.CapsuleAxis.z;
            }
        }
    }
    public class BCapsuleShape : BCollisionShape
    {
        public enum CapsuleAxis
        {
            x,
            y,
            z
        }

        protected float radius = 1f;
        public float Radius
        {
            get { return radius; }
            set
            {
                if (collisionShapePtr != null && value != radius)
                {
                    Log.Warning("Cannot change the radius after the bullet shape has been created. Radius is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else 
                {
                    radius = value;
                }
            }
        }

        protected float height = 2f;
        public float Height
        {
            get { return height; }
            set
            {
                if (collisionShapePtr != null && value != height)
                {
                    Log.Warning("Cannot change the height after the bullet shape has been created. Height is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else 
                {
                    height = value;
                }
            }
        }

        protected CapsuleAxis upAxis = CapsuleAxis.y;
        public CapsuleAxis UpAxis
        {
            get { return upAxis; }
            set
            {
                if (collisionShapePtr != null && value != upAxis)
                {
                    Log.Warning("Cannot change the upAxis after the bullet shape has been created. upAxis is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else 
                {
                    upAxis = value;
                }
            }
        }

        protected Vector3 m_localScaling = Vector3.one;
        public Vector3 LocalScaling
        {
            get { return m_localScaling; }
            set
            {
                m_localScaling = value;
                if (collisionShapePtr != null)
                {
                    ((CapsuleShape)collisionShapePtr).LocalScaling = value.ToBullet();
                }
            }
        }

        public override void OnDrawGizmosSelected() 
        {
            if (drawGizmo == false)
            {
                return;
            }
            Vector3 position = this.GetParent<Unit>().Position;
            Quaternion rotation = this.GetParent<Unit>().Quaternion;
            Vector3 scale = m_localScaling;
            if (upAxis == CapsuleAxis.x)
            {
                rotation = new Quaternion(90, new Vector3(0,0,1)) * rotation;
            } 
            else if (upAxis == CapsuleAxis.z)
            {
                rotation = new Quaternion(90, new Vector3(1,0,0)) * rotation;
            }
            // BUtility.DebugDrawCapsule(position, rotation, scale, radius, height / 2f, 1, Gizmos.color);
        }

        CapsuleShape _CreateCapsuleShape()
        {
            CapsuleShape cs = null;
            if (upAxis == CapsuleAxis.x)
            {
                cs = new CapsuleShapeX(radius, height);
            }
            else if (upAxis == CapsuleAxis.y)
            {
                cs = new CapsuleShape(radius, height);
            }
            else if (upAxis == CapsuleAxis.z)
            {
                cs = new CapsuleShapeZ(radius, height);
            }
            else
            {
                Log.Warning("invalid axis value");
            }
            cs.LocalScaling = m_localScaling.ToBullet();
            return cs;
        }

        public CollisionShape thisCopyCollisionShape()
        {
            return _CreateCapsuleShape();
        }

        public CollisionShape thisGetCollisionShape() 
        {
            if (collisionShapePtr == null) 
            {
                collisionShapePtr = _CreateCapsuleShape();
            }
            return collisionShapePtr;
        }
    }
}
