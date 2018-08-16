using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VirtualListBoxLib;

namespace Demo
{
	public class VirtualCollection :DependencyObject, IVirtualCollection
	{
		private Timer timer;

		public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(int), typeof(VirtualCollection));
		public int ItemsCount
		{
			get { return (int)GetValue(ItemsCountProperty); }
			set { SetValue(ItemsCountProperty, value); }
		}

		public VirtualCollection()
		{
			ItemsCount = 100;
			//timer = new Timer(timer_CallBack, null, 0, 1000);
			
		}

		private void timer_CallBack(object state)
		{
			Dispatcher.Invoke(() => ItemsCount++);
		}

		public object GetItem(int Index)
		{
			return $"Item {Index}";
		}
	}
}
