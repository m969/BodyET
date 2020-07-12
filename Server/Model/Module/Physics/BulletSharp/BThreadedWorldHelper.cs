using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// Threading bullet allows to run bullet simulation steps at a higher frequency than Unity.
    /// When not threaded, bullet will interpolate automatically the transforms but the action callbacks won't be called regularly
    /// </summary>
    public class BThreadedWorldHelper : BasePhysicsWorldHelper
    {
        public MicroTimer microTimer;

        Stopwatch stopwatch;


        private double LastStepTime = 0;
        private double lastStartTime = 0;
        private long lastTime = 0;

        public double[] _simulationTime;
        public double[] _meanStepTime;
        public double[] _deltaTime;

        public int nbData;

        public bool ShowGraphs;

        private int timeDataId;

        private long currentTime = 0;

        private int lastFramCount;
        private float timeStep;
        private int maxStep;
        private int nbFrames;

        /// <summary>
        /// Total simulation time [in s]
        /// </summary>
        public double TotalSimulationTime
        {
            get
            {
                return stopwatch == null ? -1 : stopwatch.Elapsed.TotalMilliseconds / 1000f;
            }
        }

        public void Start()
        {
            nbData = (int)(5 / FixedTimeStep);
            _simulationTime = new double[nbData];
            _meanStepTime = new double[nbData];
            _deltaTime = new double[nbData];
            DelayThreadStart().Coroutine();
        }

        public async ETTask DelayThreadStart()
        {
            await TimerComponent.Instance.WaitAsync(1000);
            stopwatch = new Stopwatch();
            stopwatch.Start();

            microTimer = new MicroTimer();
            microTimer.MicroTimerElapsed += new MicroTimer.MicroTimerElapsedEventHandler(OnTimedEvent);
            microTimer.Interval = (long)(FixedTimeStep * 1000 * 1000);
            microTimer.Enabled = true;
        }

        public void OnTimedEvent(object sender,
                              MicroTimerEventArgs timerEventArgs)
        {
            currentTime = timerEventArgs.ElapsedMicroseconds;
            PhysicsUpdate((currentTime - lastTime) / (1000f * 1000f));
            lastTime = currentTime;
        }

        public void PhysicsUpdate(double deltaTime)
        {
            if (m_ddWorld != null)
            {
                lock (m_ddWorld)
                {
                    //Log.Debug("PhysicsUpdate");
                    lastFramCount = m__frameCount;
                    lastStartTime = TotalSimulationTime;
                    timeStep = (float)deltaTime * TimeStepRatio;
                    maxStep = Mathf.CeilToInt(timeStep / FixedTimeStep);
                    m__frameCount += m_ddWorld.StepSimulation(timeStep, maxStep < 1 ? 1 : maxStep, FixedTimeStep);
                    if (m_collisionEventHandler != null)
                    {
                        m_collisionEventHandler.OnPhysicsStep(m_world);
                    }
                    if (ShowGraphs)
                    {
                        LastStepTime = TotalSimulationTime - lastStartTime;
                        _simulationTime[timeDataId] = LastStepTime;
                        nbFrames = (m__frameCount - lastFramCount);
                        if (nbFrames == 0)
                            _meanStepTime[timeDataId] = 0;
                        else
                            _meanStepTime[timeDataId] = LastStepTime / (float)nbFrames;
                        _deltaTime[timeDataId] = deltaTime;
                        timeDataId = (timeDataId + 1) % nbData;
                    }
                }
            }
        }

        public void OnApplicationQuit()
        {
            if (microTimer != null)
                microTimer.Stop();
        }

        public void OnDestroy()
        {
            if (microTimer != null)
                microTimer.Stop();
        }

    }

}
