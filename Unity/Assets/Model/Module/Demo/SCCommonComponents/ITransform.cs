using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public interface ITransform
	{
		Vector3 LastPosition { get; set; }
		Vector3 Position { get; set; }
		float Angle { get; set; }
		float Scale { get; set; }
	}
}