using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field,
			AllowMultiple = false, Inherited = true)]
	public class FindAttribute : InjectAttributeBase
	{
	}
}