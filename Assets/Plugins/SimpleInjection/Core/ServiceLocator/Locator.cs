using System;
using System.Collections.Generic;
using SimpleInjection.Services;

namespace SimpleInjection
{
	public class Locator : IServiceLocator
	{
		public static bool VERBOSE => SI.VERBOSE;

		// data[type] = container;
		private Dictionary<Type, BindingContainer> data = new Dictionary<Type, BindingContainer>();
		private ILogService logService;

		public Locator(ILogService logService)
		{
			this.logService = logService;
		}

		public void Register(Type type, object instance, string id)
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

			if (VERBOSE) logService.Log($"Bind<{type}>[{id}]({instance})", this);
		}

		public object Resolve(Type type, string id)
		{
			string key = type.FullName;

			if (!data.ContainsKey(type)) return default;

			return data[type].bindings.ContainsKey(id) ? data[type].bindings[key] : default;
		}

		public class BindingContainer
		{
			// bindings[Id] = instance
			public Dictionary<string, object> bindings;

			public BindingContainer()
			{
				this.bindings = new Dictionary<string, object>();
			}
		}
	}
}