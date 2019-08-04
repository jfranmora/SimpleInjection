using System;
using System.Reflection;
using SimpleInjection.Services;

namespace SimpleInjection
{
	public class Injector : IInjector
	{
		public static bool VERBOSE => SI.VERBOSE;

		private ILogService logService;
		private object valueCache;

		public Injector (ILogService logService)
		{
			this.logService = logService;
		}

		#region Public API

		public void Inject(object target)
		{
			PerformGenericInjection<InjectAttribute>(target, PerformInjectionOrThrow);
		}

		#endregion

		#region Helpers

		private void PerformGenericInjection<TAttribute>(object target, Action<object, FieldInfo, TAttribute> action) where TAttribute : Attribute
		{
			TAttribute attr;
			foreach (var field in target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				attr = field.GetCustomAttribute<TAttribute>();
				if (attr != null)
				{
					if (VERBOSE) logService.Log($"Injecting in {target} ==> {field.Name}", this);
					action(target, field, attr);
				}
			}
		}

		private void PerformInjectionOrThrow(object target, FieldInfo field, InjectAttribute attr)
		{
			valueCache = SI.Resolve(field.FieldType, attr.id);
			if (valueCache == null && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

			field.SetValue(target, valueCache);
		}

		#endregion
	}
}