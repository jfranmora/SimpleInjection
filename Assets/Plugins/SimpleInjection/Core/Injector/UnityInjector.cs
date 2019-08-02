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

		public UnityInjector(ILogService logService)
		{
			this.logService = logService;
		}

		public void Inject(object target)
		{
			PerformGenericInjection(target, typeof(GetAttribute), PerformComponentInjection);
			PerformGenericInjection(target, typeof(GetChildrenAttribute), PerformChildrenComponentInjection);
			PerformGenericInjection(target, typeof(GetParentAttribute), PerformParentComponentInjection);
			PerformGenericInjection(target, typeof(FindAttribute), PerformFindInjection);
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

		private void PerformComponentInjection(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
				// field.SetValue(target, ((Component)target).GetComponents(t));
			}
			else
			{
				field.SetValue(target, ((Component)target).GetComponent(field.FieldType));
			}
		}

		private void PerformChildrenComponentInjection(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				field.SetValue(target, ((Component)target).GetComponentInChildren(field.FieldType));
			}
		}

		private void PerformParentComponentInjection(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				field.SetValue(target, ((Component)target).GetComponentInParent(field.FieldType));
			}
		}

		private void PerformFindInjection(object target, FieldInfo field)
		{
			if (field.FieldType.IsArray)
			{
				logService.LogError("Can't cast to array!", this);
			}
			else
			{
				field.SetValue(target, UnityEngine.Object.FindObjectOfType(field.FieldType));
			}
		}
	}
}