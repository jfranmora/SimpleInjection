using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public class DefaultBindingAttribute : Attribute
	{
		public Type type;
		public int priority;

		public DefaultBindingAttribute(Type type, int priority = 0)
		{
			this.type = type;
			this.priority = priority;
		}
	}
}