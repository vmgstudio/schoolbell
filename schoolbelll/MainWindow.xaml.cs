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
        private XmlDocument doc = new XmlDocument();
        private string filename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "schedule.xml");

        private List<string> jelzocsengetesek;
        private List<string> becsengetesek;
        private List<string> kicsengetesek;

        private string jelzohang;
        private string becsengeteshang;
        private string kicsengeteshang;

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

            loadSchedule();
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            LiveTimeLabel.Content = DateTime.Now.ToString("HH:mm:ss");
            
            //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));

            if(DateTime.Now.TimeOfDay.Minutes > minutes)
            {
                minutes = DateTime.Now.TimeOfDay.Minutes;

                if (jelzocsengetesek.Contains(DateTime.Now.ToString("HH:mm")))
                {
                    mediaPlayer.Open(new Uri(jelzohang));
                    mediaPlayer.Play();
                }

                if (becsengetesek.Contains(DateTime.Now.ToString("HH:mm")))
                {
                    mediaPlayer.Open(new Uri(becsengeteshang));
                    mediaPlayer.Play();
                }

                if (kicsengetesek.Contains(DateTime.Now.ToString("HH:mm")))
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

        private void loadSchedule()
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(filename);

                XmlNodeList nodeList;
                XmlNode root = document.DocumentElement;

                nodeList = document.SelectNodes("/root/schedule");
                
                mainwindowviewmodel.ScheduleList = new List<string>();

                foreach (XmlNode schedule in nodeList)
                {
                    string schedulename = schedule["title"].InnerText;

                    mainwindowviewmodel.ScheduleList.Add(schedulename);

                    Console.WriteLine("Added " + schedulename); //debug


                }


                //-----------------előzőleg beállított csengetés

                string currentschedule = "";

                try
                {
                    currentschedule = root.SelectSingleNode("/root/currentschedule").InnerText;

                    selectSchedule.SelectedIndex = selectSchedule.Items.IndexOf(currentschedule); //ez egy szar lófaszt nem csinál
                    
                }
                catch (Exception)
                {
                    root.AppendChild(document.CreateElement("currentschedule"));
                }


                //----------------csengetési idők és hangok

                jelzocsengetesek = new List<string>();
                becsengetesek = new List<string>();
                kicsengetesek = new List<string>();

                XmlNodeList lessons = root.SelectNodes("/root/schedule[./title[contains(text(), '" + currentschedule + "')]]//lesson[@id]");
                foreach (XmlNode lesson in lessons)
                {
                    jelzocsengetesek.Add(lesson["jelzo"].InnerText);
                    becsengetesek.Add(lesson["becsengetes"].InnerText);
                    kicsengetesek.Add(lesson["kicsengetes"].InnerText);

                }

                XmlNode hangok = root.SelectSingleNode("/root/schedule[./title[contains(text(), '" + currentschedule + "')]]/sound");
                if (hangok != null)
                {
                    jelzohang = hangok["jelzo"].InnerText;
                    becsengeteshang = hangok["becsengetes"].InnerText;
                    kicsengeteshang = hangok["kicsengetes"].InnerText;
                }

                Console.WriteLine("Csengetés betöltve");

            }
            catch (System.IO.FileNotFoundException)
            {
                XmlDeclaration xmlDeclaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = document.DocumentElement;
                document.InsertBefore(xmlDeclaration, root);

                XmlElement body = document.CreateElement(string.Empty, "root", string.Empty);
                document.AppendChild(body);

                XmlElement currentschedule = document.CreateElement("currentschedule");
                body.AppendChild(currentschedule);

                document.Save(filename);

            }


        }


        private void selectSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);

            //string selectedschedule = "";
            if (selectSchedule.SelectedItem != null)
            {
                string selectedschedule = selectSchedule.SelectedItem.ToString();

                XmlNode root = document.DocumentElement;
                XmlNode current = root.SelectSingleNode("/root/currentschedule");
                current.InnerText = selectedschedule;



                document.Save(filename);

                //loadSchedule(); //itt megint be kéne tölteni de nem működik

                Console.WriteLine("Selected Item: " + selectedschedule); //debug


                MessageBox.Show("Mostantól a(z) " + selectedschedule + " csengetési rend van érvényben.", "Sikeres módosítás", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }


        private void OnScheduleAdded(object sender, EventArgs e)
        {
            loadSchedule(); //idk ez jó e xd
            Console.WriteLine("loading újra ");
        }
    }
}
