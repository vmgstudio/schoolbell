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
using System.Windows.Shapes;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Media;

namespace schoolbelll
{
    /// <summary>
    /// Interaction logic for AddSchedule.xaml
    /// </summary>
    public partial class AddSchedule : Window
    {

        string becsengohangfajl = "";
        string kicsengohangfajl = "";
        string jelzohangfajl = "";
        bool hiba = false;
        
        public AddSchedule()
        {
            InitializeComponent();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                String filename = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "schedule.xml");
                doc.Load(filename);


                string pattern = @"(?:2[0-3]|[01]?[0-9])[:.][0-5]?[0-9]";
                
                XmlElement schedule = doc.CreateElement("schedule");

                /*----------------Csengetés elnevezése---------------*/
                if (elnevezes.Text == "")
                {
                    MessageBox.Show("Adja meg a csengetés elnevezését.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    hiba = true;

                }

                XmlElement title = doc.CreateElement("title");
                title.InnerText = elnevezes.Text;
                schedule.AppendChild(title);

                /*---------------Csengetés hangok-------------------*/


                if (becsengohangfajl == "" || kicsengohangfajl == "" || jelzohangfajl == "")
                {
                    MessageBox.Show("Nincs minden hangfájl kiválasztva!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    hiba = true;
                }

                XmlElement sound = doc.CreateElement("sound");
                XmlElement jelzosound = doc.CreateElement("jelzo");
                jelzosound.InnerText = jelzohangfajl;
                XmlElement becsengosound = doc.CreateElement("becsengetes");
                becsengosound.InnerText = becsengohangfajl;
                XmlElement kicsengosound = doc.CreateElement("kicsengetes");
                kicsengosound.InnerText = kicsengohangfajl;

                sound.AppendChild(jelzosound);
                sound.AppendChild(becsengosound);
                sound.AppendChild(kicsengosound);

                schedule.AppendChild(sound);

                /*---------------Tanórák-------------------*/

                for (int i = 1; i< 9; i++) 
                {
                    TextBox jelzo = this.FindName("_" + i.ToString() + "_jelzo") as TextBox;
                    TextBox becsengo = this.FindName("_" + i.ToString() + "_becsengetes") as TextBox;
                    TextBox kicsengo = this.FindName("_" + i.ToString() + "_kicsengetes") as TextBox;
                    
                    if (!Regex.IsMatch(jelzo.Text, pattern) && jelzo.Text != "")
                    {
                        MessageBox.Show("Hibásan megadott adat: " + jelzo.Text, "Hibás formátum", MessageBoxButton.OK, MessageBoxImage.Warning);
                        hiba = true;                       
                        return;
                    }

                    if (!Regex.IsMatch(becsengo.Text, pattern) && becsengo.Text != "")
                    {
                        MessageBox.Show("Hibásan megadott adat: " + becsengo.Text, "Hibás formátum", MessageBoxButton.OK, MessageBoxImage.Warning);
                        hiba = true;  
                        return;
                    }

                    if (!Regex.IsMatch(kicsengo.Text, pattern) && kicsengo.Text != "")
                    {
                        MessageBox.Show("Hibásan megadott adat: " + kicsengo.Text, "Hibás formátum", MessageBoxButton.OK, MessageBoxImage.Warning);
                        hiba = true;  
                        return;
                    }

                    
                    XmlElement lesson = doc.CreateElement("lesson");
                    lesson.SetAttribute("id", i.ToString());
           
                    XmlElement xmljelzo = doc.CreateElement("jelzo");
                    XmlElement xmlbecsengetes = doc.CreateElement("becsengetes");
                    XmlElement xmlkicsengetes = doc.CreateElement("kicsengetes");

                    xmljelzo.InnerText = jelzo.Text;
                    xmlbecsengetes.InnerText = becsengo.Text;
                    xmlkicsengetes.InnerText =  kicsengo.Text;

                    lesson.AppendChild(xmljelzo);
                    lesson.AppendChild(xmlbecsengetes);
                    lesson.AppendChild(xmlkicsengetes);

                    schedule.AppendChild(lesson);

                }

                XmlElement root = doc.DocumentElement;
                root.AppendChild(schedule);

                if (!hiba)
                {
                    doc.Save(filename);
                    MessageBox.Show("Sikeresen rögzítette a(z) " + elnevezes.Text + " csöngetési rendet.", "Sikeres rögzítés", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppEventHandler.OnScheduleAddedEvent(this, null);
                    this.Close();
                }



            }
            catch (System.IO.FileNotFoundException)
            { 
                MessageBox.Show("Úgy űnik, a schedule.xml fájl meghibásodott. Próbálja meg újraindítani a programot.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);    
            }

            hiba = false;
        }

        private void openFileDialog(int id)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mp3";
            dlg.Filter = "Hangfájlok (.mp3, .wav)|*.mp3;*.wav";


            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                if (id == 1)
                {
                    becsengohangfajl = dlg.FileName;
                }
                if (id == 2)
                {
                    kicsengohangfajl = dlg.FileName;
                }
                if (id == 3)
                {
                    jelzohangfajl = dlg.FileName;

                }
            }

        }

        private void jelzosoundBtn_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog(3);
        }

        private void becsengosoundBtn_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog(1);
        }

        private void kicsengosoundBtn_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog(2);
        }

    }
}
