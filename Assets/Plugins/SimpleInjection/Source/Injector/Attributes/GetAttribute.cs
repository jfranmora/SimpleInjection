using System;

namespace SimpleInjection
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field,
			AllowMultiple = false, Inherited = true)]
	public class GetAttribute : InjectAttributeBase
	{
		public GetSource source;

		public GetAttribute() : this(GetSource.This)
		{

		}

		public GetAttribute(GetSource source) : base()
		{
			this.source = source;
		}
	}
}