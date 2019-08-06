using NUnit.Framework;
using UnityEngine;

namespace SimpleInjection.Tests
{
	public class UnityInjectorTests
	{
		[Test]
		public void UnityInjectorGetTestDependencyResolved()
		{
			var g = new GameObject();
			var t1 = g.AddComponent<ClassGet>();
			var t2 = g.AddComponent<ClassGetOptional>();
			var rb = g.AddComponent<Rigidbody>();

			SI.Inject(t1);
			SI.Inject(t2);

			Assert.AreSame(t1.someValue, rb);
			Assert.AreSame(t2.someValue, rb);

			Object.Destroy(g);
		}

		[Test]
		public void UnityInjectorGetChildrenTestDependencyResolved()
		{
			var g = new GameObject();
			var children = new GameObject();
			children.transform.SetParent(g.transform);

			var t1 = g.AddComponent<ClassGetChildren>();
			var t2 = g.AddComponent<ClassGetChildrenOptional>();
			var rb = children.AddComponent<Rigidbody>();

			SI.Inject(t1);
			SI.Inject(t2);

			Assert.AreSame(t1.someValue, rb);
			Assert.AreSame(t2.someValue, rb);

			Object.Destroy(g);
		}

		[Test]
		public void UnityInjectorGetParentTestDependencyResolved()
		{
			var g = new GameObject();
			var parent = new GameObject();
			g.transform.SetParent(parent.transform);

			var t1 = g.AddComponent<ClassGetParent>();
			var t2 = g.AddComponent<ClassGetParentOptional>();
			var rb = parent.AddComponent<Rigidbody>();

			SI.Inject(t1);
			SI.Inject(t2);

			Assert.AreSame(t1.someValue, rb);
			Assert.AreSame(t2.someValue, rb);

			Object.Destroy(parent);
		}

		[Test]
		public void UnityInjectorGetTestDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassGet>();
			Assert.Throws(typeof(DependencyNotResolvedException), () => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		[Test]
		public void UnityInjectorGetTestOptionalDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassGetOptional>();
			Assert.DoesNotThrow(() => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		[Test]
		public void UnityInjectorGetChildrenTestDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassGetChildren>();
			Assert.Throws(typeof(DependencyNotResolvedException), () => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		[Test]
		public void UnityInjectorGetChildrenTestOptionalDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassGetChildrenOptional>();
			Assert.DoesNotThrow(() => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		[Test]
		public void UnityInjectorGetParentTestDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassGetParent>();
			Assert.Throws(typeof(DependencyNotResolvedException), () => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		[Test]
		public void UnityInjectorGetParentTestOptionalDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassGetParentOptional>();
			Assert.DoesNotThrow(() => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		[Test]
		public void UnityInjectorFindTestDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassFind>();
			Assert.Throws(typeof(DependencyNotResolvedException), () => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		[Test]
		public void UnityInjectorFindTestOptionalDependencyNotResolved()
		{
			var target = CreateGameObjectWithComponent<ClassFindOptional>();
			Assert.DoesNotThrow(() => SI.Inject(target));
			Object.Destroy(target.gameObject);
		}

		private T CreateGameObjectWithComponent<T>() where T : Component
		{
			GameObject g = new GameObject();
			return g.AddComponent<T>();
		}

		private class ClassGet : MonoBehaviour
		{
			[Get] public Rigidbody someValue;
		}

		private class ClassGetOptional : MonoBehaviour
		{
			[Get(optional = true)] public Rigidbody someValue;
		}

		private class ClassGetChildren : MonoBehaviour
		{
			[Get(GetSource.Children)] public Rigidbody someValue;
		}

		private class ClassGetChildrenOptional : MonoBehaviour
		{
			[Get(GetSource.Children, optional = true)] public Rigidbody someValue;
		}

		private class ClassGetParent : MonoBehaviour
		{
			[Get(GetSource.Parent)] public Rigidbody someValue;
		}

		private class ClassGetParentOptional : MonoBehaviour
		{
			[Get(GetSource.Parent, optional = true)] public Rigidbody someValue;
		}

		private class ClassFind : MonoBehaviour
		{
			[Find] public Rigidbody someValue;
		}

		private class ClassFindOptional : MonoBehaviour
		{
			[Find(optional = true)] public Rigidbody someValue;
		}
	}
}