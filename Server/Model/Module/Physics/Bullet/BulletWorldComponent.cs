using System.Collections.Generic;
using PF;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class BulletWorldComponentAwakeSystem : AwakeSystem<BulletWorldComponent>
    {
        public override void Awake(BulletWorldComponent self)
        {
            //客户端地面
            self.World = EntityFactory.Create<Unit, UnitType, Vector3, Quaternion>(self.Domain, UnitType.Item,new Vector3(0,-2f,0), 
                new Quaternion(0, 0, 0,0));
            self.World.AddComponent<BCollisionShape>();
            self.World.AddComponent<BConvexHullShape,string>("Map.obj");
            self.World.AddComponent<BCollisionObject>();
            self.World.AddComponent<BMeshCollisionObject>();
            
            //加碰撞检测
            self.trigger = EntityFactory.Create<Unit, UnitType, Vector3, Quaternion>(self.Domain, UnitType.Item,new Vector3(30,8,-10), 
                new Quaternion(0, 0, 0,0));
            self.trigger.AddComponent<BCollisionShape>();
            self.trigger.AddComponent<BBoxShape,Vector3,Vector3>(new Vector3(20,3,20),new Vector3(1,1,1) );
            self.trigger.AddComponent<BCollisionObject>();
            self.trigger.AddComponent<BGhostObject>();

            //运动物体
            self.box = EntityFactory.Create<Unit, UnitType, Vector3, Quaternion>(self.Domain, UnitType.Item,new Vector3(0,10,0), 
                new Quaternion(0, 10, 0,0));
            self.box.AddComponent<BCollisionObject>();
            self.box.AddComponent<BCollisionShape>();
            self.box.AddComponent<BBoxShape,Vector3,Vector3>(new Vector3(1,1,1),new Vector3(1,1,1) );
            self.box.AddComponent<BRigidBody>();
        }
    }
    
    public class BulletWorldComponent: Entity
    {
        public Unit World;
        public Unit trigger;
        public Unit box;

        public List<Vector3> worlvectors { get; set; }
    }
}