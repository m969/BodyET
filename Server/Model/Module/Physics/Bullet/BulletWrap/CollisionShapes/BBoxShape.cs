using System;
using System.Collections;
using BulletSharp;
using ETModel;
using UnityEngine;

namespace ETModel 
{
    [ObjectSystem]
    public class BBoxShapeComponentAwakeSystem : AwakeSystem<BBoxShape,Vector3,Vector3>
    {
        public override void Awake(BBoxShape self,Vector3 extents,Vector3 localscale)
        {
            self.Extents = extents;
            self.GetParent<Unit>().GetComponent<BCollisionShape>().baseBCollisionShape = self;
            self.GetParent<Unit>().GetComponent<BCollisionShape>().CopyCollisionShape= self.thisCopyCollisionShape();
            self.GetParent<Unit>().GetComponent<BCollisionShape>().GetCollisionShape= self.thisGetCollisionShape();
            self.LocalScaling = localscale;
        }
    }
    public class BBoxShape : BCollisionShape 
    {
        protected Vector3 extents = Vector3.one;
        public Vector3 Extents
        {
            get { return extents; }
            set
            {
                if (collisionShapePtr != null && value != extents)
                {
                    Log.Warning("Cannot change the extents after the bullet shape has been created. Extents is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                } else 
                {
                    extents = value;
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
                    ((BoxShape)collisionShapePtr).LocalScaling = value.ToBullet();
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
            // BUtility.DebugDrawBox(position, rotation, scale, extents, Color.yellow);
        }

        public CollisionShape thisCopyCollisionShape()
        {
            BoxShape bs = new BoxShape(extents.ToBullet());
            bs.LocalScaling = m_localScaling.ToBullet();
            return bs;
        }

        public CollisionShape thisGetCollisionShape() 
        {
            if (collisionShapePtr == null) 
            {
                collisionShapePtr = new BoxShape(extents.ToBullet());
                ((BoxShape)collisionShapePtr).LocalScaling = m_localScaling.ToBullet();
            }
            return collisionShapePtr;
        }
    }
}
