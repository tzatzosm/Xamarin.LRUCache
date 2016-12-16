using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Xamarin.LRUCache.NUnitTests
{
	[TestFixture()]
	public class Test
	{
		
		[Test()]
		public void TestGet()
		{
			LRUCache<string, string> cache = new LRUCache<string, string>(1024);

			for (int i = 0; i < 100; i++)
			{
				cache.Put(i.ToString(), i.ToString());
			}

			for (int i = 0; i < 100; i++)
			{
				Assert.AreEqual(i.ToString(), cache.Get(i.ToString()));
			}
		}


		[Test()]
		public void TestUpdated()
		{
			LRUCache<string, string> cache = new LRUCache<string, string>(1024);

			for (int i = 0; i < 200; i++)
			{
				cache.Put((i % 100).ToString(), i.ToString());
			}

			for (int i = 100; i < 200; i++)
			{
				Assert.AreEqual(cache.Get((i % 100).ToString()), i.ToString());
			}
		}

		[Test()]
		public void TestEviction()
		{
			LRUCache<string, string> cache = new LRUCache<string, string>(100);

			for (int i = 0; i < 1000; i++)
			{
				cache.Put(i.ToString(), i.ToString());
			}

			Assert.AreEqual(cache.Size, 100);
			Assert.AreEqual(cache.EvictionCount, 900);

			for (int i = 0; i < 900; i++)
			{
				Assert.AreEqual(cache.Get(i.ToString()), null);
			}

			for (int i = 900; i < 1000; i++)
			{
				Assert.AreEqual(cache.Get(i.ToString()), i.ToString());
			}
		}


	}
}
