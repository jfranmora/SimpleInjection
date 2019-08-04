using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class InjectAttribute : Attribute
	{
		public string id { get; set; }
		public bool optional { get; set; }

		public InjectAttribute() : this("")
		{

		}

		public InjectAttribute(string id)
		{
			this.id = id;
		}
	}
}