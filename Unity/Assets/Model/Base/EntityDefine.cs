using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ETModel
{
	[AttributeUsage(AttributeTargets.Class)]
	public class EntityDefineAttribute : BaseAttribute
	{
		public ushort EntityTypeId { get; }

		public EntityDefineAttribute(ushort type)
		{
			this.EntityTypeId = type;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyDefineAttribute : BaseAttribute
	{
		public ushort Id { get; }
		public Type Type { get; set; }
		public SyncFlag Flag { get; }

		public PropertyDefineAttribute(ushort id, SyncFlag flag = SyncFlag.OwnClient)
		{
			this.Id = id;
			this.Flag = flag;
		}
	}

	public enum SyncFlag
	{
		AllClients,
		OtherClients,
		OwnClient
	}

	public class EntityDefine
	{
		public static DoubleMap<Type, ushort> EntityIds { get; set; } = new DoubleMap<Type, ushort>();
		public static Dictionary<ushort, Dictionary<string, PropertyDefineAttribute>> PropertyDefineCollectionMap { get; set; } = new Dictionary<ushort, Dictionary<string, PropertyDefineAttribute>>();
		public static Dictionary<ushort, Dictionary<ushort, PropertyInfo>> PropertyCollectionMap { get; set; } = new Dictionary<ushort, Dictionary<ushort, PropertyInfo>>();
		public static Dictionary<ushort, Dictionary<ushort, PropertyInfo>> ReactPropertyCollectionMap { get; set; } = new Dictionary<ushort, Dictionary<ushort, PropertyInfo>>();
		public static Action<Entity, string, byte[]> OnPropertyChanged { get; set; }


		public static ushort GetTypeId(Type type)
		{
			return EntityIds.GetValueByKey(type);
		}

		public static ushort GetTypeId<T>()
		{
			return EntityIds.GetValueByKey(typeof(T));
		}

		public static void Init()
		{
			var assembly = typeof(Game).Assembly;
			foreach (Type type in assembly.GetTypes())
			{
				if (type.IsAbstract)
				{
					continue;
				}
				var entityDefineAttribute = type.GetCustomAttribute<EntityDefineAttribute>();
				if (entityDefineAttribute == null)
					continue;
				EntityIds.Add(type, entityDefineAttribute.EntityTypeId);
				PropertyDefineCollectionMap.Add(entityDefineAttribute.EntityTypeId, new Dictionary<string, PropertyDefineAttribute>());
				PropertyCollectionMap.Add(entityDefineAttribute.EntityTypeId, new Dictionary<ushort, PropertyInfo>());
				ReactPropertyCollectionMap.Add(entityDefineAttribute.EntityTypeId, new Dictionary<ushort, PropertyInfo>());
				foreach (var propertyInfo in type.GetProperties())
				{
					var attribute = propertyInfo.GetCustomAttribute<PropertyDefineAttribute>();
					if (attribute != null)
					{
						attribute.Type = propertyInfo.PropertyType;
						PropertyDefineCollectionMap[entityDefineAttribute.EntityTypeId].Add(propertyInfo.Name, attribute);

						PropertyCollectionMap[entityDefineAttribute.EntityTypeId].Add(attribute.Id, propertyInfo);
						var reactPropertyInfo = type.GetProperty($"{propertyInfo.Name}Property");
						ReactPropertyCollectionMap[entityDefineAttribute.EntityTypeId].Add(attribute.Id, reactPropertyInfo);
					}
				}
			}
		}
	}

	public partial class Entity
	{
		protected void PublishProperty(string name,  byte[] valueBytes = null)
		{
#if !SERVER
			if (this != Unit.LocalUnit)
				return;
#else
			if (Domain == null)
				return;
#endif
			EntityDefine.OnPropertyChanged?.Invoke(this, name, valueBytes);
		}

		protected void PublishProperty(string name, string value)
		{
			PublishProperty(name, MongoHelper.ToBson(value));
		}

		protected void PublishProperty(string name, int value)
		{
			PublishProperty(name, MongoHelper.ToBson(value));
		}

		protected void PublishProperty(string name, float value)
		{
			PublishProperty(name, MongoHelper.ToBson(value));
		}

		protected void PublishProperty(string name, Vector3 value)
		{
			PublishProperty(name, MongoHelper.ToBson(value));
		}


		public void SetPropertyValue(ushort propertyId, byte[] valueBytes)
		{
			var server = false;
#if SERVER
			server = true;
#endif
			var propertyCollection = EntityDefine.PropertyCollectionMap[EntityDefine.GetTypeId(GetType())];
			var reactPropertyCollection = EntityDefine.ReactPropertyCollectionMap[EntityDefine.GetTypeId(GetType())];
			var propertyInfo = propertyCollection[propertyId];
			var reactPropertyInfo = reactPropertyCollection[propertyId];
			if (propertyInfo == null)
			{
				Log.Error($"Error {propertyId} propertyInfo == null {propertyInfo}");
				return;
			}
			if (propertyInfo.PropertyType == typeof(int))
			{
				if (server)
					propertyInfo.SetValue(this, MongoHelper.ToInt(valueBytes));
				else 
					typeof(ReactProperty<int>).GetProperty("Value").SetValue(reactPropertyInfo.GetValue(this), MongoHelper.ToInt(valueBytes));
			}
			if (propertyInfo.PropertyType == typeof(float))
			{
				if (server)
					propertyInfo.SetValue(this, MongoHelper.ToFloat(valueBytes));
				else
					typeof(ReactProperty<float>).GetProperty("Value").SetValue(reactPropertyInfo.GetValue(this), MongoHelper.ToFloat(valueBytes));
			}
			if (propertyInfo.PropertyType == typeof(string))
			{
				if (server)
					propertyInfo.SetValue(this, MongoHelper.ToString(valueBytes));
				else
					typeof(ReactProperty<string>).GetProperty("Value").SetValue(reactPropertyInfo.GetValue(this), MongoHelper.ToString(valueBytes));
			}
			if (propertyInfo.PropertyType == typeof(Vector3))
			{
				if (server)
					propertyInfo.SetValue(this, MongoHelper.FromBson<Vector3>(valueBytes));
				else
					typeof(ReactProperty<Vector3>).GetProperty("Value").SetValue(reactPropertyInfo.GetValue(this), MongoHelper.FromBson<Vector3>(valueBytes));
			}
		}
	}
}