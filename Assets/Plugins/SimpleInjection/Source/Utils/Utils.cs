using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleInjection
{
	public static class Utils
	{
		public static IEnumerable<Type> GetTypesWithCustomAttribute<T>(Assembly assembly) where T : Attribute
		{
			foreach (Type type in assembly.GetTypes())
			{
				if (type.GetCustomAttributes(typeof(T), true).Length > 0)
				{
					yield return type;
				}
			}
		}
	}
}