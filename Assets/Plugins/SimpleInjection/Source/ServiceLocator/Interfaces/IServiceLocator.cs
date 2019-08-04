using System;

namespace SimpleInjection
{
	public interface IServiceLocator
	{
		void Register(Type type, object instance, string id);
		object Resolve(Type type, string id);
	}
}