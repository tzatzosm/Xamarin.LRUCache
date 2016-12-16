using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Xamarin.LRUCache
{
	public class LRUCache<TKey, TValue> : ICacheable<TKey, TValue>, ISizable<TValue>
		where TKey : class
		where TValue : class
	{

		#region Properties

		/// <summary>
		/// Max size of the cache.
		/// </summary>
		private int _maxSize;
		public int MaxSize
		{
			get { return _maxSize; }
		}

		/// <summary>
		/// Current size of the cache.
		/// </summary>
		private int _size = 0;
		public int Size
		{
			get { return _size; }
		}

		/// <summary>
		/// The count of entries evicted 
		/// during the lifetime of the cache object's instance.
		/// </summary>
		private int _evictionCount = 0;
		public int EvictionCount
		{
			get { return _evictionCount; }
		}

		private int _hitCount = 0;
		public int HitCount
		{
			get { return _hitCount; }
		}

		private int _missCount = 0;
		public int MissCount
		{
			get { return _missCount; }
		}

		/// <summary>
		/// The dictionary with the cache values.
		/// </summary>
		private Dictionary<TKey, TValue> _dictionary;

		/// <summary>
		/// The dictionary with the queued values. 
		/// When an item is used, it will be pushed in the queue.
		/// Helps with the LRU eviction policy.
		/// Due to the nature of the queue during the eviction 
		/// there might be keys that are not mapped to a value.
		/// </summary>
		private Queue<TKey> _queue;

		#endregion

		#region Constructor

		public LRUCache(int maxSize)
		{
			if (maxSize <= 0)
			{
				throw new ArgumentException("maxSize <= 0");
			}
			_maxSize = maxSize;
			_dictionary = new Dictionary<TKey, TValue>();
			_queue = new Queue<TKey>();
		}

		#endregion

		#region ICache

		/// <summary>
		/// Clears all the entries cached.
		/// </summary>
		public void Clear()
		{
			Trim(-1);
		}

		/// <summary>
		/// Returns the value for the given key 
		/// found in the in-memory dictionary or 
		/// null if there isn't one.
		/// </summary>
		/// <param name="key">Key</param>
		public TValue Get(TKey key)
		{
			// Value for key is not in the dictionary.
			if (!_dictionary.ContainsKey(key))
			{
				_missCount++;
				return null;
			}
			var val = _dictionary[key];
			_hitCount++;

			// If a value was found, we simply 
			// append the key to our queue.
			// Our eviction policy will 
			// take care of the duplicate keys by checking 
			// if the queue contains a key.
			// FIXME : find a data structure better suited for this case.
			_queue.Enqueue(key);
			return val;
		}

		/// <summary>
		/// Removes and returns the value for the specific key.
		/// If no value found for the specific key returns null instead.
		/// </summary>
		/// <param name="key">Key.</param>
		public TValue Remove(TKey key)
		{
			var val = _dictionary[key];

			// If value is not null we 
			// remove it from the dictionary.
			if (val != null)
			{
				_dictionary.Remove(key);
				_size -= SizeOf(val);
			}

			return val;
		}

		/// <summary>
		/// Stores the value for the given key. 
		/// Both cannot be null.
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="val">val</param>
		public void Put(TKey key, TValue val)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key));
			}
			if (val == null)
			{
				throw new ArgumentNullException(nameof(val));
			}
			// If the dictionary already contains a value 
			// for the key we remove that value
			// so that it gets updated.
			// Important : We have to also reduce the total size.
			if (_dictionary.ContainsKey(key))
			{
				var oldVal = _dictionary[key];
				_dictionary.Remove(key);
				_size -= SizeOf(oldVal);
			}
			_dictionary.Add(key, val);
			_size += SizeOf(val);
			_queue.Enqueue(key);
			Trim(MaxSize);
		}

		#endregion

		#region ISizeable

		/// <summary>
		/// Returns 1 as a default implementation for the size, 
		/// since we cannot measure the size of a class efficiently.
		/// </summary>
		/// <returns>1</returns>
		/// <param name="val">val</param>
		public virtual int SizeOf(TValue val)
		{
			return 1;
		}

		#endregion

		#region Private Functions

		private void Trim(int maxSize)
		{
			while (_size > maxSize && _dictionary.Count > 0)
			{
				// if queue is null we break the loop. 
				// We dont have any keys left.
				if (_queue.Count == 0)
				{
					break;
				}

				var keyToEvict = _queue.Dequeue();

				// If the queue contains the keyToEvict, 
				// after it has been dequeued, 
				// we shouldn't remove it.
				if ( _queue.Contains(keyToEvict))
				{
					continue;
				}

				Remove(keyToEvict);
				_evictionCount++;
			}
		}

		#endregion


	}
}
