using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleInjection.Services;

namespace SimpleInjection
{
	/// <summary>
	/// - Inject (DynamicInjectAttribute)
	/// </summary>
	public class DynamicInjector : IInjector
	{
		public static bool VERBOSE => SI.VERBOSE;

		private DiContainer container;
		private ILogService logService;

		private Dictionary<Type, Dictionary<string, DynamicBinding>> bindingDatabase;

		public DynamicInjector(DiContainer container, ILogService logService)
		{
			bindingDatabase = new Dictionary<Type, Dictionary<string, DynamicBinding>>();

			this.container = container;
			this.logService = logService;

			container.OnBind += OnBind;
		}

		#region Public API

		public void Inject(object target)
		{
			this.PerformGenericInjection<DynamicInjectAttribute>(target, PerformInjection);
		}

		#endregion

		#region Event Handling

		private void OnBind(Type type, object target, string id)
		{
			if (VERBOSE) logService.Log($"Notify [{type}-{id}] bindings!");

			if (bindingDatabase.ContainsKey(type) && bindingDatabase[type].ContainsKey(id))
			{
				bindingDatabase[type][id].UpdateFieldValues(target);
			}
		}

		#endregion

		#region Helpers

		private void PerformInjection(object target, FieldInfo field, DynamicInjectAttribute attr)
		{
			var type = field.FieldType;
			var id = attr.id;

			Dictionary<string, DynamicBinding> typeContainer = bindingDatabase.ContainsKey(type) ? bindingDatabase[type] : new Dictionary<string, DynamicBinding>();
			if (!bindingDatabase.ContainsKey(type))
				bindingDatabase.Add(type, typeContainer);

			DynamicBinding binding = typeContainer.ContainsKey(id) ? typeContainer[id] : new DynamicBinding(type, id);
			if (!bindingDatabase[type].ContainsKey(id))
				bindingDatabase[type].Add(id, binding);

			if (!binding.instanceList.Contains(target))
			{
				binding.instanceList.Add(target);
				binding.fieldInfoList.Add(field);

				if (VERBOSE) logService.Log($"Binding {field} to {type}-{id}");
			}
		}

		#endregion

		private struct DynamicBinding
		{
			public Type type;
			public string id;

			public List<object> instanceList;
			public List<FieldInfo> fieldInfoList;

			public DynamicBinding(Type type, string id)
			{
				this.type = type;
				this.id = id;

				this.instanceList = new List<object>();
				this.fieldInfoList = new List<FieldInfo>();
			}

			public void UpdateFieldValues(object value)
			{
				for (var i = 0; i < instanceList.Count; i++)
				{
					if (instanceList[i] == null || fieldInfoList[i] == null) continue;

					fieldInfoList[i].SetValue(instanceList[i], value);
				}
			}
		}
	}
}