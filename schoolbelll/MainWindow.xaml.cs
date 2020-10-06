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
using System.Collections.ObjectModel;

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

        private bool mostNeMukodjel = false;

        MainWindowViewModel mainwindowviewmodel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = mainwindowviewmodel;

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

                if (!mainwindowviewmodel.IsScheduleDisabled)
                {
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


        }

        private void addSchedule_Click(object sender, RoutedEventArgs e)
        {
            AddSchedule dlg = new AddSchedule();

            dlg.Owner = this;

            dlg.ShowDialog();

        }

        private void LoadDataFile() //betölti a legfrissebb XML-t a DataFile public változóba
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


        private void LoadScheduleList() //betölti a select listába az elemeket a DataFile XML-ből
        {
            XmlNodeList nodeList;

            nodeList = DataFile.SelectNodes("/root/schedule");

            mostNeMukodjel = true; //ne triggerelődjön az eventje a lista változásnak mer nem Magdi basztatja hanem a program

            List<string> _schedulelist = new List<string>();

            foreach (XmlNode schedule in nodeList)
            {
                string schedulename = schedule["title"].InnerText;
                _schedulelist.Add(schedulename);

                Console.WriteLine("Added " + schedulename); //debug
            }
            mainwindowviewmodel.ScheduleList = _schedulelist; //belerakja a selectlistbe

            selectSchedule.SelectedIndex = mainwindowviewmodel.ScheduleList.IndexOf(GetDefaultSchedule());

            Console.WriteLine("Összes csengetés hozzáadva a listához. Összesen: " + selectSchedule.Items.Count);
            mostNeMukodjel = false;
        }

        private void ApplySelectedSchedule(string ScheduleName) //aktiválja a megadott nevű csengetést
        {
            XmlNode root = DataFile.DocumentElement;

            ObservableCollection<CsengetesiRend> _csengetes = new ObservableCollection<CsengetesiRend>();

            mainwindowviewmodel.jelzocsengetesek = new List<string>();
            mainwindowviewmodel.becsengetesek = new List<string>();
            mainwindowviewmodel.kicsengetesek = new List<string>();

            XmlNodeList lessons = root.SelectNodes("/root/schedule[./title[contains(text(), '" + ScheduleName + "')]]//lesson[@id]");
            foreach (XmlNode lesson in lessons)
            {
                _csengetes.Add(new CsengetesiRend() { jelzo = lesson["jelzo"].InnerText, becsengetes = lesson["becsengetes"].InnerText, kicsengetes = lesson["kicsengetes"].InnerText });

                mainwindowviewmodel.jelzocsengetesek.Add(lesson["jelzo"].InnerText);
                mainwindowviewmodel.becsengetesek.Add(lesson["becsengetes"].InnerText);
                mainwindowviewmodel.kicsengetesek.Add(lesson["kicsengetes"].InnerText);

            }

            mainwindowviewmodel.Csengetes = _csengetes;

            XmlNode hangok = root.SelectSingleNode("/root/schedule[./title[contains(text(), '" + ScheduleName + "')]]/sound");
            if (hangok != null)
            {
                jelzohang = hangok["jelzo"].InnerText;
                becsengeteshang = hangok["becsengetes"].InnerText;
                kicsengeteshang = hangok["kicsengetes"].InnerText;
            }

            Console.WriteLine(ScheduleName + " nevű csengetés aktiválva.");

        }

        private string GetDefaultSchedule() //lekéri az előzőleg Magdi által beállított csengetést
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

        private void SetDefaultSchedule(string scheduleName) //megjegyzi mit állított be Magdi csengetésnek (belerakja az XMLbe)
        {
            XmlNode root = DataFile.DocumentElement;
            XmlNode current = root.SelectSingleNode("/root/currentschedule");
            current.InnerText = scheduleName;

            DataFile.Save(filename); //Elmentjük a módosított XML-t. 

            MessageBox.Show("Mostantól a(z) " + scheduleName + " csengetési rend van érvényben.", "Sikeres módosítás", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        private void selectSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e) //ez az event mi a faszért fut le az elején?
        {

            if (selectSchedule.SelectedItem != null && !mostNeMukodjel)
            {
                SetDefaultSchedule(selectSchedule.SelectedItem.ToString()); //Fájlba menjük az új csengetést

                ApplySelectedSchedule(GetDefaultSchedule()); //Betöltjük a kiválasztott csengetést.
            }
        }


        private void OnScheduleAdded(object sender, EventArgs e)
        {
            LoadDataFile(); //Írtunk az XML-be szóval betöltjük újra - frissül a DataFile változó
            LoadScheduleList(); //Újra betöltjük a legördülő listát
            Console.WriteLine("OnScheduleAdded Event lefutott"); //persze hogy lefut geci de miért lesz üres az a kurva lista
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Biztosan ki akarod törölni?", "Megerősítés szükséges", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                XmlNode root = DataFile.DocumentElement;
                XmlNode schedule = root.SelectSingleNode("/root/schedule[./title[contains(text(), '" + GetDefaultSchedule() + "')]]");
                root.RemoveChild(schedule);
                DataFile.Save(filename);

                schedule = root.SelectSingleNode("/root/schedule[0]/title");


                if(schedule != null)
                {
                    SetDefaultSchedule(schedule.InnerText);
                    ApplySelectedSchedule(schedule.InnerText);
                }
                else
                {
                    MessageBox.Show("Nem található másik betölthető csengetési rend.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                LoadScheduleList();
                ApplySelectedSchedule(GetDefaultSchedule());
            }
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            mainwindowviewmodel.IsScheduleDisabled = !mainwindowviewmodel.IsScheduleDisabled;
        }
    }
}
