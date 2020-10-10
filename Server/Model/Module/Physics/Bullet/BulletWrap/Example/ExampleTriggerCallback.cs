using System.Collections;
using BulletSharp;
using ETModel;
using ETModel;

public class ExampleTriggerCallback : BGhostObject 
{
    [ObjectSystem]
    public class BGhostObjectAwakeSystem : AwakeSystem<ExampleTriggerCallback>
    {
        public override void Awake(ExampleTriggerCallback self)
        {
            self.GetParent<Unit>().GetComponent<BGhostObject>().collisionFlags = CollisionFlags.None;
            self.GetParent<Unit>().GetComponent<BGhostObject>().groupsIBelongTo = CollisionFilterGroups.DefaultFilter;
            self.GetParent<Unit>().GetComponent<BGhostObject>().collisionMask = CollisionFilterGroups.Everything;
        }
    }
    [ObjectSystem]
    public class BGhostObjectStartSystem : StartSystem<ExampleTriggerCallback>
    {
        public override void Start(ExampleTriggerCallback self)
        {
            self=(ExampleTriggerCallback)self.GetParent<Unit>().GetComponent<ExampleTriggerCallback>().tiggerBCollisionObject;
        }
    }
    public BCollisionObject tiggerBCollisionObject { get; set; }
    public override void BOnTriggerEnter(CollisionObject other, AlignedManifoldArray details)
    {
        Log.Debug("Enter with " + other.UserObject + " fixedFrame " + BPhysicsWorld.Get.frameCount);
    }

    public override void BOnTriggerStay(CollisionObject other, AlignedManifoldArray details)
    {
        Log.Debug("Stay with " + other.UserObject + " fixedFrame " + BPhysicsWorld.Get.frameCount);
    }

    public override void BOnTriggerExit(CollisionObject other)
    {
        Log.Debug("Exit with " + other.UserObject + " fixedFrame " + BPhysicsWorld.Get.frameCount);
    }
}
