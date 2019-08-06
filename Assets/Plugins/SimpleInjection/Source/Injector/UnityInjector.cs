using System.Reflection;
using SimpleInjection.Services;
using UnityEngine;

namespace SimpleInjection
{
	/// <summary>
	/// - Inject (GetAttribute, FindAttribute)
	/// </summary>
	public class UnityInjector : IInjector
	{
		public static bool VERBOSE => SI.VERBOSE;

		private DiContainer container;
		private ILogService logService;

		private object valueCache;

		public UnityInjector(DiContainer container, ILogService logService)
		{
			this.container = container;
			this.logService = logService;
		}

		public void Inject(object target)
		{
			this.PerformGenericInjection<GetAttribute>(target, PerformInjectionOrThrow);
			this.PerformGenericInjection<FindAttribute>(target, PerformFindInjectionOrThrow);
		}

		private void PerformInjectionOrThrow(object target, FieldInfo field, GetAttribute attr)
		{
			switch (attr.source)
			{
				case GetSource.This: PerformComponentInjectionOrThrow(target, field, attr); break;
				case GetSource.Children: PerformChildrenComponentInjectionOrThrow(target, field, attr); break;
				case GetSource.Parent: PerformParentComponentInjectionOrThrow(target, field, attr); break;
			}
		}

		#region Helpers

		private void PerformComponentInjectionOrThrow(object target, FieldInfo field, GetAttribute attr)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponent(field.FieldType);
				if ((valueCache == null || valueCache.Equals(null)) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

				field.SetValue(target, valueCache);
			}
		}

		private void PerformChildrenComponentInjectionOrThrow(object target, FieldInfo field, GetAttribute attr)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponentInChildren(field.FieldType);
				if ((valueCache == null || valueCache.Equals(null)) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

				field.SetValue(target, valueCache);
			}
		}

		private void PerformParentComponentInjectionOrThrow(object target, FieldInfo field, GetAttribute attr)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponentInParent(field.FieldType);
				if ((valueCache == null || valueCache.Equals(null)) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

				field.SetValue(target, valueCache);
			}
		}

		private void PerformFindInjectionOrThrow(object target, FieldInfo field, FindAttribute attr)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = UnityEngine.Object.FindObjectOfType(field.FieldType);
				if ((valueCache == null || valueCache.Equals(null)) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

				field.SetValue(target, valueCache);
			}
		}

		#endregion
	}
}