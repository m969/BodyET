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

		public override async ETTask G2M_CreateUnitsHandler(Scene scene, G2M_CreateUnit request, M2G_CreateUnit response, Action reply)
		{

			await ETTask.CompletedTask;
		}
	}
}