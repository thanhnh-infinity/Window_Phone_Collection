using Microsoft.Phone.Controls;

namespace DanielVaughan.ScrollViewerMonitor
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();
			DataContext = new MainPageViewModel();
		}
	}
}