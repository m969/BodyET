﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EGamePlay.Combat.Status
{
    public class Status
    {
        public StatusConfigObject StatusConfigObject { get; set; }
        public StatusListen StatusListen { get; set; }
        public StatusRun StatusRun { get; set; }
    }
}