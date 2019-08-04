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
			PerformGenericInjection(target, typeof(GetAttribute), PerformComponentInjectionOrThrow);
			PerformGenericInjection(target, typeof(GetChildrenAttribute), PerformChildrenComponentInjectionOrThrow);
			PerformGenericInjection(target, typeof(GetParentAttribute), PerformParentComponentInjectionOrThrow);
			PerformGenericInjection(target, typeof(FindAttribute), PerformFindInjectionOrThrow);
		}

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

		private void PerformComponentInjectionOrThrow(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
				// field.SetValue(target, ((Component)target).GetComponents(t));
			}
			else
			{
				valueCache = ((Component)target).GetComponent(field.FieldType);
				if (valueCache == null) throw new DependencyNotResolvedException();

				field.SetValue(target, valueCache);
			}
		}

		private void PerformChildrenComponentInjectionOrThrow(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponentInChildren(field.FieldType);
				if (valueCache == null) throw new DependencyNotResolvedException();

				field.SetValue(target, valueCache);
			}
		}

		private void PerformParentComponentInjectionOrThrow(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = ((Component)target).GetComponentInParent(field.FieldType);
				if (valueCache == null) throw new DependencyNotResolvedException();

				field.SetValue(target, valueCache);
			}
		}

		private void PerformFindInjectionOrThrow(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				valueCache = UnityEngine.Object.FindObjectOfType(field.FieldType);
				if (valueCache == null) throw new DependencyNotResolvedException();

				field.SetValue(target, valueCache);
			}
		}
	}
}