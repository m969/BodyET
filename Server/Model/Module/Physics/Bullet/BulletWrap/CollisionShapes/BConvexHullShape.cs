using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BulletSharp;
using ETModel;

namespace ETModel {
    [ObjectSystem]
    public class BConvexHullShapeComponentAwakeSystem : AwakeSystem<BConvexHullShape,string>
    {
        public override void Awake(BConvexHullShape self,string map)
        {
            // 读取地图数据
            self.verts = BulletJsonDeserializeHelper.Load(map);
            self.GetParent<Unit>().GetComponent<BCollisionShape>().baseBCollisionShape = self;
            self.GetParent<Unit>().GetComponent<BCollisionShape>().CopyCollisionShape= self.thisCopyCollisionShape();
            self.GetParent<Unit>().GetComponent<BCollisionShape>().GetCollisionShape= self.thisGetCollisionShape();
        }
    }
    public class BConvexHullShape : BCollisionShape 
    {

        protected Vector3 m_localScaling = Vector3.one;
        public Vector3 LocalScaling
        {
            get { return m_localScaling; }
            set
            {
                m_localScaling = value;
                if (collisionShapePtr != null)
                {
                    ((ConvexHullShape) collisionShapePtr).LocalScaling = value.ToBullet();
                }
            }
        }

        //todo draw the hull when not in the world
        public override void OnDrawGizmosSelected() 
        {
              
        }

        ConvexHullShape _CreateConvexHullShape()
        {
            // Vector3[] verts = hullMesh.vertices;
            //todo remove duplicate verts
            //todo use vertex reduction utility
            float[] points = new float[verts.Count * 3];
            for (int i = 0; i < verts.Count; i++)
            {
                int idx = i * 3;
                points[idx] = verts[i].x;
                points[idx + 1] = verts[i].y;
                points[idx + 2] = verts[i].z;
            }
            ConvexHullShape cs = new ConvexHullShape(points);
            cs.LocalScaling = m_localScaling.ToBullet();
            return cs;
        }

        public CollisionShape thisCopyCollisionShape()
        {
            return _CreateConvexHullShape();
        }

        public CollisionShape thisGetCollisionShape() 
        {
            if (collisionShapePtr == null) 
            {
                collisionShapePtr = _CreateConvexHullShape();
            }
            return collisionShapePtr;
        }

        public List<Vector3> verts { get; set; }

    }
}
