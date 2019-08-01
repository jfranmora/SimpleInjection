using System;
using System.Collections.Generic;
using SimpleInjection.Services;

namespace SimpleInjection
{
	public class ServiceLocator : IServiceLocator
	{
		public static readonly bool VERBOSE = false;

		private List<BindingContainer> data = new List<BindingContainer>();
		private ILogService logService;

		public ServiceLocator(ILogService logService)
		{
			this.logService = logService;
		}

		public void Bind(Type type, object instance, string id)
		{
			if (instance == null)
			{
				logService.LogWarning("Instance is null...", this);
				return;
			}

			// Find db
			var container = data.Find(x => x.id.Equals(id));
			if (container == null)
			{
				container = new BindingContainer(id);
				data.Add(container);
			}

			// Register service
			string key = type.FullName;
			if (!container.bindings.ContainsKey(key))
			{
				container.bindings.Add(key, null);
			}
			container.bindings[key] = instance;

			if (VERBOSE) logService.Log($"Bind<{type}>({instance})", this);
		}

		public void Unbind(Type type, string id)
		{
			string key = type.FullName;

			var container = data.Find(x => x.id.Equals(id));
			if (container != null)
			{
				container.bindings.Remove(key);
			}
		}

		public object Resolve(Type type, string id)
		{
			string key = type.FullName;

			var container = data.Find(x => x.id.Equals(id));
			if (container == null)
			{
				return default;
			}
			else
			{
				return container.bindings.ContainsKey(key) ? container.bindings[key] : ResolveFallback(type, id);
			}
		}

		private object ResolveFallback(Type type, string id)
		{
			string key = type.FullName;

			foreach (var container in data)
			{
				if (container.id.Equals(id)) continue;

				if (container.bindings.ContainsKey(key)) return container.bindings[key];
			}

			return default;
		}

		public class BindingContainer
		{
			public string id;
			public Dictionary<string, object> bindings;

			public BindingContainer(string id)
			{
				this.id = id;
				this.bindings = new Dictionary<string, object>();
			}
		}
	}
}