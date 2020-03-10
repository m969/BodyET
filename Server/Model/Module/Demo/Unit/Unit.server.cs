using UnityEngine;

namespace ETModel
{
	public partial class Unit
	{
		public Vector3 Position
		{
			get
			{
				return Transform.Position;
			}
			set
			{
				Transform.Position = value;
			}
		}

		public void Update()
		{

		}
	}
}