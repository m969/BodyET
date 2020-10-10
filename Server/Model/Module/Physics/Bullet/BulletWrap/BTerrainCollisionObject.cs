using System;
using System.Collections;
using System.Diagnostics;
using BulletSharp;
using UnityEngine;

namespace ETModel
{
    /* 
    Custom verson of the collision object for handling heightfields to deal with some issues matching terrains to heighfields
    1) Unity heitfiels have pivot at corner. Bullet heightfields have pivot at center
    2) Can't rotate unity heightfields        
    */
    public class BTerrainCollisionObject : BCollisionObject
    {
        // TerrainData td;

        public override void Awake()
        {
            // base.Awake();
            // Terrain t =this.GetParent<Unit>().GetComponent<Terrain>();
            // if (t != null)
            // {
            //     td = t.terrainData;
            // }
        }

        //called by Physics World just before rigid body is added to world.
        //the current rigid body properties are used to rebuild the rigid body.
        internal override bool _BuildCollisionObject()
        {
            // if (td == null)
            // {
            //     Log.Warning("Must be attached to an object with a terrain ");
            //     return false;
            // }
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
            //     Log.Warning("The local scale on this collision shape is not one. Bullet physics does not support scaling on a rigid body world transform. Instead alter the dimensions of the CollisionShape.");
            // }

            m_collisionShape =this.GetParent<Unit>().GetComponent<BCollisionShape>();//这里将不能用mono的方式获取，就直接赋值了!
            if (m_collisionShape == null)
            {
                Log.Warning("There was no collision shape component attached to this BRigidBody. ");
                return false;
            }
            // if (!(m_collisionShape is BHeightfieldTerrainShape))
            // {
            //     Log.Warning("The collision shape needs to be a BHeightfieldTerrainShape. ");
            //     return false;
            // }

            CollisionShape cs = m_collisionShape.GetCollisionShape;
            //rigidbody is dynamic if and only if mass is non zero, otherwise static

            if (m_collisionObject == null)
            {
                m_collisionObject = new CollisionObject();
                m_collisionObject.CollisionShape = cs;
                m_collisionObject.UserObject = this;

                BulletSharp.Math.Matrix worldTrans = BulletSharp.Math.Matrix.Identity;
                // Vector3 pos = this.GetParent<Unit>().Position + new Vector3(td.size.x * .5f, td.size.y * .5f, td.size.z * .5f);
                Vector3 pos = this.GetParent<Unit>().Position;
                worldTrans.Origin = pos.ToBullet();
                m_collisionObject.WorldTransform = worldTrans;
                m_collisionObject.CollisionFlags = m_collisionFlags;
            }
            else {
                m_collisionObject.CollisionShape = cs;
                BulletSharp.Math.Matrix worldTrans = BulletSharp.Math.Matrix.Identity;
                // Vector3 pos = this.GetParent<Unit>().Position + new Vector3(td.size.x * .5f, td.size.y * .5f, td.size.z * .5f);
                Vector3 pos = this.GetParent<Unit>().Position;
                worldTrans.Origin = pos.ToBullet();
                m_collisionObject.WorldTransform = worldTrans;
                m_collisionObject.CollisionFlags = m_collisionFlags;
            }
            return true;
        }

        public override void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            if (isInWorld)
            {
                BulletSharp.Math.Matrix newTrans = m_collisionObject.WorldTransform;
                newTrans.Origin = this.GetParent<Unit>().Position.ToBullet();
                m_collisionObject.WorldTransform = newTrans;
                this.GetParent<Unit>().Position = position;
                this.GetParent<Unit>().Quaternion = rotation;
            } else
            {
                this.GetParent<Unit>().Position = position;
                this.GetParent<Unit>().Quaternion = rotation;
            }
        }

    }
}
