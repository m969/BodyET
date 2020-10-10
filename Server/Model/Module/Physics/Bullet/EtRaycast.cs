using System.Diagnostics;
using BulletSharp;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class EtRaycastAwakeSystem : AwakeSystem<EtRaycast>
    {
        public override void Awake(EtRaycast self)
        {

        }
    }
    [ObjectSystem]
    public class EtRaycastUpdateSystem : UpdateSystem<EtRaycast>
    {
        public override void Update(EtRaycast self)
        {
            self.Update();
        }
    }
    public class EtRaycast : Entity
    {
        private int setrefresh = 0;

        public void Update()
        {
            setrefresh++;
            if (setrefresh % 10 == 0)
            {
                BulletSharp.Math.Vector3 fromP = this.GetParent<Unit>().Position.ToBullet();
                BulletSharp.Math.Vector3 toP = (this.GetParent<Unit>().Position + Vector3.down * 10f).ToBullet();
                ClosestRayResultCallback callback = new ClosestRayResultCallback(ref fromP, ref toP);
                BPhysicsWorld world = BPhysicsWorld.Get;
                world.world.RayTest(fromP, toP, callback);
                if (callback.HasHit && callback.CollisionObject.UserObject is Unit unit )
                {
                    Log.Debug("fromp=" + fromP + "||top=" + toP);
                    Log.Debug("Hit p" + callback.HitPointWorld + "N=" + callback.HitNormalWorld + "obj=" + callback.CollisionObject.UserObject);
                    Unit x = unit;
                }

                if (setrefresh > 10000)
                {
                    setrefresh = 0;
                }
            }
        }
    }
}