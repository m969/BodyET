using BulletSharp;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class BBoxShapeAwakeSystem : AwakeSystem<BBoxShape>
    {
        public override void Awake(BBoxShape self)
        {
            //self.Awake();
        }
    }

    [ObjectSystem]
    public class BBoxShapeUpdateSystem : UpdateSystem<BBoxShape>
    {
        public override void Update(BBoxShape self)
        {
            //self.Update();
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
                    Log.Error("Cannot change the extents after the bullet shape has been created. Extents is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else
                {
                    extents = value;
                }
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (!drawGizmo)
            {
                return;
            }
            //Vector3 position = transform.position;
            //Quaternion rotation = transform.rotation;
            //Vector3 scale = m_localScaling;
            //Gizmos.color = Color.yellow;
            //BoxShape shape = GetCollisionShape() as BoxShape;
            //Gizmos.matrix = this.transform.localToWorldMatrix * Matrix4x4.Scale(transform.lossyScale).inverse;
            //for (int i = 0; i < shape.NumEdges; i++)
            //{
            //    BulletSharp.Math.Vector3 vertex1;
            //    BulletSharp.Math.Vector3 vertex2;
            //    shape.GetEdge(i, out vertex1, out vertex2);
            //    Vector3 vertexUnity1 = BSExtensionMethods2.ToUnity(vertex1);
            //    Vector3 vertexUnity2 = BSExtensionMethods2.ToUnity(vertex2);
            //    Gizmos.DrawLine(vertexUnity1, vertexUnity2);
            //}
        }

        public override CollisionShape CopyCollisionShape()
        {
            BoxShape bs = new BoxShape(extents.ToBullet());
            bs.LocalScaling = m_localScaling.ToBullet();
            bs.Margin = m_Margin;
            return bs;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new BoxShape(extents.ToBullet());
                ((BoxShape)collisionShapePtr).LocalScaling = m_localScaling.ToBullet();
                collisionShapePtr.Margin = m_Margin;
            }
            return collisionShapePtr;
        }
    }
}
