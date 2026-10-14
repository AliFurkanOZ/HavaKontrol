using System.Collections.Specialized;
using System.Windows.Controls;

namespace hava_kontrol.Views
{
    /// <summary>
    /// Interaction logic for EventLogView.xaml
    /// </summary>
    public partial class EventLogView : UserControl
    {
        public EventLogView()
        {
            InitializeComponent();
            if (LogListView.Items is INotifyCollectionChanged notify)
            {
                notify.CollectionChanged += (s, e) =>
                {
                    if (e.Action == NotifyCollectionChangedAction.Add && LogListView.Items.Count > 0)
                    {
                        LogListView.ScrollIntoView(LogListView.Items[0]);
                    }
                };
            }
        }
    }
}
