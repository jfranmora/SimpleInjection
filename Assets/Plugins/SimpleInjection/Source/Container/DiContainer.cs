using System;
using System.Collections.Generic;
using SimpleInjection.Services;

namespace SimpleInjection
{
	/// <summary>
	/// - Bind
	/// - Resolve
	/// </summary>
	public class DiContainer
	{
		public static bool VERBOSE => SI.VERBOSE;

		public event Action<Type, object, string> OnBind;

		// data[type] = container;
		private Dictionary<Type, BindingContainer> data = new Dictionary<Type, BindingContainer>();
		private ILogService logService;

		public DiContainer(ILogService logService)
		{
			this.logService = logService;
		}

		public void Bind<T>(T instance, string id = "")
		{
			Bind(typeof(T), instance, id);
		}

		public void Bind(Type type, object instance, string id = "")
		{
			if (instance == null)
			{
				logService.LogWarning("Instance is null...", this);
				return;
			}

			// Find db
			var container = data.ContainsKey(type) ? data[type] : null;
			if (container == null)
			{
				container = new BindingContainer();
				data.Add(type, container);
			}

			// Register service
			if (!container.bindings.ContainsKey(id))
			{
				container.bindings.Add(id, null);
			}
			container.bindings[id] = instance;

			OnBind?.Invoke(type, instance, id);

			if (VERBOSE) logService.Log($"Bind<{type}>[{id}]({instance})", this);
		}

		public object Resolve(Type type, string id)
		{
			if (!data.ContainsKey(type)) return default;

			return data[type].bindings.ContainsKey(id) ? data[type].bindings[id] : default;
		}

		public class BindingContainer
		{
			public Dictionary<string, object> bindings;

			public BindingContainer()
			{
				bindings = new Dictionary<string, object>();
			}
		}
	}
}