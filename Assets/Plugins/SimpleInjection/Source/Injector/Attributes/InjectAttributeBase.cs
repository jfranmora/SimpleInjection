using System;
namespace SimpleInjection
{
	public class InjectAttributeBase : Attribute
	{
		public bool optional { get; set; }

		public InjectAttributeBase() { }
	}
}