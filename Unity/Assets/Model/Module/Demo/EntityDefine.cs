using System;
using System.Collections.Generic;
using System.Reflection;

namespace ETModel
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class EntityDefineAttribute : BaseAttribute
	{
		public ushort Type { get; }

		public EntityDefineAttribute(ushort type)
		{
			this.Type = type;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyDefineAttribute : BaseAttribute
	{
		public ushort Id { get; }
		public PropertyType Type { get; }
		public SyncFlag Flag { get; }

		public PropertyDefineAttribute(ushort id, PropertyType type, SyncFlag flag = SyncFlag.OwnClient)
		{
			this.Id = id;
			this.Type = type;
			this.Flag = flag;
		}
	}

	public enum SyncFlag
	{
		AllClients,
		OtherClients,
		OwnClient
	}

	public enum PropertyType
	{
		Int32,
		Int64,
		String
	}

	public class EntityDefine
	{
		public static DoubleMap<Type, ushort> EntityIds { get; set; } = new DoubleMap<Type, ushort>();
		public static Dictionary<ushort, Dictionary<string, PropertyDefineAttribute>> EntityDefInfo { get; set; } = new Dictionary<ushort, Dictionary<string, PropertyDefineAttribute>>();


		public static void Init()
		{
			var assembly = typeof(Game).Assembly;
			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsAbstract)
				{
					continue;
				}
				object[] objects = type.GetCustomAttributes(typeof(EntityDefineAttribute), true);
				if (objects.Length == 0)
				{
					continue;
				}
				var baseAttribute = (EntityDefineAttribute)objects[0];
				EntityIds.Add(type, baseAttribute.Type);
				EntityDefInfo.Add(baseAttribute.Type, new Dictionary<string, PropertyDefineAttribute>());
				foreach (var propertyInfo in type.GetProperties())
				{
					var attribute = propertyInfo.GetCustomAttribute<PropertyDefineAttribute>();
					if (attribute != null)
					{
						EntityDefInfo[baseAttribute.Type].Add(propertyInfo.Name, attribute);
					}
				}
			}
			Console.WriteLine($"{JsonHelper.ToJson(EntityDefInfo.Values)}");
		}
	}
}