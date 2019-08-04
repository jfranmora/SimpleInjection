using System;
using System.Collections.Generic;
using System.Reflection;
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

		private static IServiceLocator serviceLocator;
		private static List<IInjector> injectorList;
		private static ILogService logService;

		#region Helpers

		private static void PerformBindMainServices()
		{
			if (VERBOSE) logService.Log("Initializing main services...", typeof(SI));

			// Bind locators
			serviceLocator = new Locator(logService);

			// Bind injectors...
			injectorList = new List<IInjector>();
			injectorList.Add(new Injector(logService));
			injectorList.Add(new UnityInjector(logService));

			// TODO: Test this!
			// injectorList.Add(new AutoInjector(logService, (Locator)serviceLocator));
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
						serviceLocator.Register(attribute.type, instance, "");
					}
				}
			}
		}

		#endregion

		#region Bind

		public static void Bind<T>(T instance, string id = "")
		{
			serviceLocator.Register(typeof(T), instance, id);
		}

		// TODO: Bind factories
		public static void Bind<T>(T instance, Func<T> factory, string id = "")
		{
			throw new NotImplementedException();
		}

		public static T Resolve<T>(string id = "")
		{
			return (T)serviceLocator.Resolve(typeof(T), id);
		}

		public static object Resolve(Type type, string id = "")
		{
			return serviceLocator.Resolve(type, id);
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

		#region Helpers

		static IEnumerable<Type> GetTypesWithCustomAttribute<T>(Assembly assembly) where T : Attribute
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.GetCustomAttributes(typeof(T), true).Length > 0)
				{
					yield return type;
				}
			}
		}

		#endregion
	}
}