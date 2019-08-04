using System;
using System.Linq;
using System.Reflection;
using SimpleInjection.Services;
using UnityEngine;

namespace SimpleInjection
{
	public class UnityInjector : IInjector
	{
		public static bool VERBOSE => SI.VERBOSE;

		private ILogService logService;
		private object valueCache;

		public UnityInjector(ILogService logService)
		{
			this.logService = logService;
		}

		public void Inject(object target)
		{
			PerformGenericInjection<GetAttribute>(target, PerformComponentInjectionOrThrow);
			PerformGenericInjection<GetChildrenAttribute>(target, PerformChildrenComponentInjectionOrThrow);
			PerformGenericInjection<GetParentAttribute>(target, PerformParentComponentInjectionOrThrow);
			PerformGenericInjection<FindAttribute>(target, PerformFindInjectionOrThrow);
		}

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

		private void PerformComponentInjectionOrThrow(object target, FieldInfo field, GetAttribute attr)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponent(field.FieldType);
				if (valueCache.Equals(null) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

				field.SetValue(target, valueCache);
			}
		}

		private void PerformChildrenComponentInjectionOrThrow(object target, FieldInfo field, GetChildrenAttribute attr)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponentInChildren(field.FieldType);
				if (valueCache.Equals(null) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

				field.SetValue(target, valueCache);
			}
		}

		private void PerformParentComponentInjectionOrThrow(object target, FieldInfo field, GetParentAttribute attr)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponentInParent(field.FieldType);
				if (valueCache.Equals(null) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

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
				if (valueCache.Equals(null) && !attr.optional) throw new DependencyNotResolvedException($"Dependency not resolved ==> {field.FieldType}");

				field.SetValue(target, valueCache);
			}
		}
	}
}