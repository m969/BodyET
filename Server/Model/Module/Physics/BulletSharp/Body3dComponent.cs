using BulletSharp;
using ETModel;
using UnityEngine;
using System.Collections.Generic;

namespace ETModel
{
	[ObjectSystem]
	public class Body3dComponentAwakeSystem : AwakeSystem<Body3dComponent>
	{
		public override void Awake(Body3dComponent self)
		{
			self.Awake();
		}
	}

	[ObjectSystem]
	public class Body3dComponentUpdateSystem : UpdateSystem<Body3dComponent>
	{
		public override void Update(Body3dComponent self)
		{
			self.Update();
		}
	}

	public class Body3dComponent : Entity, BCollisionObject.BICollisionCallbackEventHandler
	{
		public System.Action<Body3dComponent> OnBeginContactAction { get; set; }
		public BRigidBody Body { get; set; }
		public Vector3 Position
		{
			get
			{
                return (GetParent<Entity>() as ITransform).Position;
			}
			set
			{
				(GetParent<Entity>() as ITransform).Position = new Vector3(value.x, value.y, value.z);
			}
		}

		public float Angle
		{
			get
			{
				return (GetParent<Entity>() as ITransform).Angle;
			}
		}

		public BulletSharpWorldComponent WorldComponent
		{
			get
			{
				return Parent.Domain.GetComponent<BulletSharpWorldComponent>();
			}
		}

		public BPhysicsWorld World
		{
			get
			{
				return WorldComponent.World;
			}
		}

		public void Awake()
		{
			CreateBody(1, 1);
            //TryToGetCollisionObject();
        }

        public Body3dComponent CreateBody(float hx, float hy)
		{
            try
            {
                this.AddComponent<BRigidBody>();
                this.AddComponent<BBoxShape>();
                Body = GetComponent<BRigidBody>();
                Body.Start();
                Body.AddOnCollisionCallbackEventHandler(this);
                myCollisionObject = Body.GetCollisionObject();
            }
            catch (System.Exception e)
            {
                Log.Error(e);
            }
			return this;
        }

		private Vector3 lastPosition { get; set; }
		public void Update()
		{
			if (Position != lastPosition)
			{
				//Log.Debug("Position = " + Position.ToString());
				lastPosition = Position;
			}
			this.Body.SetPositionAndRotation(new Vector3(Position.x, Position.y, Position.z), Quaternion.identity);
		}

		public override void Dispose()
		{
            Body.RemoveOnCollisionCallbackEventHandler(this);
			World.RemoveRigidBody(Body.RigidBody);
            base.Dispose();
		}

        public class PersistentManifoldList
        {
            public List<PersistentManifold> manifolds = new List<PersistentManifold>();
        }

        CollisionObject myCollisionObject;
        Dictionary<CollisionObject, PersistentManifoldList> otherObjs2ManifoldMap = new Dictionary<CollisionObject, PersistentManifoldList>();
        List<PersistentManifoldList> newContacts = new List<PersistentManifoldList>();
        List<CollisionObject> objectsToRemove = new List<CollisionObject>();
        private void TryToGetCollisionObject()
        {
            try
            {
                //CollisionObject collisionObject;
                //BCollisionObject co = GetComponent<BCollisionObject>();
                //if (co == null)
                //{
                //    //Log.Error("BCollisionCallbacksDefault must be attached to an object with a BCollisionObject", this);
                //    //yield break;
                //}
                //collisionObject = co.GetCollisionObject();
                var collisionObject = Body.GetCollisionObject();
                if (collisionObject == null)
                {
                    //if (BPhysicsWorld.Get().debugType >= BulletUnity.Debugging.BDebug.DebugType.Debug)
                    Log.Debug($"Could not recover the collision object from the current BCollisionObject. Will try again. {this}");
                    float currentTime = TimeHelper.Now() / 1000f;
                    while (collisionObject == null && (TimeHelper.Now() / 1000f - currentTime) < 5f)
                    {
                        collisionObject = Body.GetCollisionObject();
                        currentTime = TimeHelper.Now() / 1000f;
                    }
                }
                myCollisionObject = collisionObject;
            }
            catch (System.Exception e)
            {
                Log.Error(e);
            }
        }

        public void OnVisitPersistentManifold(PersistentManifold pm)
        {
            if (myCollisionObject == null)
                return;

            CollisionObject other;
            if (pm.NumContacts > 0)
            {

                if (pm.Body0 == myCollisionObject)
                {
                    other = pm.Body1;
                }
                else
                {
                    other = pm.Body0;
                }
                PersistentManifoldList pml;
                if (!otherObjs2ManifoldMap.TryGetValue(other, out pml))
                {
                    //todo get PersistentManifoldList from object pool
                    //this is first contact with this other object
                    //might have multiple new contacts with same object stored in separate persistent manifolds
                    //don't add two different lists to new contacts
                    bool foundExisting = false;
                    for (int i = 0; i < newContacts.Count; i++)
                    {
                        if (newContacts[i].manifolds[0].Body0 == other ||
                            newContacts[i].manifolds[0].Body1 == other)
                        {
                            foundExisting = true;
                            newContacts[i].manifolds.Add(pm);
                        }
                    }
                    if (!foundExisting)
                    {
                        pml = new PersistentManifoldList();
                        newContacts.Add(pml);
                        pml.manifolds.Add(pm);
                        //don't add to otherObjs2ManifoldMap here. It messes up onStay do it after all pm's have been visited.
                    }
                }
                else
                {
                    pml.manifolds.Add(pm);
                }
            }
        }

        public void OnFinishedVisitingManifolds()
        {
            if (myCollisionObject == null)
                return;

            objectsToRemove.Clear();
            foreach (CollisionObject co in otherObjs2ManifoldMap.Keys)
            {
                PersistentManifoldList pml = otherObjs2ManifoldMap[co];
                if (pml.manifolds.Count > 0)
                {
                    BOnCollisionStay(co, pml);
                }
                else
                {
                    BOnCollisionExit(co);
                    objectsToRemove.Add(co);
                }
            }

            for (int i = 0; i < objectsToRemove.Count; i++)
            {
                otherObjs2ManifoldMap.Remove(objectsToRemove[i]);
            }
            objectsToRemove.Clear();


            for (int i = 0; i < newContacts.Count; i++)
            {
                PersistentManifoldList pml = newContacts[i];
                CollisionObject other;
                if (pml.manifolds[0].Body0 == myCollisionObject)
                {
                    other = pml.manifolds[0].Body1;
                }
                else
                {
                    other = pml.manifolds[0].Body0;
                }
                otherObjs2ManifoldMap.Add(other, pml);
                BOnCollisionEnter(other, pml);
            }
            newContacts.Clear();

            foreach (CollisionObject co in otherObjs2ManifoldMap.Keys)
            {
                PersistentManifoldList pml = otherObjs2ManifoldMap[co];
                pml.manifolds.Clear();
            }
        }

        public virtual void BOnCollisionEnter(CollisionObject other, PersistentManifoldList manifoldList)
        {
            Log.Debug($"===================== BOnCollisionEnter {other} {manifoldList}");
        }

        public virtual void BOnCollisionStay(CollisionObject other, PersistentManifoldList manifoldList)
        {
            Log.Debug($"BOnCollisionStay {other} {manifoldList}");
        }

        public virtual void BOnCollisionExit(CollisionObject other)
        {
            Log.Debug($"BOnCollisionExit {other}");
        }
    }
}