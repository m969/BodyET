using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ETModel
{
	[AttributeUsage(AttributeTargets.Class)]
	public class EntityDefineAttribute : BaseAttribute
	{
		public int EntityTypeId { get; set; }

		public EntityDefineAttribute()
		{
			//this.EntityTypeId = type;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public class ComponentDefineAttribute : BaseAttribute
	{
		public int ComponentTypeId { get; set; }

		public ComponentDefineAttribute()
		{
			//this.EntityTypeId = type;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class PropertyDefineAttribute : BaseAttribute
	{
		public int Id { get; set; }
		public Type Type { get; set; }
		public SyncFlag Flag { get; }

		public PropertyDefineAttribute(SyncFlag flag = SyncFlag.OwnClient)
		{
			//this.Id = id;
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
		public static DoubleMap<Type, int> TypeIds { get; set; } = new DoubleMap<Type, int>();
		//public static DoubleMap<Type, int> ComponentTypeIds { get; set; } = new DoubleMap<Type, int>();
		public static Dictionary<int, Dictionary<string, PropertyDefineAttribute>> PropertyDefineCollectionMap { get; set; } = new Dictionary<int, Dictionary<string, PropertyDefineAttribute>>();
		public static Dictionary<int, Dictionary<int, PropertyInfo>> PropertyCollectionMap { get; set; } = new Dictionary<int, Dictionary<int, PropertyInfo>>();
		public static Dictionary<int, Dictionary<int, PropertyInfo>> ReactPropertyCollectionMap { get; set; } = new Dictionary<int, Dictionary<int, PropertyInfo>>();
		public static Action<Entity, string, byte[]> OnPropertyChanged { get; set; }


		public static Type GetType(int typeId)
		{
			return TypeIds.GetKeyByValue(typeId);
		}

		public static int GetTypeId(Type type)
		{
			return TypeIds.GetValueByKey(type);
		}

		public static int GetTypeId<T>()
		{
			return TypeIds.GetValueByKey(typeof(T));
		}

		public static bool IsComponent(Type type)
		{
			var conponentDefineAttribute = type.GetCustomAttribute<ComponentDefineAttribute>();
			return conponentDefineAttribute != null;
		}

		public static bool IsEntity(Type type)
		{
			var conponentDefineAttribute = type.GetCustomAttribute<EntityDefineAttribute>();
			return conponentDefineAttribute != null;
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
				var typeId = 0;
				var entityDefineAttribute = type.GetCustomAttribute<EntityDefineAttribute>();
				if (entityDefineAttribute != null)
				{
					var arr = type.Name.ToCharArray();
					var i = 0;
					typeId = (int)arr[i];
					Log.Debug($"{type.Name} {typeId.ToString()}");
					i++;
					while (TypeIds.ContainsValue(typeId) && i < arr.Length)
					{
						typeId = int.Parse(typeId.ToString() + (int)arr[i]);
						Log.Debug($"{type.Name} {typeId.ToString()}");
						i++;
					}
					entityDefineAttribute.EntityTypeId = typeId;
					TypeIds.Add(type, typeId);
				}
				var conponentDefineAttribute = type.GetCustomAttribute<ComponentDefineAttribute>();
				if (conponentDefineAttribute != null)
				{
					var arr = type.Name.ToCharArray();
					var i = 0;
					typeId = (int)arr[i];
					Log.Debug($"{type.Name} {typeId.ToString()}");
					i++;
					while (TypeIds.ContainsValue(typeId) && i < arr.Length)
					{
						typeId = int.Parse(typeId.ToString() + (int)arr[i]);
						Log.Debug($"{type.Name} {typeId.ToString()}");
						i++;
					}
					conponentDefineAttribute.ComponentTypeId = typeId;
					TypeIds.Add(type, typeId);
				}
				if (typeId == 0)
					continue;
				PropertyDefineCollectionMap.Add(typeId, new Dictionary<string, PropertyDefineAttribute>());
				PropertyCollectionMap.Add(typeId, new Dictionary<int, PropertyInfo>());
				ReactPropertyCollectionMap.Add(typeId, new Dictionary<int, PropertyInfo>());
				foreach (var propertyInfo in type.GetProperties())
				{
					var attribute = propertyInfo.GetCustomAttribute<PropertyDefineAttribute>();
					if (attribute != null)
					{
						attribute.Type = propertyInfo.PropertyType;
						PropertyDefineCollectionMap[typeId].Add(propertyInfo.Name, attribute);
						//
						var arr1 = propertyInfo.Name.ToCharArray();
						var j = 0;
						var propertyId = (int)arr1[j];
						Log.Debug($"{propertyInfo.Name} {propertyId.ToString()}");
						j++;
						while (PropertyCollectionMap[typeId].ContainsKey(propertyId) && j < arr1.Length)
						{
							propertyId = int.Parse(propertyId.ToString() + (int)arr1[j]);
							Log.Debug($"{propertyInfo.Name} {propertyId.ToString()}");
							j++;
						}
						attribute.Id = propertyId;
						//
						PropertyCollectionMap[typeId].Add(attribute.Id, propertyInfo);
						var reactPropertyInfo = type.GetProperty($"{propertyInfo.Name}Property");
						ReactPropertyCollectionMap[typeId].Add(attribute.Id, reactPropertyInfo);
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


		public void SetPropertyValue(int propertyId, byte[] valueBytes)
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