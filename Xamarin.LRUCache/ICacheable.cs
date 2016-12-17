using System;
namespace Xamarin.LRUCache
{
	public interface ICacheable<TKey, TValue> 
		where TKey : class
		where TValue : class
	{
		/// <summary>
		/// returns true if the cache contains the given key.
		/// </summary>
		/// <param name="key">Key.</param>
		bool Contains(TKey key);

		/// <summary>
		/// Returns the cached value for the given key.
		/// </summary>
		/// <param name="key">Key.</param>
		TValue Get(TKey key);

		/// <summary>
		/// Removes and returns the cached value for the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		TValue Remove(TKey key);

		/// <summary>
		/// Stores the value for the given key.
		/// </summary>
		/// <param name="key">Key to specify the value.</param>
		/// <param name="val">Value.</param>
		void Put(TKey key, TValue val);

		/// <summary>
		/// Clears all the entries cached.
		/// </summary>
		void Clear();

	}
}
