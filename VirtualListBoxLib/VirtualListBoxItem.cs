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
	///     <MyNamespace:VirtualListBoxItem/>
	///
	/// </summary>
	public class VirtualListBoxItem : ContentControl
	{


		public static readonly DependencyProperty HooveredBorderBrushProperty = DependencyProperty.Register("HooveredBorderBrush", typeof(Brush), typeof(VirtualListBoxItem));
		public Brush HooveredBorderBrush
		{
			get { return (Brush)GetValue(HooveredBorderBrushProperty); }
			set { SetValue(HooveredBorderBrushProperty, value); }
		}


		public static readonly DependencyProperty HooveredBackgroundProperty = DependencyProperty.Register("HooveredBackground", typeof(Brush), typeof(VirtualListBoxItem));
		public Brush HooveredBackground
		{
			get { return (Brush)GetValue(HooveredBackgroundProperty); }
			set { SetValue(HooveredBackgroundProperty, value); }
		}


		public static readonly DependencyProperty SelectedBorderBrushProperty = DependencyProperty.Register("SelectedBorderBrush", typeof(Brush), typeof(VirtualListBoxItem));
		public Brush SelectedBorderBrush
		{
			get { return (Brush)GetValue(SelectedBorderBrushProperty); }
			set { SetValue(SelectedBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(VirtualListBoxItem));
		public Brush SelectedBackground
		{
			get { return (Brush)GetValue(SelectedBackgroundProperty); }
			set { SetValue(SelectedBackgroundProperty, value); }
		}


		public static readonly DependencyProperty InactiveBorderBrushProperty = DependencyProperty.Register("InactiveBorderBrush", typeof(Brush), typeof(VirtualListBoxItem));
		public Brush InactiveBorderBrush
		{
			get { return (Brush)GetValue(InactiveBorderBrushProperty); }
			set { SetValue(InactiveBorderBrushProperty, value); }
		}

		public static readonly DependencyProperty InactiveBackgroundProperty = DependencyProperty.Register("InactiveBackground", typeof(Brush), typeof(VirtualListBoxItem));
		public Brush InactiveBackground
		{
			get { return (Brush)GetValue(InactiveBackgroundProperty); }
			set { SetValue(InactiveBackgroundProperty, value); }
		}



		public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(VirtualListBoxItem));
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
			set { SetValue(IsSelectedProperty, value); }
		}

		static VirtualListBoxItem()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(VirtualListBoxItem), new FrameworkPropertyMetadata(typeof(VirtualListBoxItem)));
		}
	}
}
