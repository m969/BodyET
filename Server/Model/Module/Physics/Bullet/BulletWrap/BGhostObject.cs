using System;
using System.Collections;
using System.Collections.Generic;
using BulletSharp;

namespace ETModel
{
    [ObjectSystem]
    public class BGhostObjectAwakeSystem : AwakeSystem<BGhostObject>
    {
        public override void Awake(BGhostObject self)
        {
            self.GetParent<Unit>().GetComponent<BCollisionObject>().collisionFlags = CollisionFlags.None;
            self.GetParent<Unit>().GetComponent<BCollisionObject>().groupsIBelongTo = CollisionFilterGroups.DefaultFilter;
            self.GetParent<Unit>().GetComponent<BCollisionObject>().collisionMask = CollisionFilterGroups.Everything;
            self.GetParent<Unit>().GetComponent<BCollisionObject>().realyBCollisionObject = self;
            self.GetParent<Unit>().GetComponent<ExampleTriggerCallback>().tiggerBCollisionObject = self;
        }
    }
    [ObjectSystem]
    public class BGhostObjectStartSystem : StartSystem<BGhostObject>
    {
        public override void Start(BGhostObject self)
        {
            self.Start();
        }
    }
    [ObjectSystem]
    public class BGhostObjecFixedUpdateSystem : FixedUpdateSystem<BGhostObject>
    {
        public override void FixedUpdate(BGhostObject self)
        {
            self.FixedUpdate();
        }
    }
    public class BGhostObject : BCollisionObject
    {
        private GhostObject m_ghostObject
        {
            get { return (GhostObject) m_collisionObject; }   
        }

        internal override bool _BuildCollisionObject()
        {
            BPhysicsWorld world = BPhysicsWorld.Get;
            if (m_collisionObject != null)
            {
                if (isInWorld && world != null)
                {
                    world.RemoveCollisionObject(this);
                }
            }
            m_collisionShape =this.GetParent<Unit>().GetComponent<BCollisionShape>();
            if (m_collisionShape == null)
            {
                Log.Warning("There was no collision shape component attached to this BRigidBody. " );
                return false;
            }

            CollisionShape cs = m_collisionShape.GetCollisionShape;
            //rigidbody is dynamic if and only if mass is non zero, otherwise static


            if (m_collisionObject == null)
            {
                m_collisionObject = new BulletSharp.GhostObject();
                m_collisionObject.CollisionShape = cs;
                BulletSharp.Math.Matrix worldTrans;
                BulletSharp.Math.Quaternion q = this.GetParent<Unit>().Quaternion.ToBullet();
                BulletSharp.Math.Matrix.RotationQuaternion(ref q, out worldTrans);
                worldTrans.Origin = this.GetParent<Unit>().Position.ToBullet();
                m_collisionObject.WorldTransform = worldTrans;
                m_collisionObject.UserObject = this;
                m_collisionObject.CollisionFlags = m_collisionObject.CollisionFlags | BulletSharp.CollisionFlags.NoContactResponse;
            }
            else {
                BulletSharp.Math.Matrix worldTrans;
                BulletSharp.Math.Quaternion q = this.GetParent<Unit>().Quaternion.ToBullet();
                BulletSharp.Math.Matrix.RotationQuaternion(ref q, out worldTrans);
                worldTrans.Origin = this.GetParent<Unit>().Position.ToBullet();
                m_collisionObject.WorldTransform = worldTrans;
                m_collisionObject.CollisionShape = cs;
                m_collisionObject.CollisionFlags = m_collisionObject.CollisionFlags | BulletSharp.CollisionFlags.NoContactResponse;
            }
            return true;
        }


        HashSet<CollisionObject> objsIWasInContactWithLastFrame = new HashSet<CollisionObject>();
        HashSet<CollisionObject> objsCurrentlyInContactWith = new HashSet<CollisionObject>();
        public void FixedUpdate()
        {
            //TODO should do two passes like with collisions
            objsCurrentlyInContactWith.Clear();
            for (int i = 0; i < m_ghostObject.NumOverlappingObjects; i++)
            {
                CollisionObject otherObj = m_ghostObject.GetOverlappingObject(i);
                objsCurrentlyInContactWith.Add(otherObj);
                if (!objsIWasInContactWithLastFrame.Contains(otherObj))
                {
                    BOnTriggerEnter(otherObj, null);
                } 
                else
                {
                    BOnTriggerStay(otherObj, null);
                }
            }
            objsIWasInContactWithLastFrame.ExceptWith(objsCurrentlyInContactWith);

            foreach(CollisionObject co in objsIWasInContactWithLastFrame)
            {
                BOnTriggerExit(co);
            }

            //swap the hashsets so objsIWasInContactWithLastFrame now contains the list of objs.
            HashSet<CollisionObject> temp = objsIWasInContactWithLastFrame;
            objsIWasInContactWithLastFrame = objsCurrentlyInContactWith;
            objsCurrentlyInContactWith = temp;
        }

        public virtual void BOnTriggerEnter(CollisionObject other, AlignedManifoldArray details)
        {
            Log.Debug("Enter with " + other.UserObject + " fixedFrame " + BPhysicsWorld.Get.frameCount);

            if (other.UserObject is Unit unit)
            {
                unit.needSend = true;
                unit.triggerType = "TriggerEnter";

            }
        }

        public virtual void BOnTriggerStay(CollisionObject other, AlignedManifoldArray details)
        {
            Log.Debug("Stay with " + other.UserObject.ToString() + " fixedFrame " + BPhysicsWorld.Get.frameCount);
            if (other.UserObject is Unit unit)
            {
                unit.needSend = true;
                unit.triggerType = "TriggerStay";
            }
        }

        public virtual void BOnTriggerExit(CollisionObject other)
        {
            Log.Debug("Exit with " + other.UserObject + " fixedFrame " + BPhysicsWorld.Get.frameCount);
            if (other.UserObject is Unit unit)
            {
                unit.needSend = true;
                unit.triggerType = "TriggerExit";
            }
        }
    }
}
