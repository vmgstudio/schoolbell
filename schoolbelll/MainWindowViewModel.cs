 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schoolbelll
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> scheduleList;
        

        public MainWindowViewModel()
        {
            scheduleList = new List<string>();
            
        }

        public List<string> ScheduleList 
        {
            get { return scheduleList; }
            set
            {
                if (scheduleList != value)
                {
                    scheduleList = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                }
            }
        }


    }
}
