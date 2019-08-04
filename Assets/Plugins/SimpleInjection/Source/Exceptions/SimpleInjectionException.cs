using System;

namespace SimpleInjection
{
	public class SimpleInjectionException : Exception
	{
		public SimpleInjectionException() : base() { }
		public SimpleInjectionException(string message) : base(message) { }
	}
}