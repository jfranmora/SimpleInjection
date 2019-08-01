using System;

namespace SimpleInjection.Services
{
	public interface ILogService : IService
	{
		void Log(object msg);
		void Log(object msg, object context);
		void Log(object msg, Type type);

		void LogWarning(object msg);
		void LogWarning(object msg, object context);
		void LogWarning(object msg, Type type);

		void LogError(object msg);
		void LogError(object msg, object context);
		void LogError(object msg, Type type);
	}
}