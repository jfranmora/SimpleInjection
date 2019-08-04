using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class GetParentAttribute : Attribute
	{
		public bool optional { get; set; }
	}
}