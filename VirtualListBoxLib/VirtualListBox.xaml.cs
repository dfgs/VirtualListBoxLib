using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VirtualListBoxLib
{
	/// <summary>
	/// Logique d'interaction pour VirtualListBox.xaml
	/// </summary>
	public partial class VirtualListBox : UserControl
	{
		public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(int), typeof(VirtualListBox), new FrameworkPropertyMetadata(0));
		public int ItemsCount
		{
			get { return (int)GetValue(ItemsCountProperty); }
			set { SetValue(ItemsCountProperty, value); }
		}

		public static readonly DependencyProperty VirtualCollectionProperty = DependencyProperty.Register("VirtualCollection", typeof(IVirtualCollection), typeof(VirtualListBox), new FrameworkPropertyMetadata(null));
		public IVirtualCollection VirtualCollection
		{
			get { return (IVirtualCollection)GetValue(VirtualCollectionProperty); }
			set { SetValue(VirtualCollectionProperty, value); }
		}


		public static readonly DependencyProperty ItemsHeightProperty = DependencyProperty.Register("ItemsHeight", typeof(double), typeof(VirtualListBox), new FrameworkPropertyMetadata(20.0d));
		public double ItemsHeight
		{
			get { return (double)GetValue(ItemsHeightProperty); }
			set { SetValue(ItemsHeightProperty, value); }
		}


		public VirtualListBox()
		{
			InitializeComponent();
		}
	}
}
