﻿using System;
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
        private List<string> _jelzocsengetesek;
        private List<string> _becsengetesek;
        private List<string> _kicsengetesek;
        public List<string> jelzocsengetesek
        {
            get { return _jelzocsengetesek; }
            set
            {
                _jelzocsengetesek = value;
                OnPropertyChanged("jelzocsengetesek");
            }
        }
        public List<string> becsengetesek
        {
            get { return _becsengetesek; }
            set
            {
                _becsengetesek = value;
                OnPropertyChanged("becsengetesek");
            }
        }
        public List<string> kicsengetesek
        {
            get { return _kicsengetesek; }
            set
            {
                _kicsengetesek = value;
                OnPropertyChanged("kicsengetesek");
            }
        }


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
