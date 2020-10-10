using System;
using System.Collections;
using BulletSharp.Math;
using BulletSharp;
using ETModel;

namespace ETModel 
{
    [ObjectSystem]
    public class BCollisionShapeComponentAwakeSystem : AwakeSystem<BCollisionShape>
    {
        public override void Awake(BCollisionShape self)
        {
            self = self.baseBCollisionShape;
        }
    }
    [ObjectSystem]
    public class BCollisionShapeComponentStartSystem : StartSystem<BCollisionShape>
    {
        public override void Start(BCollisionShape self)
        {
            self = self.baseBCollisionShape;
        }
    }
    [System.Serializable]
    public  class BCollisionShape : Entity, IDisposable
    {
        public BCollisionShape baseBCollisionShape { get; set; }
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

        void OnDestroy() 
        {
            Dispose(false);
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isdisposing) 
        {
            if (collisionShapePtr != null) {
                collisionShapePtr.Dispose();
                collisionShapePtr = null;
            }
        }

        public virtual void OnDrawGizmosSelected()
        {
            
        }
        public CollisionShape CopyCollisionShape { get; set; }
        public CollisionShape GetCollisionShape { get; set; }
    }
    
}


