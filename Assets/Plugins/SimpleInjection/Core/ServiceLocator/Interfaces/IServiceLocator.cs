using System;

namespace SimpleInjection
{
	public interface IServiceLocator
	{
		void Bind(Type type, object instance, string id);
		void Unbind(Type type, string id);
		object Resolve(Type type, string id);
	}
}