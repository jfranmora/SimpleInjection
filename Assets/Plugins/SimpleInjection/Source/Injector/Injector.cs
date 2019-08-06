using System.Reflection;
using SimpleInjection.Services;

namespace SimpleInjection
{
	/// <summary>
	/// - Inject (InjectAttribute, DynamicInjectAttribute)
	/// </summary>
	public class Injector : IInjector
	{
		public static bool VERBOSE => SI.VERBOSE;

		private DiContainer container;
		private ILogService logService;
		private object valueCache;

		public Injector (DiContainer container, ILogService logService)
		{
			this.container = container;
			this.logService = logService;
		}

		#region Public API

		public void Inject(object target)
		{
			this.PerformGenericInjection<InjectAttribute>(target, PerformInjectionOrThrow);
			this.PerformGenericInjection<DynamicInjectAttribute>(target, PerformDynamicInjection);
		}

		#endregion

		#region Helpers

		private void PerformInjectionOrThrow(object target, FieldInfo field, InjectAttribute attr)
		{
			valueCache = container.Resolve(field.FieldType, attr.id);
			if (valueCache == null && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

			field.SetValue(target, valueCache);
		}

		private void PerformDynamicInjection(object target, FieldInfo field, DynamicInjectAttribute attr)
		{
			field.SetValue(target, container.Resolve(field.FieldType, attr.id));
		}

		#endregion
	}
}