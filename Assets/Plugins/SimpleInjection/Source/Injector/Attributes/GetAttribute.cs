using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class GetAttribute : Attribute
	{
		public bool optional { get; set; }
	}
}