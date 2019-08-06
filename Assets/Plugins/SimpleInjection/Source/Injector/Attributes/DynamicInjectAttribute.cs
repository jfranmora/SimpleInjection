using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field,
			AllowMultiple = false, Inherited = true)]
	public class DynamicInjectAttribute : InjectAttributeBase
	{
		public string id { get; set; }

		public DynamicInjectAttribute() : this("") { }

		public DynamicInjectAttribute(string id) : base()
		{
			this.id = id;
		}
	}
}