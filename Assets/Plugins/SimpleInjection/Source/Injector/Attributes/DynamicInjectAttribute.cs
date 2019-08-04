using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DynamicInjectAttribute : Attribute
	{
		public string id { get; set; }

		public DynamicInjectAttribute() : this("")
		{

		}

		public DynamicInjectAttribute(string id)
		{
			this.id = id;
		}
	}
}