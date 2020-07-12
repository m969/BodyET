using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using NETCoreTest.Framework;
using BulletSharp;
using ETModel;

namespace ETModel
{
    [ObjectSystem]
    public class BulletSharpWorldComponentAwakeSystem : AwakeSystem<BulletSharpWorldComponent>
    {
        public override void Awake(BulletSharpWorldComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class BulletSharpWorldComponentUpdateSystem : UpdateSystem<BulletSharpWorldComponent>
    {
        public override void Update(BulletSharpWorldComponent self)
        {
            self.Update();
        }
    }

    public class BulletSharpWorldComponent : Entity
    {
        public BPhysicsWorld World;


        public void Awake()
        {
            World = BPhysicsWorld.Get();
        }

        public void Update()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public void Remove(BRigidBody body)
        {

        }

        private void Step(bool showProfile)
        {

        }
    }
}