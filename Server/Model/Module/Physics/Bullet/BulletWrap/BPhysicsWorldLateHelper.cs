using System;
using System.Collections;
using System.Diagnostics;
using BulletSharp;

namespace ETModel
{
    [ObjectSystem]
    public class BPhysicsWorldLateHelperAwakeSystem : AwakeSystem<BPhysicsWorldLateHelper>
    {
        public override void Awake(BPhysicsWorldLateHelper self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
    public class BPhysicsWorldLateHelperFixedUpdateSystem : FixedUpdateSystem<BPhysicsWorldLateHelper>
    {
        public override void FixedUpdate(BPhysicsWorldLateHelper self)
        {
            self.FixedUpdate();
        }
    }
    [ObjectSystem]
    public class BPhysicsWorldLateHelperUpdateSystem : UpdateSystem<BPhysicsWorldLateHelper>
    {
        public override void Update(BPhysicsWorldLateHelper self)
        {
            self.Update();
        }
    }

    /**
    This script is last in the script execution order. Its purpose is to ensure that StepSimulation is called after other scripts LateUpdate calls
    Do not add this script manually. The BPhysicsWorld will add it.
    */
    public class BPhysicsWorldLateHelper : Entity
    {
        internal BPhysicsWorld m_physicsWorld;
        internal BDefaultCollisionHandler m_collisionEventHandler = new BDefaultCollisionHandler();
        public void RegisterCollisionCallbackListener(BCollisionObject.BICollisionCallbackEventHandler toBeAdded)
        {
            if (m_collisionEventHandler != null) m_collisionEventHandler.RegisterCollisionCallbackListener(toBeAdded);
        }

        public void DeregisterCollisionCallbackListener(BCollisionObject.BICollisionCallbackEventHandler toBeRemoved)
        {
            if (m_collisionEventHandler != null) m_collisionEventHandler.DeregisterCollisionCallbackListener(toBeRemoved);
        }

        internal DiscreteDynamicsWorld m_ddWorld;
        internal CollisionWorld m_world;
        internal int m__frameCount = 0;
        internal float m_fixedTimeStep = 1f / 60f;
        
        internal long m_lastSimulationStepTime;
        
        internal float m_elapsedBetweenFixedFrames = 0;
        
        private float needTime = 0.001f; //目前这个needTime=1,物体在受到跟重力大小相等，方向相反的力的作用下可以静止不动。

        int numUpdates = 0;
        public void Awake()
        {
            this.m_lastSimulationStepTime = TimeHelper.Now();
        }

        public virtual void FixedUpdate()
        {
            if (m_ddWorld != null)
            {
                float deltaTime = m_fixedTimeStep - m_elapsedBetweenFixedFrames;
                int numSteps = m_ddWorld.StepSimulation(deltaTime, 1, m_fixedTimeStep);
                m__frameCount += numSteps;
                m_lastSimulationStepTime = TimeHelper.Now();
                m_elapsedBetweenFixedFrames = 0f;
                numUpdates = 0;
            }
            //collisions
            if (m_collisionEventHandler != null)
            {
                m_collisionEventHandler.OnPhysicsStep(m_world);
            }
        }
        //This is needed for rigidBody interpolation. The motion states will update the positions of the rigidbodies
        public virtual void Update()
        {
            float deltaTime = (TimeHelper.Now() - m_lastSimulationStepTime)*1f/1000;
            if (deltaTime > this.needTime && (m_elapsedBetweenFixedFrames + deltaTime) < m_fixedTimeStep)
            {
                m_elapsedBetweenFixedFrames += deltaTime;
                int numSteps = m_ddWorld.StepSimulation(deltaTime, 1, m_fixedTimeStep);
                m__frameCount += numSteps;
                this.m_lastSimulationStepTime = TimeHelper.Now();
                numUpdates++;
            }
        }
    }
}
