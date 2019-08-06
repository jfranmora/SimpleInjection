using System;
using System.Reflection;

namespace SimpleInjection
{
	public static class InjectorExtensions
	{
		public static void PerformGenericInjection<TAttribute>
			(this IInjector injector, object target, Action<object, FieldInfo, TAttribute> action)
			where TAttribute : Attribute
		{
			TAttribute attr;
			foreach (var field in target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				attr = field.GetCustomAttribute<TAttribute>();
				if (attr != null)
				{
					action(target, field, attr);
				}
			}
		}
	}
}