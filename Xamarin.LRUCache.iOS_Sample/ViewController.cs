using System;

using UIKit;

namespace Xamarin.LRUCache.iOS_Sample
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LRUCache<string, string> cache = new LRUCache<string, string>(10);

			cache.Put("a", "1");
			cache.Put("b", "1");
			cache.Put("c", "1");
			cache.Put("d", "1");
			cache.Put("a", "1");
			cache.Put("a", "1");
			cache.Put("a", "1");
			cache.Put("a", "1");

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
