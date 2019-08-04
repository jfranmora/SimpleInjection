using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class FindAttribute : Attribute
	{
		public bool optional { get; set; }
	}
}