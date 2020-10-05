using System;
using System.IO;
using System.Xml;
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
using System.Windows.Threading;
using System.Media;

namespace schoolbelll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MediaPlayer mediaPlayer = new MediaPlayer();
        private int minutes;

        private string filename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "schedule.xml");

        XmlDocument DataFile;

        private string jelzohang;
        private string becsengeteshang;
        private string kicsengeteshang;

        private bool mostNeMukodjel;

        MainWindowViewModel mainwindowviewmodel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = mainwindowviewmodel;

            AppEventHandler.ScheduleAdded += OnScheduleAdded;

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();

            LoadDataFile();

            LoadScheduleList();
            ApplySelectedSchedule(GetDefaultSchedule());

        }

        void timer_Tick(object sender, EventArgs e)
        {
            LiveTimeLabel.Content = DateTime.Now.ToString("HH:mm:ss");

            if (DateTime.Now.TimeOfDay.Minutes > minutes)
            {
                minutes = DateTime.Now.TimeOfDay.Minutes;

                if (mainwindowviewmodel.jelzocsengetesek.Contains(DateTime.Now.ToString("HH:mm")))
                {
                    mediaPlayer.Open(new Uri(jelzohang));
                    mediaPlayer.Play();
                }

                if (mainwindowviewmodel.becsengetesek.Contains(DateTime.Now.ToString("HH:mm")))
                {
                    mediaPlayer.Open(new Uri(becsengeteshang));
                    mediaPlayer.Play();
                }

                if (mainwindowviewmodel.kicsengetesek.Contains(DateTime.Now.ToString("HH:mm")))
                {
                    mediaPlayer.Open(new Uri(kicsengeteshang));
                    mediaPlayer.Play();
                }
            }


        }

        private void addSchedule_Click(object sender, RoutedEventArgs e)
        {
            AddSchedule dlg = new AddSchedule();

            dlg.Owner = this;

            dlg.ShowDialog();

        }

        private void LoadDataFile()
        {
            XmlDocument _document = new XmlDocument();

            try
            {
                _document.Load(filename);
                DataFile = _document;
                Console.WriteLine("DataFile betöltve.");
            }
            catch (System.IO.FileNotFoundException)
            {
                XmlDeclaration xmlDeclaration = _document.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = _document.DocumentElement;
                _document.InsertBefore(xmlDeclaration, root);

                XmlElement body = _document.CreateElement(string.Empty, "root", string.Empty);
                _document.AppendChild(body);

                XmlElement currentschedule = _document.CreateElement("currentschedule");
                body.AppendChild(currentschedule);

                _document.Save(filename);
                DataFile = _document;
                Console.WriteLine("DataFile Létrehozva, betöltve.");
            }


        }


        private void LoadScheduleList()
        {
            XmlNodeList nodeList;

            nodeList = DataFile.SelectNodes("/root/schedule");

            mostNeMukodjel = true;

            mainwindowviewmodel.ScheduleList = new List<string>();

            foreach (XmlNode schedule in nodeList)
            {
                string schedulename = schedule["title"].InnerText;

                mainwindowviewmodel.ScheduleList.Add(schedulename);

                Console.WriteLine("Added " + schedulename); //debug
            }

            selectSchedule.SelectedIndex = mainwindowviewmodel.ScheduleList.IndexOf(GetDefaultSchedule());

            mostNeMukodjel = false;
        }


        private void ApplySelectedSchedule(string ScheduleName) //beállít egy megadott nevű csengetést
        {
            XmlNode root = DataFile.DocumentElement;

            mainwindowviewmodel.jelzocsengetesek = new List<string>();
            mainwindowviewmodel.becsengetesek = new List<string>();
            mainwindowviewmodel.kicsengetesek = new List<string>();

            XmlNodeList lessons = root.SelectNodes("/root/schedule[./title[contains(text(), '" + ScheduleName + "')]]//lesson[@id]");
            foreach (XmlNode lesson in lessons)
            {
                mainwindowviewmodel.jelzocsengetesek.Add(lesson["jelzo"].InnerText);
                mainwindowviewmodel.becsengetesek.Add(lesson["becsengetes"].InnerText);
                mainwindowviewmodel.kicsengetesek.Add(lesson["kicsengetes"].InnerText);

            }

            XmlNode hangok = root.SelectSingleNode("/root/schedule[./title[contains(text(), '" + ScheduleName + "')]]/sound");
            if (hangok != null)
            {
                jelzohang = hangok["jelzo"].InnerText;
                becsengeteshang = hangok["becsengetes"].InnerText;
                kicsengeteshang = hangok["kicsengetes"].InnerText;
            }

            Console.WriteLine(ScheduleName + " nevű csengetés aktiválva.");

        }

        private string GetDefaultSchedule()
        {
            string currentschedule = "";

            XmlNode root = DataFile.DocumentElement;

            try
            {
                currentschedule = root.SelectSingleNode("/root/currentschedule").InnerText;

            }
            catch (Exception)
            {
                root.AppendChild(DataFile.CreateElement("currentschedule"));
            }

            return currentschedule;
        }

        private void SetDefaultSchedule(string scheduleName)
        {
            string selectedschedule = selectSchedule.SelectedItem.ToString();

            XmlNode root = DataFile.DocumentElement;
            XmlNode current = root.SelectSingleNode("/root/currentschedule");
            current.InnerText = selectedschedule;

            DataFile.Save(filename); //Elmentjük a módosított XML-t. 

            MessageBox.Show("Mostantól a(z) " + selectedschedule + " csengetési rend van érvényben.", "Sikeres módosítás", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void selectSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (selectSchedule.SelectedItem != null && !mostNeMukodjel)
            {
                SetDefaultSchedule(selectSchedule.SelectedItem.ToString()); //Fájlba menjük az új csengetést

                ApplySelectedSchedule(GetDefaultSchedule()); //Betöltjük a kiválasztott csengetést.
            }
        }


        private void OnScheduleAdded(object sender, EventArgs e)
        {
            LoadDataFile(); //Írtunk az XML-be szóval betöltjük újra.
            LoadScheduleList(); //Újra betöltjük a legördülő listát. DE MIÉRT LESZ ÜRES??
            Console.WriteLine("OnScheduleAdded Event lefutott");
        }
    }
}
