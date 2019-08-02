using System;
using System.Linq;
using System.Reflection;
using SimpleInjection.Services;

namespace SimpleInjection
{
	public class Injector : IInjector
	{
		public static bool VERBOSE => SI.VERBOSE;

		private ILogService logService;

		public Injector (ILogService logService)
		{
			this.logService = logService;
		}

		#region Public API

		public void Inject(object target)
		{
			PerformGenericInjection(target, typeof(InjectAttribute), PerformInjection);
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
			field.SetValue(
				target, 
				SI.Resolve(field.FieldType, field.GetCustomAttribute<InjectAttribute>().id)
			);
		}

		#endregion
	}
}