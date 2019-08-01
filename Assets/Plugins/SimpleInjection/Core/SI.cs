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
			InitializeSI();
		}

		static void InitializeSI()
		{
			// Bind main services
			logService = new UnityLogService();
			serviceLocator = new ServiceLocator(logService);
			injector = new Injector(logService);

			if (VERBOSE) logService.Log("Initializing Simple Injection...", typeof(SI));

			PerformBindOnLoad();

			if (VERBOSE) logService.Log("Simple Injection Initialized!", typeof(SI));
		}

		private static IServiceLocator serviceLocator;
		private static IInjector injector;
		private static ILogService logService;

		#region Helpers

		private static void PerformBindOnLoad()
		{
			object[] attributes;
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					attributes = type.GetCustomAttributes(typeof(BindOnLoadAttribute), true);
					if (attributes.Length > 0)
					{
						var attribute = (BindOnLoadAttribute)attributes[0];
						var instance = Activator.CreateInstance(type);
						serviceLocator.Bind(attribute.type, instance, "");
					}
				}
			}
		}

		#endregion

		#region Service Locator

		public static void Bind<T>(T instance, string id = "")
		{
			serviceLocator.Bind(typeof(T), instance, id);
		}

		public static void Unbind<T>(string id = "")
		{
			serviceLocator.Unbind(typeof(T), id);
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
			injector.Inject(target);
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