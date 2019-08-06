using System;
using System.Collections.Generic;
using SimpleInjection.Services;

namespace SimpleInjection
{
#if UNITY_EDITOR
	[UnityEditor.InitializeOnLoad]
#endif
	public static class SI
	{
		public static readonly bool VERBOSE = false;

		// Initialize SimpleInjection
		static SI()
		{
			Initialize();
		}

		static void Initialize()
		{
			logService = new UnityLogService();

			if (VERBOSE) logService.Log("Initializing Simple Injection...", typeof(SI));

			PerformBindMainServices();
			PerformDefaultBindings();

			if (VERBOSE) logService.Log("Simple Injection Initialized!", typeof(SI));
		}

		private static DiContainer container;
		private static List<IInjector> injectorList;
		private static ILogService logService;

		#region Helpers

		private static void PerformBindMainServices()
		{
			if (VERBOSE) logService.Log("Initializing main services...", typeof(SI));

			// Bind container
			container = new DiContainer(logService);

			// Bind injectors...
			injectorList = new List<IInjector>();
			injectorList.Add(new Injector(container, logService));
			injectorList.Add(new UnityInjector(container, logService));
			injectorList.Add(new DynamicInjector(container, logService));
		}

		private static void PerformDefaultBindings()
		{
			if (VERBOSE) logService.Log("Perform default bindings...", typeof(SI));

			object[] attributes;
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					attributes = type.GetCustomAttributes(typeof(DefaultBindingAttribute), true);
					if (attributes.Length > 0)
					{
						var attribute = (DefaultBindingAttribute)attributes[0];
						var instance = Activator.CreateInstance(type);
						container.Bind(attribute.type, instance, "");
					}
				}
			}
		}

		#endregion

		#region Bind

		public static void Bind<T>(T instance, string id = "")
		{
			container.Bind(instance, id);
		}

		public static T Resolve<T>(string id = "")
		{
			return (T)container.Resolve(typeof(T), id);
		}

		public static object Resolve(Type type, string id = "")
		{
			return container.Resolve(type, id);
		}

		#endregion

		#region Injector

		public static void Inject(object target)
		{
			foreach (var injector in injectorList)
			{
				injector.Inject(target);
			}
		}

		#endregion
	}
}