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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GUIproject
{ 
    public partial class MainWindow : Window
    {
        public string filename;
        public List<Gazeta> lista = new List<Gazeta>();
        public Gazeta gazeta = new Gazeta();
        public MainWindow()
        {
            InitializeComponent();            
        }

        protected void Browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog open = new Microsoft.Win32.OpenFileDialog();
            bool? res = open.ShowDialog();
            if (res == true)
            {
                filename = open.FileName;
                content.Items.Clear();
                FilePathBox.Text = open.FileName;
                Deserialize(open.FileName, lista, gazeta);
            }
        }

        protected void Add_Click(object sender, RoutedEventArgs e)
        {
            Okno2 okno2 = new Okno2();
            okno2.Show();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            content.Items.Clear();
            MostArticles(lista);
        }
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            content.Items.Clear();
            SortByRating(lista);
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            content.Items.Clear();
            SortByDate(lista);
        }
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            content.Items.Clear();
            HighestRating(lista);
        }
        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            content.Items.Clear();
            HighestCombinedViews(lista);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            content.Items.Clear();
            if(File.Exists(FilePathBox.Text))
            {
                Deserialize(FilePathBox.Text, lista, gazeta);
            }
            else MessageBox.Show("File Not found!");
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            content.Items.Clear();
            lista.Clear();
        }

        public void Deserialize(string filename, List<Gazeta> lista, Gazeta gazeta) //"Unpack" the JSON items to a List of objects and display them
        {
            using (StreamReader r = new StreamReader(filename))
            {
                string json = r.ReadToEnd();
                var gazety = JsonConvert.DeserializeObject<List<RootObject>>(json);

                int i = 0;
                foreach (var item in gazety)
                {
                    gazeta.Convert(item);

                    lista.Add(new Gazeta { redaktorzy = gazeta.redaktorzy, artykuly = gazeta.artykuly });

                    content.Items.Add("\nid artykulu: " +  lista[i].artykuly.id + "\ndata: " + lista[i].artykuly.data.ToString("dd/MM/yyyy") + "\noceny: " +  lista[i].artykuly.stats.oceny+ "\nwyswietlenia: " + lista[i].artykuly.stats.wyswietlenia + "\nid: " + lista[i].redaktorzy.id + "\nimie: " + lista[i].redaktorzy.dane.imie + "\nnazwisko: " + lista[i].redaktorzy.dane.nazwisko);
                    i++;
                }
            }
        }
        public void MostArticles(List<Gazeta> lista) //Which author wrote the most articles?
        {
            var temp = new Dictionary<string, int>();

            foreach (var item in lista)
            {
                if (!temp.ContainsKey(item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko))
                {
                    temp.Add(item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko, 1);
                }
                else
                {
                    temp[item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko] += 1;
                }
            }

            var sorted = temp.OrderByDescending(item => item.Value);

            var max = new KeyValuePair<string, int>();

            foreach (var item in temp)
            {
                if (item.Value > max.Value)
                    max = item;
            }

            content.Items.Add("\nNajwiecej artykulow napisal: " + max.Key + ", az " + max.Value);

            //foreach (var item in sorted)
            //{
            //    Console.WriteLine("Autor " + item.Key + " napisal " + item.Value + " artykulow");
            //}

            temp.Clear();
        }
        public void SortByRating(List<Gazeta> lista) //Dispay articles sorted by rating
        {
            var temp = new Dictionary<int, double>();

            foreach (var item in lista)
            {
                temp.Add(item.artykuly.id, item.artykuly.stats.oceny);
            }

            var sorted = temp.OrderByDescending(item => item.Value);

            //var sortedWithId = new Dictionary<int, Dictionary<string, double>>();

            Console.WriteLine();
            foreach (var item in sorted)
            {
                content.Items.Add("Artykul o id: " + item.Key + ", ocena: " + item.Value);
            }

            temp.Clear();
        }
        public void SortByDate(List<Gazeta> lista) //Display articles sorted by views
        {
            var temp = new Dictionary<int, DateTime>();

            foreach (var item in lista)
            {
                temp.Add(item.artykuly.id, item.artykuly.data);
            }

            var sorted = temp.OrderByDescending(item => item.Value);

            Console.WriteLine();
            foreach (var item in sorted)
            {
                content.Items.Add("Artykul o id: " + item.Key + ", data: " + item.Value.ToString("dd/MM/yyyy"));
            }

            temp.Clear();
        }
        public void HighestRating(List<Gazeta> lista) //Whose articles got the highest average rating?
        {
            var cache = new Dictionary<string, double>();
            var occ = new Dictionary<string, int>();

            foreach (var item in lista)
            {
                if (!cache.ContainsKey(item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko))
                {
                    cache.Add(item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko, item.artykuly.stats.oceny);
                    occ.Add(item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko, 1);
                }
                else
                {
                    cache[item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko] += item.artykuly.stats.oceny;
                    occ[item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko] += 1;
                }

            }

            foreach (var item in lista)
            {
                if (cache[item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko] > 5)
                {
                    cache[item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko] /= occ[item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko];
                }
            }

            var max = new KeyValuePair<string, double>();

            foreach (var item in cache)
            {
                if (item.Value > max.Value)
                    max = item;
            }

            content.Items.Add("\nAutor " + max.Key + " uzyskal srednia ocen: " + max.Value.ToString("0.###"));

            //foreach (var item in cache)
            //{
            //    Console.WriteLine("Autor " + item.Key + " uzyskal srednia ocen: " + item.Value);
            //}

            cache.Clear();
        }
        public void HighestCombinedViews(List<Gazeta> lista) //Which author got the highest combined views number?
        {
            var cache = new Dictionary<string, int>();

            foreach (var item in lista)
            {
                if (!cache.ContainsKey(item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko))
                {
                    cache.Add(item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko, item.artykuly.stats.wyswietlenia);
                }
                else
                {
                    cache[item.redaktorzy.dane.imie + " " + item.redaktorzy.dane.nazwisko] += item.artykuly.stats.wyswietlenia;
                }
            }

            Console.WriteLine();

            foreach (var item in cache)
            {
                content.Items.Add("Artykuly autora " + item.Key + " uzyskaly lacznie " + item.Value + " wyswietlen.");
            }

            cache.Clear();
        }       
    }
}
