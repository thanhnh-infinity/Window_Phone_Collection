using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using DanielVaughan.WindowsPhone7Unleashed;

namespace DanielVaughan.ScrollViewerMonitor
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		public MainPageViewModel()
		{
			AddMoreItems();

			fetchMoreDataCommand = new DelegateCommand(
				obj =>
					{
						if (busy)
						{
							return;
						}
						Busy = true;
						ThreadPool.QueueUserWorkItem(
							delegate
						    {
								/* This is just to demonstrate a slow operation. */
						        Thread.Sleep(3000);
								/* We invoke back to the UI thread. 
								 * Ordinarily this would be done 
								 * by the Calcium infrastructure automatically. */
						        Deployment.Current.Dispatcher.BeginInvoke(
									delegate
						            {
										AddMoreItems();
						            	Busy = false;
						            });
						    });
					
				});
		}

		void AddMoreItems()
		{
			int start = items.Count;
			int end = start + 10;
			for (int i = start; i < end; i++)
			{
				items.Add("Item " + i);
			}
		}

		readonly DelegateCommand fetchMoreDataCommand;

		public ICommand FetchMoreDataCommand
		{
			get
			{
				return fetchMoreDataCommand;
			}
		}

		readonly ObservableCollection<string> items = new ObservableCollection<string>();

		public ObservableCollection<string> Items
		{
			get
			{
				return items;
			}
		}

		bool busy;

		public bool Busy
		{
			get
			{
				return busy;
			}
			set
			{
				if (busy == value)
				{
					return;
				}
				busy = value;
				OnPropertyChanged(new PropertyChangedEventArgs("Busy"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var tempEvent = PropertyChanged;
			if (tempEvent != null)
			{
				tempEvent(this, e);
			}
		}
	}


}
