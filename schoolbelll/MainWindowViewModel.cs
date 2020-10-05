 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schoolbelll
{
    public class MainWindowViewModel : ViewModel
    {
        private List<string> _scheduleList;
        

        public MainWindowViewModel()
        {
            _scheduleList = new List<string>();
            
        }

        public List<string> ScheduleList 
        {
            get { return _scheduleList; }
            set
            {
                if (_scheduleList != value)
                {
                    _scheduleList = value;
                    OnPropertyChanged("ScheduleList");
                }
            }
        }


    }
}
