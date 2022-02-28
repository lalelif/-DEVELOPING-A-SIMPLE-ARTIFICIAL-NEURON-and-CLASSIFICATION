using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static Random random = new Random();  //random sayı üretimi için
        static void Main(string[] args)
        {
            double[,] noktaDizisi = rastgeleNoktaUretimi(20, 100, 100); //metoda değer verip çalıstırılır
            double[,] noktaDizisiCopy= new double[20, 2];
            //noktaDizisi.CopyTo(noktaDizisiCopy,0);
            for (int i = 0; i < noktaDizisi.GetLength(0); i++)
            { 

                for (int j = 0; j < noktaDizisi.GetLength(1); j++)
                {
                    noktaDizisiCopy[i,j]=noktaDizisi[i, j];
                }
            }
            double[,] noktalar = new double[10,2]; //knn için 10 farklı noktayı tut
            
            
            
                Console.WriteLine("Noktalar n=20 için :\n");                       //Noktaları yazdırıyoruz
            for (int i = 0; i < noktaDizisi.GetLength(0); i++)
            {
                Console.WriteLine("Nokta" + i + ": " + "x" + i + "\t" + "y" + i + "\t ");

                for (int j = 0; j < noktaDizisi.GetLength(1); j++)
                {
                    Console.Write(String.Format("\t{0:0.0}", noktaDizisi[i, j]));
                }
                Console.WriteLine("\n");
            }

            double[,] noktaDizisiTest = rastgeleNoktaUretimi(50, 100, 100);

            Console.WriteLine("Noktalar n=50 için:\n");                       //Noktaları yazdırıyoruz
            for (int i = 0; i < noktaDizisiTest.GetLength(0); i++)
            {
                Console.WriteLine("Nokta" + i + ": " + "x" + i + "\t" + "y" + i + "\t ");

                for (int j = 0; j < noktaDizisiTest.GetLength(1); j++)
                {
                    Console.Write(String.Format("\t{0:0.0}", noktaDizisiTest[i, j]));
                }
                Console.WriteLine("\n");
            }

            Console.WriteLine("\n\t\tDistance Matrix\n");           //Distance Matrix i yazdırıyoruz

            double[,] DM = distanceMatrixUretimi(noktaDizisi);  //metot calıstırılır
            Console.Write("   ");
            for (int j = 0; j < noktaDizisi.GetLength(0); j++)
            {
                Console.Write(String.Format("\t{0}", j));
            }
            Console.Write("\n\n");
            for (int i = 0; i < noktaDizisi.GetLength(0); i++)
            {
                Console.Write(" " + i + " \t");
                for (int j = 0; j < noktaDizisi.GetLength(0); j++)
                {
                    Console.Write(String.Format("{0:0.0}", DM[i, j]) + "\t");
                }
                Console.WriteLine("\n");
            }
            Console.WriteLine("\n\t\tNearest Neighbour\n");
            
            for (int i = 0; i < noktalar.GetLength(0); i++) //rastgele 10 noktayı noktalar arrayinin içine at
            {
                int random_nokta_row = random.Next(0, noktaDizisiCopy.GetLength(0));
                int random_nokta_column = random.Next(0, noktaDizisiCopy.GetLength(1));
                TrimArray(random_nokta_row, random_nokta_column, noktaDizisiCopy);
                for (int j = 0; j < noktalar.GetLength(1); j++)
                {
                    noktalar[i, j] = noktaDizisi[random_nokta_row, random_nokta_column];
                }
            }
            Karsilastir[][] info = KNNclassify(noktalar, noktaDizisi);         
            double top_yol = 0;
            for (int i = 0; i < noktalar.GetLength(0); i++)
            {

                Console.WriteLine("Tur numarası:{0}", i + 1);   //nearest neighbour metoduna göre noktaların ekrana yazılması
                Console.WriteLine("İlgili turda sırayla uğradığı noktalar:");
                for (int j = 0; j < noktaDizisi.GetLength(0); j++)
                { 
                    Console.Write("{0},", info[i][j].idx);
                    top_yol += info[i][j].uzaklik;
                }
                Console.WriteLine();    
                Console.WriteLine("Turun toplam yol uzunluğu:{0}", top_yol);
            }
            Console.ReadLine();
        }
        public static double[,] TrimArray(int rowToRemove, int columnToRemove, double[,] originalArray)   //rastgele seçilen noktayı bir daha seçmemek için arrayi düzenledim
        {
            double[,] result = new double[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

            for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
            {
                if (i == rowToRemove)
                    continue;

                for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
                {
                    if (k == columnToRemove)
                        continue;

                    result[j, u] = originalArray[i, k];
                    u++;
                }
                j++;
            }

            return result;
        }

        static double[,] rastgeleNoktaUretimi(int n, double width, double height)  //rastgele nokta üretimi için metot
        {
            double[,] noktalar = new double[n, 2];
            for (int i = 0; i < n; i++)
            {
                double x = random.NextDouble() * width;
                double y = random.NextDouble() * height;
                noktalar[i, 0] = x;
                noktalar[i, 1] = y;
            }

            return noktalar;

        }

        public static double Distance(double x1, double y1, double x2, double y2)  //2 nokta arasındaki uzaklık hesaplayan metot
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        static double[,] distanceMatrixUretimi(double[,] noktalar)  //DM üretimi için metot, parametresi nx2 lik noktalar matrisi
        {
            double[,] distanceMatrix = new double[noktalar.Length, noktalar.Length];

            for (int i = 0; i < noktalar.GetLength(0); i++)
            {
                for (int j = 0; j < noktalar.GetLength(0); j++)     //noktalar arası distance hesabı ve değerleri matrikse atma
                {
                    distanceMatrix[i, j] = Distance(noktalar[i, 0], noktalar[i, 1], noktalar[j, 0], noktalar[j, 1]);

                }

            }
            return distanceMatrix;      //nxn lik uzaklık matrisine döndürür

        }
        static Karsilastir[][] KNNclassify(double[,] noktalar, double[,] veriler)  //KNN siniflandirma metodu
        {                                                                           //İcerisinde altta yazdigimiz yardımcı metotlar kullanilir
            int n = noktalar.GetLength(0);
            int m = veriler.GetLength(0);
            
            Karsilastir[][] info2 = new Karsilastir[n][];
            double[][] distance = UzaklikHesapla(noktalar, veriler);
            for (int i = 0; i < n; ++i)
            {
                Karsilastir[] info = new Karsilastir[m];
                for (int j = 0; j < m; ++j)
                {
                    Karsilastir cmp = new Karsilastir(j, distance[i][j]);
                    info[j] = cmp;
                }
                Array.Sort(info); // uzaklıklara göre noktaların sıralanması
                info2[i] = info;
            }
            
            return info2;
        }
        public class Karsilastir : IComparable//<Karsilastir> //verisetindekiyle kullanıcıdan alınacak öğeye olan ilişkili mesafeyi tutar
        {
            public int idx;  // veri indeksi
            public double uzaklik;  // bilinmeyen noktaya uzaklık
            public Karsilastir(int idx, double uzaklik)
            {
                this.idx = idx;
                this.uzaklik = uzaklik;
            }
            public int CompareTo(object obj) //karşılaştırma yapar
            {
                if (obj == null) return 1;

                Karsilastir other = obj as Karsilastir;
                if (other != null)
                    return this.uzaklik.CompareTo(other.uzaklik);
                else
                    throw new ArgumentException("Object is not a distance");
            }
        }
        static double[][] UzaklikHesapla(double[,] noktalar, double[,] data)  //Distance hesabı icin metot
        {
            double[][] sum = new double[noktalar.GetLength(0)][]; //jagged array
          
                      
            for (int i = 0; i < noktalar.GetLength(0); ++i)
            {
                double[] dst = new double[data.GetLength(0)];
                for (int j = 0; j < data.GetLength(0); ++j)
                     dst[j] =Distance(noktalar[i, 0], noktalar[i, 1], data[j, 0], data[j, 1]);
                sum[i] = dst;

            }
            
            return sum;
        }
    }
}
