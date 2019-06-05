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
using Newtonsoft;
using Newtonsoft.Json;

namespace GUIproject
{
    /// <summary>
    /// Logika interakcji dla klasy Okno2.xaml
    /// </summary>
    public partial class Okno2 : Window
    {
        public Okno2()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Serialize(((MainWindow)Application.Current.MainWindow).FilePathBox.Text, ((MainWindow)Application.Current.MainWindow).lista, ((MainWindow)Application.Current.MainWindow).gazeta);
        }

        public void Serialize(string filename, List<Gazeta> lista, Gazeta gazeta)
        {
            RootObject obj = new RootObject();
            obj.IdentyfikatorArt = Int32.Parse(ArtIDBox.Text);
            obj.Data = DateTime.Parse(DateBox.Text);
            obj.Oceny = Double.Parse(RatingBox.Text);
            obj.Wyswietlenia = Int32.Parse(ViewsBox.Text);
            obj.Identyfikator = Int32.Parse(IDBox.Text);
            obj.imie = NameBox.Text;
            obj.nazwisko = SurnameBox.Text;

            gazeta.Convert(obj);
            lista.Add(new Gazeta { redaktorzy = gazeta.redaktorzy, artykuly = gazeta.artykuly });

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(filename))
                using(JsonWriter writer = new JsonTextWriter(sw))
            {
                var temp = new List<RootObject>();
                foreach (var item in lista)
                {
                    obj.Convert(item);
                    temp.Add(new RootObject { Data = obj.Data, Identyfikator = obj.Identyfikator, IdentyfikatorArt = obj.IdentyfikatorArt, imie = obj.imie, nazwisko = obj.nazwisko, Oceny = obj.Oceny, Wyswietlenia = obj.Wyswietlenia });
                }        
                serializer.Serialize(writer, temp);
            }
        }
    }
}
