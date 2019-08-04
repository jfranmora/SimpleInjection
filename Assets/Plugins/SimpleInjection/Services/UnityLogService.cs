using System;
using UnityEngine;

namespace SimpleInjection.Services
{
	public class UnityLogService : ILogService
	{
		public void Log(object msg)
		{
			Debug.Log($"[{Now()}] {msg}");
		}

		public void Log(object msg, object context)
		{
			Log(msg, context.GetType());
		}

		public void Log(object msg, Type type)
		{
			Debug.Log($"[{Now()}]<b>[{type}]</b> {msg}");
		}

		public void LogWarning(object msg)
		{
			Debug.LogWarning($"[{Now()}] {msg}");
		}

		public void LogWarning(object msg, object context)
		{
			LogWarning(msg, context.GetType());
		}

		public void LogWarning(object msg, Type type)
		{
			Debug.LogWarning($"[{Now()}]<b>[{type}]</b> {msg}");
		}

		public void LogError(object msg)
		{
			Debug.LogError($"[{Now()}] {msg}");
		}

		public void LogError(object msg, object context)
		{
			LogError(msg, context.GetType());
		}

		public void LogError(object msg, Type type)
		{
			Debug.LogError($"[{Now()}]<b>[{type}]</b> {msg}");
		}

		private string Now()
		{
			return DateTime.Now.ToString("HH:mm:ss");
		}
	}
}