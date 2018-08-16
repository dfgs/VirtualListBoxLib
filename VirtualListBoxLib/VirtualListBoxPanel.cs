using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
	/// Suivez les étapes 1a ou 1b puis 2 pour utiliser ce contrôle personnalisé dans un fichier XAML.
	///
	/// Étape 1a) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans le projet actif.
	/// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
	/// être utilisé :
	///
	///     xmlns:MyNamespace="clr-namespace:VirtualListBoxLib"
	///
	///
	/// Étape 1b) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans un autre projet.
	/// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
	/// être utilisé :
	///
	///     xmlns:MyNamespace="clr-namespace:VirtualListBoxLib;assembly=VirtualListBoxLib"
	///
	/// Vous devrez également ajouter une référence du projet contenant le fichier XAML
	/// à ce projet et regénérer pour éviter des erreurs de compilation :
	///
	///     Cliquez avec le bouton droit sur le projet cible dans l'Explorateur de solutions, puis sur
	///     "Ajouter une référence"->"Projets"->[Recherchez et sélectionnez ce projet]
	///
	///
	/// Étape 2)
	/// Utilisez à présent votre contrôle dans le fichier XAML.
	///
	///     <MyNamespace:VirtualListBoxPanel/>
	///
	/// </summary>
	public class VirtualListBoxPanel : VirtualizingPanel, IScrollInfo
	{
		public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(int), typeof(VirtualListBoxPanel), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public int ItemsCount
		{
			get { return (int)GetValue(ItemsCountProperty); }
			set { SetValue(ItemsCountProperty, value); }
		}


		public static readonly DependencyProperty VirtualCollectionProperty = DependencyProperty.Register("VirtualCollection", typeof(IVirtualCollection), typeof(VirtualListBoxPanel), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, VirtualCollectionPropertyChanged));
		public IVirtualCollection VirtualCollection
		{
			get { return (IVirtualCollection)GetValue(VirtualCollectionProperty); }
			set { SetValue(VirtualCollectionProperty, value); }
		}


		public static readonly DependencyProperty ItemsHeightProperty = DependencyProperty.Register("ItemsHeight", typeof(double), typeof(VirtualListBoxPanel), new FrameworkPropertyMetadata(20.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public double ItemsHeight
		{
			get { return (double)GetValue(ItemsHeightProperty); }
			set { SetValue(ItemsHeightProperty, value); }
		}


		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(VirtualListBoxPanel));
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set { SetValue(ItemTemplateProperty, value); }
		}



		public static readonly DependencyProperty FirstItemIndexProperty = DependencyProperty.Register("FirstItemIndex", typeof(int), typeof(VirtualListBoxPanel));
		public int FirstItemIndex
		{
			get { return (int)GetValue(FirstItemIndexProperty); }
			private set { SetValue(FirstItemIndexProperty, value); }
		}



		public static readonly DependencyProperty SelectedItemIndexProperty = DependencyProperty.Register("SelectedItemIndex", typeof(int), typeof(VirtualListBoxPanel),new PropertyMetadata(-1,SelectedItemIndexPropertyChanged));
		public int SelectedItemIndex
		{
			get { return (int)GetValue(SelectedItemIndexProperty); }
			set { SetValue(SelectedItemIndexProperty, value); }
		}







		static VirtualListBoxPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(VirtualListBoxPanel), new FrameworkPropertyMetadata(typeof(VirtualListBoxPanel)));
		}



		#region IScrollInfo
		public bool CanVerticallyScroll { get; set; }
		public bool CanHorizontallyScroll { get; set; }

		public static readonly DependencyProperty ExtentWidthProperty = DependencyProperty.Register("ExtentWidth", typeof(double), typeof(VirtualListBoxPanel), new FrameworkPropertyMetadata(200.0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public double ExtentWidth
		{
			get { return (double)GetValue(ExtentWidthProperty); }
			set { SetValue(ExtentWidthProperty, value); }
		}

		//private Dictionary<int, double> rowHeights;
		public double ExtentHeight
		{
			get
			{
				return ItemsCount*ItemsHeight;
			}
		}
		public double ViewportWidth { get; private set; }
		public double ViewportHeight { get; private set; }


		public static readonly DependencyProperty HorizontalOffsetProperty = DependencyProperty.Register("HorizontalOffset", typeof(double), typeof(VirtualListBoxPanel), new FrameworkPropertyMetadata(0d));
		public double HorizontalOffset
		{
			get { return (double)GetValue(HorizontalOffsetProperty); }
			set { SetValue(HorizontalOffsetProperty, value); }
		}


		public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset", typeof(double), typeof(VirtualListBoxPanel), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public double VerticalOffset
		{
			get { return (double)GetValue(VerticalOffsetProperty); }
			set { SetValue(VerticalOffsetProperty, value); }
		}

		public ScrollViewer ScrollOwner { get; set; }
		#endregion


		public VirtualListBoxPanel()
		{
		}


		private static void VirtualCollectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((VirtualListBoxPanel)d).OnVirtualCollectionChanged();
		}
		protected virtual void OnVirtualCollectionChanged()
		{
			RemoveInternalChildRange(0, Children.Count);
			FirstItemIndex = 0;
			SelectedItemIndex = -1;
		}



	
		private static void SelectedItemIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((VirtualListBoxPanel)d).OnSelectedItemIndexChanged();
		}
		protected virtual void OnSelectedItemIndexChanged()
		{
			VirtualListBoxItem item;
			int childIndex;

			foreach(VirtualListBoxItem child in Children)
			{
				child.IsSelected = false;
			}

			if ((SelectedItemIndex < 0) || (SelectedItemIndex >= ItemsCount)) return;

			if (SelectedItemIndex < FirstItemIndex)
			{
				SetVerticalOffset(SelectedItemIndex * ItemsHeight);
				// we cannot set IsSelected on this item, because OnMeasure has not been called yet
			} else if (SelectedItemIndex >= FirstItemIndex + Children.Count)
			{
				SetVerticalOffset((SelectedItemIndex - Children.Count+1 ) * ItemsHeight);
				// we cannot set IsSelected on this item, because OnMeasure has not been called yet
			}
			else
			{
				childIndex = SelectedItemIndex - FirstItemIndex;
				item = (VirtualListBoxItem)Children[childIndex];
				item.IsSelected = true;
				item.Focus();
			}

		}



		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			VirtualListBoxItem item;

			e.Handled = false;

			item = e.Source as VirtualListBoxItem;
			if (item == null) return;
			
			SelectedItemIndex = FirstItemIndex + Children.IndexOf(item);
			e.Handled = true;
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			int pageStep;

			pageStep = (int)(ViewportHeight / ItemsHeight)/2;
			

			e.Handled = false;
			switch(e.Key)
			{
				case Key.Down:
					if (SelectedItemIndex < ItemsCount - 1) SelectedItemIndex++;
					e.Handled = true;
					break;
				case Key.Up:
					if (SelectedItemIndex > 0) SelectedItemIndex--;
					e.Handled = true;
					break;
				case Key.PageDown:
					if (SelectedItemIndex+pageStep > ItemsCount - 1) SelectedItemIndex = ItemsCount - 1;
					else SelectedItemIndex+=pageStep;
					e.Handled = true;
					break;
				case Key.PageUp:
					if (SelectedItemIndex - pageStep < 0) SelectedItemIndex = 0;
					else SelectedItemIndex-=pageStep;
					e.Handled = true;
					break;


			}

		}





		protected VirtualListBoxItem OnCreateChildItem(object DataContext,int ItemIndex)
		{
			VirtualListBoxItem item;

			item = new  VirtualListBoxItem();
			item.DataContext = DataContext;
			item.Content = DataContext;
			item.IsSelected = (ItemIndex == SelectedItemIndex);
			return item;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			int itemIndex, childIndex;
			VirtualListBoxItem item;
			int delta;
			int visibleItemsCount;
			int maxItemIndex;


			visibleItemsCount = (int)(availableSize.Height / ItemsHeight);
			itemIndex = (int)(VerticalOffset / ItemsHeight);

			maxItemIndex = itemIndex + visibleItemsCount;
			if (maxItemIndex >= ItemsCount) maxItemIndex = maxItemIndex - 1;

			if (itemIndex >= FirstItemIndex)
			{
				// remove children that are above top
				delta = itemIndex - FirstItemIndex;
				RemoveInternalChildRange(0, delta);

				// measure already created children
				childIndex = 0;
				while ((itemIndex < ItemsCount) && (childIndex < Children.Count) && (itemIndex<=maxItemIndex))
				{
					item = (VirtualListBoxItem)Children[childIndex];
					item.Measure(new Size(availableSize.Width, ItemsHeight));
					itemIndex++; childIndex++;
				}
				// create and measure new children is needed
				while ((itemIndex < ItemsCount) && (itemIndex <= maxItemIndex))
				{
					item = OnCreateChildItem(VirtualCollection.GetItem(itemIndex),itemIndex);
					AddInternalChild(item);
					if (item.IsSelected) item.Focus();
					item.Measure(new Size(availableSize.Width, ItemsHeight));
					itemIndex++; childIndex++;
				}
				// remove children that are below bottom
				delta = Children.Count - childIndex;
				RemoveInternalChildRange(childIndex, delta);
			}
			else
			{
				// create and measure new children is needed
				childIndex = 0;
				while ((itemIndex < ItemsCount) && (itemIndex < FirstItemIndex) && (itemIndex <= maxItemIndex))
				{
					item = OnCreateChildItem(VirtualCollection.GetItem(itemIndex), itemIndex);
					InsertInternalChild(childIndex, item);
					if (item.IsSelected) item.Focus();
					item.Measure(new Size(availableSize.Width, availableSize.Height));
					itemIndex++; childIndex++;
				}
				// measure already created children
				while ((itemIndex < ItemsCount) && (childIndex < Children.Count) && (itemIndex <= maxItemIndex))
				{
					item = (VirtualListBoxItem)Children[childIndex];
					item.Measure(new Size(availableSize.Width, availableSize.Height));
					itemIndex++; childIndex++;
				}
				// remove children that are below bottom
				delta = Children.Count - childIndex;
				RemoveInternalChildRange(childIndex, delta);
			}


			FirstItemIndex = (int)(VerticalOffset/ItemsHeight);
			ViewportHeight = visibleItemsCount*ItemsHeight;
			ViewportWidth = availableSize.Width;
			SetVerticalOffset(VerticalOffset);	// in order to adjust position when sizing control height

			return availableSize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			int childIndex;

			childIndex = 0;
			foreach (UIElement child in InternalChildren)
			{
				child.Arrange(new Rect(0, childIndex*ItemsHeight, ViewportWidth, ItemsHeight));
				childIndex++;
			}

			return DesiredSize;
		}


		#region IScrollInfo

		public void LineUp()
		{
			SetVerticalOffset(VerticalOffset - ItemsHeight);
		}

		public void LineDown()
		{
			SetVerticalOffset(VerticalOffset + ItemsHeight);
		}

		public void LineLeft()
		{
			SetHorizontalOffset(HorizontalOffset - 1);
		}

		public void LineRight()
		{
			SetHorizontalOffset(HorizontalOffset + 1);
		}

		public void PageUp()
		{
			SetVerticalOffset(VerticalOffset - ViewportHeight / 2);
		}

		public void PageDown()
		{
			SetVerticalOffset(VerticalOffset + ViewportHeight / 2);
		}

		public void PageLeft()
		{
			SetHorizontalOffset(HorizontalOffset - ViewportWidth);
		}

		public void PageRight()
		{
			SetHorizontalOffset(HorizontalOffset + ViewportWidth);
		}

		public void MouseWheelUp()
		{
			SetVerticalOffset(VerticalOffset - ItemsHeight);
		}

		public void MouseWheelDown()
		{
			SetVerticalOffset(VerticalOffset + ItemsHeight);
		}

		public void MouseWheelLeft()
		{
			SetHorizontalOffset(HorizontalOffset - 10);
		}

		public void MouseWheelRight()
		{
			SetHorizontalOffset(HorizontalOffset + 10);
		}

		public void SetHorizontalOffset(double offset)
		{
			if (offset < 0 || ViewportWidth >= ExtentWidth)
			{
				offset = 0;
			}
			else
			{
				if (offset + ViewportWidth >= ExtentWidth)
				{
					offset = ExtentWidth - ViewportWidth;
				}
			}

			HorizontalOffset = (int)offset;

			ScrollOwner?.InvalidateScrollInfo();
		}

		public void SetVerticalOffset(double offset)
		{
			if (offset < 0 || ViewportHeight >= ExtentHeight)
			{
				offset = 0;
			}
			else
			{
				if (offset + ViewportHeight >= ExtentHeight)
				{
					offset = ExtentHeight - ViewportHeight;
				}
			}

			VerticalOffset = (int)offset;

			ScrollOwner?.InvalidateScrollInfo();
		}

		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			//throw new NotImplementedException();
			return Rect.Empty;
		}


		#endregion


	}
}
