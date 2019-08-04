using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjection.Services;

namespace SimpleInjection
{
	public class AutoInjector : IInjector
	{
		public static bool VERBOSE => SI.VERBOSE;

		private ILogService logService;
		private Locator locator;

		private Dictionary<Type, Dictionary<string, Container>> bindingData;

		public AutoInjector(ILogService logService, Locator locator)
		{
			bindingData = new Dictionary<Type, Dictionary<string, Container>>();

			this.logService = logService;

			this.locator = locator;
			locator.OnRegister += Locator_OnRegister;
		}

		#region Public API

		public void Inject(object target)
		{
			PerformGenericInjection(target, typeof(InjectAttribute), PerformInjection);
		}

		#endregion

		#region Event Handling

		private void Locator_OnRegister(Type type, object target, string id)
		{
			// TODO: Notify to observers
			if (VERBOSE) logService.Log($"Notify [{type}-{id}] bindings!");

			if (bindingData.ContainsKey(type))
			{
				if (bindingData[type].ContainsKey(id))
				{
					var container = bindingData[type][id];
					for (var i = 0; i < container.instanceList.Count; i++)
					{
						if (container.instanceList[i] == null || container.fieldInfoList[i] == null) continue;

						container.fieldInfoList[i].SetValue(container.instanceList[i], target);
						if (VERBOSE) logService.Log($"Updated reference on {container.instanceList[i]} - {container.fieldInfoList[i]}");
					}
				}
			}
		}

		#endregion

		#region Helpers

		private void PerformGenericInjection(object target, Type attribute, Action<object, FieldInfo> action)
		{
			var fields = target.GetType()
				.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(prop => Attribute.IsDefined(prop, attribute));

			foreach (var field in fields)
			{
				if (VERBOSE) logService.Log($"Injecting in {target} ==> {field.Name}", this);
				action(target, field);
			}
		}

		private void PerformInjection(object target, FieldInfo field)
		{
			var attr = field.GetCustomAttribute<InjectAttribute>();
			var type = field.FieldType;
			var id = attr.id;

			Dictionary<string, Container> typeContainer = bindingData.ContainsKey(type) ? bindingData[type] : new Dictionary<string, Container>();
			if (!bindingData.ContainsKey(type))
				bindingData.Add(type, typeContainer);

			Container bindingContainer = typeContainer.ContainsKey(id) ? typeContainer[id] : new Container(type, id);
			if (!bindingData[type].ContainsKey(id))
				bindingData[type].Add(id, bindingContainer);

			if (!bindingContainer.instanceList.Contains(target))
			{
				bindingContainer.instanceList.Add(target);
				bindingContainer.fieldInfoList.Add(field);

				if (VERBOSE) logService.Log($"Binding {field} to {type}-{id}");
			}
		}

		#endregion

		private struct Container
		{
			public Type type;
			public string id;

			public List<object> instanceList;
			public List<FieldInfo> fieldInfoList;

			public Container(Type type, string id)
			{
				this.type = type;
				this.id = id;

				this.instanceList = new List<object>();
				this.fieldInfoList = new List<FieldInfo>();
			}
		}
	}
}