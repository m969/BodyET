using UnityEngine;
using MongoDB.Bson.Serialization.Attributes;

namespace ETModel
{
	public partial class Bullet
	{
		//[BsonIgnore]
		public GameObject BodyView { get; set; }
		//[BsonIgnore]
		//public Vector3 Position
		//{
		//	get
		//	{
		//		return BodyView.transform.position;
		//	}
		//	set
		//	{
		//		BodyView.transform.position = value;
		//	}
		//}

		public override void Dispose()
		{
			if (BodyView != null)
				GameObject.Destroy(BodyView);
			BodyView = null;
			base.Dispose();
		}
	}
}