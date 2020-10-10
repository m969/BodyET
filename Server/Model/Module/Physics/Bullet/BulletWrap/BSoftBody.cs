using System.Collections;
using BulletSharp.SoftBody;
using System;
using BulletSharp;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

//using BulletSharp.Math;

namespace ETModel
{

    public class BSoftBody : BCollisionObject, IDisposable
    {
        //common Soft body settings class used for all softbodies, parameters set based on type of soft body
        
        private SBSettings _softBodySettings = new SBSettings();      //SoftBodyEditor will display this when needed
        public SBSettings SoftBodySettings
        {
            get { return _softBodySettings; }
            set { _softBodySettings = value; }
        }

        //protected SoftBody m_BSoftBody;

        SoftRigidDynamicsWorld _world;
        protected SoftRigidDynamicsWorld World
        {
            get {
                if (_world != null) {
                    return _world;
                } else {
                    BPhysicsWorld w = BPhysicsWorld.Get;
                    if (w == null) {
                        return null;
                    } else if (w.world is SoftRigidDynamicsWorld)
                    {
                        _world = (SoftRigidDynamicsWorld)w.world;
                        return _world;
                    } else
                    {
                        return null;
                    }
                }
            }
        }

        //for converting to/from unity mesh
        protected Vector3[] verts = new Vector3[0];
        protected Vector3[] norms = new Vector3[0];
        protected int[] tris = new int[1];

        public override void Awake()
        {
            //disable warning
        }

        protected override void AddObjectToBulletWorld()
        {
            BPhysicsWorld.Get.AddSoftBody(this);
        }

        protected override void RemoveObjectFromBulletWorld()
        {
            BPhysicsWorld world = BPhysicsWorld.Get;
            if (world !=null && isInWorld)
            {
                world.RemoveSoftBody((SoftBody)m_collisionObject);
            }
        }

        public void BuildSoftBody()
        {
            _BuildCollisionObject();
        }

        protected override void Dispose(bool isdisposing)
        {
            SoftBody m_BSoftBody = (SoftBody)m_collisionObject;
            if (isInWorld && isdisposing && m_BSoftBody != null)
            {
                if (m_BSoftBody != null)
                {
                    World.RemoveSoftBody(m_BSoftBody);
                }
            }
            if (m_BSoftBody != null)
            {
                m_BSoftBody.Dispose();
                m_BSoftBody = null;
            }
            // Log.Info("Destroying SoftBody " + name);
        }

        public void DumpDataFromBullet()
        {
            if (isInWorld)
            {
                SoftBody m_BSoftBody = (SoftBody)m_collisionObject;
                if (verts.Length != m_BSoftBody.Nodes.Count)
                {
                    verts = new Vector3[m_BSoftBody.Nodes.Count];
                }
                if (norms.Length != verts.Length)
                {
                    norms = new Vector3[m_BSoftBody.Nodes.Count];
                }
                for (int i = 0; i < m_BSoftBody.Nodes.Count; i++)
                {
                    verts[i] = m_BSoftBody.Nodes[i].Position.ToUnity();
                    norms[i] = m_BSoftBody.Nodes[i].Normal.ToUnity();
                }
            }
        }


        public virtual void Update()
        {
            DumpDataFromBullet();  //Get Bullet data
            UpdateMesh(); //Update mesh based on bullet data
            //Make coffee
        }

        /// <summary>
        /// Update Mesh (or line renderer) at runtime, call from Update 
        /// </summary>
        public virtual void UpdateMesh()
        {

        }





    }
}