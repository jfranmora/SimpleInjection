using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field,
			AllowMultiple = false, Inherited = true)]
	public class InjectAttribute : InjectAttributeBase
	{
		public string id { get; set; }

		public InjectAttribute() : this("") { }

		public InjectAttribute(string id) : base()
		{
			this.id = id;
		}
	}
}