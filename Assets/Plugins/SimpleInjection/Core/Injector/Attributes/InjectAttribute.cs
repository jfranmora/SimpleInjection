using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Field)]
	public class InjectAttribute : Attribute
	{
		public string id;

		public InjectAttribute() : this("")
		{

		}

		public InjectAttribute(string id)
		{
			this.id = id;
		}
	}
}