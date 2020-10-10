using System;
using UnityEngine;
using System.Collections;
using System.Threading;
using BulletSharp;

namespace ETModel
{
    [ObjectSystem]
    public class BCharacterControllerAwakeSystem : AwakeSystem<BCharacterController>
    {
        public override void Awake(BCharacterController self)
        {
            self.GetParent<Unit>().GetComponent<BCollisionObject>().collisionFlags = CollisionFlags.CharacterObject;
            self.GetParent<Unit>().GetComponent<BCollisionObject>().groupsIBelongTo = CollisionFilterGroups.DefaultFilter;
            self.GetParent<Unit>().GetComponent<BCollisionObject>().collisionMask = CollisionFilterGroups.Everything;
            self.GetParent<Unit>().GetComponent<BCollisionObject>().realyBCollisionObject = self;
            self.GetParent<Unit>().GetComponent<ExampleTriggerCallback>().tiggerBCollisionObject = self;
        }
    }
    [ObjectSystem]
    public class BCharacterControllerFixedUpdateSystem : FixedUpdateSystem<BCharacterController>
    {
        public override void FixedUpdate(BCharacterController self)
        {
            self.FixedUpdate();
        }
    }
    public class BCharacterController : BPairCachingGhostObject
    {

        public CancellationTokenSource CancellationTokenSource;
        KinematicCharacterController m_characterController;
        public float stepHeight = 1f;
        public int upAxis = 1; //0=x, 1=y, 2=z


        public KinematicCharacterController GetKinematicCharacterController()
        {
            return m_characterController;
        }

        public override void Awake()
        {
            base.Awake();
            m_collisionFlags = BulletSharp.CollisionFlags.CharacterObject;
        }

        internal override bool _BuildCollisionObject()
        {
            BPhysicsWorld world = BPhysicsWorld.Get;
            if (m_collisionObject != null)
            {
                if (isInWorld && world != null)
                {
                    isInWorld = false;
                    world.RemoveCollisionObject(this);
                }
            }

            // if (transform.localScale != UnityEngine.Vector3.one)
            // {
            //     Log.Error("The local scale on this collision shape is not one. Bullet physics does not support scaling on a rigid body world transform. Instead alter the dimensions of the CollisionShape.");
            // }

            m_collisionShape =this.GetParent<Unit>().GetComponent<BCollisionShape>();
            if (m_collisionShape == null)
            {
                Log.Error("There was no collision shape component attached to this BRigidBody. " );
                return false;
            }
            if (!(m_collisionShape.GetCollisionShape is ConvexShape))
            {
                Log.Error("The CollisionShape on this BCharacterController was not a convex shape. " );
                return false;
            }

            if (m_collisionObject == null)
            {
                m_collisionObject = new PairCachingGhostObject();
                m_collisionObject.CollisionShape = m_collisionShape.GetCollisionShape;
                m_collisionObject.CollisionFlags = m_collisionFlags;
                m_characterController = new KinematicCharacterController((PairCachingGhostObject)m_collisionObject, (ConvexShape)m_collisionShape.GetCollisionShape, stepHeight, upAxis);
                BulletSharp.Math.Matrix worldTrans;
                BulletSharp.Math.Quaternion q = this.GetParent<Unit>().Quaternion.ToBullet();
                BulletSharp.Math.Matrix.RotationQuaternion(ref q, out worldTrans);
                worldTrans.Origin = this.GetParent<Unit>().Position.ToBullet();
                m_collisionObject.WorldTransform = worldTrans;
                m_collisionObject.UserObject = this;
                //world.world.AddCollisionObject(m_collisionObject, CollisionFilterGroups.CharacterFilter, CollisionFilterGroups.StaticFilter | CollisionFilterGroups.DefaultFilter);
                //((DynamicsWorld)world.world).AddAction(m_characterController);
            }
            else 
            {
                m_collisionObject.CollisionShape = m_collisionShape.GetCollisionShape;
                m_collisionObject.CollisionFlags = m_collisionFlags;
                if (m_characterController != null)
                {
                    world.RemoveAction(m_characterController);
                }
                m_characterController = new KinematicCharacterController((PairCachingGhostObject)m_collisionObject, (ConvexShape)m_collisionShape.GetCollisionShape, stepHeight, upAxis);
                BulletSharp.Math.Matrix worldTrans;
                BulletSharp.Math.Quaternion q = this.GetParent<Unit>().Quaternion.ToBullet();
                BulletSharp.Math.Matrix.RotationQuaternion(ref q, out worldTrans);
                worldTrans.Origin = this.GetParent<Unit>().Position.ToBullet();
                m_collisionObject.WorldTransform = worldTrans;
                m_collisionObject.UserObject = this;
            }
            return true;
        }

        public void Move(Vector3 displacement,bool canmove = false)
        {
            this.GetParent<Unit>().needSend = canmove;
            m_characterController.SetWalkDirection(displacement.ToBullet());
        }

        public void Jump()
        {
            m_characterController.Jump();
        }

        public void Rotate(float turnAmount,bool needrotate = false)
        {
            this.GetParent<Unit>().needSend = needrotate;
            BulletSharp.Math.Matrix xform = m_collisionObject.WorldTransform;
            BulletSharp.Math.Matrix orn = xform;
            BulletSharp.Math.Vector3 pos = xform.Origin;
            orn.Row4 = new BulletSharp.Math.Vector4(0, 0, 0, 1);
            BulletSharp.Math.Vector3 upDir = new BulletSharp.Math.Vector3(xform.M21, xform.M22, xform.M23);
            orn *= BulletSharp.Math.Matrix.RotationAxis(upDir, turnAmount);
            orn.Row4 = new BulletSharp.Math.Vector4(pos.X, pos.Y, pos.Z, 1);
            m_collisionObject.WorldTransform = orn;
        }

        public void FixedUpdate()
        {
            if (this.CancellationTokenSource == null)
            {
                this.Move(Vector3.zero);
                this.Rotate(0);
            }
            BulletSharp.Math.Matrix trans;
            m_collisionObject.GetWorldTransform(out trans);
            this.GetParent<Unit>().Position = BSExtensionMethods2.ExtractTranslationFromMatrix(ref trans);
            this.GetParent<Unit>().Quaternion = BSExtensionMethods2.ExtractRotationFromMatrix(ref trans);
            // transform.localScale = BSExtensionMethods2.ExtractScaleFromMatrix(ref trans);
        }
    }
}
