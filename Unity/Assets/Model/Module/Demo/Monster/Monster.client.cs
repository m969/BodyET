using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	public partial class Monster
	{
		public GameObject BodyView { get; set; }
		

		partial void Setup()
		{
		}

		public override void Dispose()
		{
			if (BodyView != null)
				GameObject.Destroy(BodyView);
			BodyView = null;
			base.Dispose();
		}
	}
}