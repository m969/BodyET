using BulletSharp;
using BM = BulletSharp.Math;
using System;
using System.Diagnostics;
using UnityEngine;

namespace ETModel 
{
    public class BGameObjectMotionState : MotionState, IDisposable
    {
        private readonly Unit unit;
        private Stopwatch sw;       private int i = 0;

        public BGameObjectMotionState(Unit _unit,Vector3 t,Quaternion q)
        {
            this.unit = _unit;
            unit.Position = t;
            unit.Quaternion = q;
        }

		public delegate void GetTransformDelegate(out BM.Matrix worldTrans);
		public delegate void SetTransformDelegate(ref BM.Matrix m);

        //Bullet wants me to fill in worldTrans
        //This is called by bullet once when rigid body is added to the the world
        //For kinematic rigid bodies it is called every simulation step
		//[MonoPInvokeCallback(typeof(GetTransformDelegate))]
        public override void GetWorldTransform(out BM.Matrix worldTrans) 
        {
            BulletSharp.Math.Vector3 pos = unit.Position.ToBullet();
            BulletSharp.Math.Quaternion rot = unit.Quaternion.ToBullet();
            BulletSharp.Math.Matrix.AffineTransformation(1f, ref rot, ref pos, out worldTrans);
        }
        /// <summary>
        /// 位置更新。获取bullet的位置信息，赋值给服务端及unity
        /// </summary>
        /// <param name="m"></param>
        //Bullet calls this so I can copy bullet data to unity
        public override void SetWorldTransform(ref BM.Matrix m)
        {
            i++;
            if (this.i == 1)
            {
                sw = new Stopwatch();
                sw.Start(); 
            }
            unit.Position = BSExtensionMethods2.ExtractTranslationFromMatrix(ref m);
            unit.Quaternion = BSExtensionMethods2.ExtractRotationFromMatrix(ref m);
            if (unit.Position.y<-5000)
            {
                sw.Stop();
                long xx = this.sw.ElapsedMilliseconds;
            }
        }
    }
}
