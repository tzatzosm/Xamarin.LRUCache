using System;
namespace Xamarin.LRUCache
{
	public interface ISizable<TValue>
		where TValue : class
	{
		/// <summary>
		/// Returns the size of the value. Since we cannot 
		/// measure the size of a class in c# we provide this 
		/// implementation for memory intense applications.
		/// </summary>
		/// <returns>The size in bytes for the value.</returns>
		/// <param name="val">Value</param>
		int SizeOf(TValue val);
	}
}
