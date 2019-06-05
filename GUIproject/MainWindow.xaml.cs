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
        public MainWindow()
        {
            InitializeComponent();
        }

        protected void Button_Click(object sender, RoutedEventArgs e)
        {
            //No
        }
        public static void Deserialize(string filename, List<Gazeta> lista, Gazeta gazeta) //"Unpack" the JSON items to a List of objects and display them
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

                    Console.WriteLine("\nid artykulu: {0} \ndata: {1} \noceny: {2} \nwyswietlenia: {3} \nid: {4} \nimie: {5} \nnazwisko: {6}", lista[i].artykuly.id, lista[i].artykuly.data.ToString("dd/MM/yyyy"), lista[i].artykuly.stats.oceny, lista[i].artykuly.stats.wyswietlenia, lista[i].redaktorzy.id, lista[i].redaktorzy.dane.imie, lista[i].redaktorzy.dane.nazwisko);
                    i++;
                }
            }
        }
        public static void MostArticles(List<Gazeta> lista) //Which author wrote the most articles?
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

            Console.WriteLine("\nNajwiecej artykulow napisal : {0}, az {1}", max.Key, max.Value);

            //foreach (var item in sorted)
            //{
            //    Console.WriteLine("Autor " + item.Key + " napisal " + item.Value + " artykulow");
            //}
        }
        public static void SortByRating(List<Gazeta> lista) //Dispay articles sorted by rating
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
                Console.WriteLine("Artykul o id: " + item.Key + ", ocena: " + item.Value);
            }
        }
        public static void SortByDate(List<Gazeta> lista) //Display articles sorted by views
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
                Console.WriteLine("Artykul o id: " + item.Key + ", data: " + item.Value.ToString("dd/MM/yyyy"));
            }
        }
        public static void HighestRating(List<Gazeta> lista) //Whose articles got the highest average rating?
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

            Console.WriteLine("\nAutor " + max.Key + " uzyskal srednia ocen: " + max.Value.ToString("0.###"));

            //foreach (var item in cache)
            //{
            //    Console.WriteLine("Autor " + item.Key + " uzyskal srednia ocen: " + item.Value);
            //}
        }
        public static void HighestCombinedViews(List<Gazeta> lista) //Which author got the highest combined views number?
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
                Console.WriteLine("Artykuly autora " + item.Key + " uzyskaly lacznie " + item.Value + " wyswietlen.");
            }
        }
    }
}
