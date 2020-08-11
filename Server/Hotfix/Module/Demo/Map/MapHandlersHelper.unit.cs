using System;
using ETModel;
using PF;
using UnityEngine;
using System.Linq;

namespace ETHotfix
{
	public partial class MapHandlersHelper: HandlersHelperBase
	{
		public static MapHandlersHelper Instance { get; set; }

        public override ETTask C2G_LoginGateHandler(Scene scene, C2G_LoginGate request, G2C_LoginGate response, Action reply)
        {
            return base.C2G_LoginGateHandler(scene, request, response, reply);
        }
    }
}