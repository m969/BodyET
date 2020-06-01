using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
    public class EntitySyncComponent : Entity
    {
        public int Fps { get; set; } = 20;
        public long Interval { get; set; } = 100;
        public long Timer { get; set; } = 0;
    }
}