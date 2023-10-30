using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercle_Hellman
{
    internal class Program
    {
        static void Main()
        {
            int[] w = { 2, 7, 11, 21, 42, 89, 180, 354 }; //Закрытый ключ - последовательность весов для задачи укладки сверхвозрастающего ранца
            int q = 881; //Значение q должно быть больше суммы всех чисел последовательности
            int r = 588; //Значение r должно быть взаимно простым числом с модулем
            int[] beta = GetPublicKey(w, q, r);

            //Environment.GetEnvironmentVariables(); //GetEnvironmentVariable возвращает из текущего процесса значение переменной среды, которое будет передано для зашифровки
            //Console.Write("Введите текст который нужно зашифровать: " );
            string plainText = "hello, Mercle Hellman! :)";
            

            int[] encoded = Encrypt(plainText, beta);

            Console.Write("Tекст который нужно зашифровать: " + plainText);
            //int[] encoded = Encrypt(value, beta);
            Console.WriteLine("\nЗашифрованный текст: ");
            int count = 0;

            for (int i = 0; i < encoded.Length; i++)
            {
                Console.Write("{0, 5}", encoded[i]);
                count++;
                if (count == 16)
                {
                    count = 0;
                    Console.WriteLine();
                }
            }

            if (count > 0) Console.WriteLine();
            string decoded = Decrypt(encoded, w, q, r);
            Console.WriteLine("\nРасшифрованный текст: {0}", decoded);
            Console.Read();

        }

        static int[] GetPublicKey(int[] w, int q, int r) //Генерируем открытый ключ. Открытым ключом является последовательность весов для задачи укладки нормального ранца, имеющая то же самое решение.
        {
            int[] beta = new int[w.Length];
            for (int i = 0; i < w.Length; i++)
                beta[i] = w[i] * r % q; //Берем i-ый элемент из последовательности закрытого ключа *r mod q
            return beta;
        }

        static int[] Encrypt(string plainText, int[] beta)
        {
            if (String.IsNullOrEmpty(plainText)) return null;

            int[] encoded = new int[plainText.Length];

            for (int i = 0; i < encoded.Length; i++)
            {
                string bin = ConvertToBinary(plainText[i]); //переводим текст в двоичный вид
                int sum = 0;
                for (int j = 0; j < 8; j++)
                    sum += (bin[j] - 48) * beta[j]; //биты открытого текста умножаются на открытый ключ и суммируются, итоговая сумма представляет шифротекст (S = b1*M1 + b2*M2 +...+ bn*Mn)
                encoded[i] = sum;
            }
            return encoded;
        }

        static string Decrypt(int[] encoded, int[] w, int q, int r)
        {
            if (encoded == null || encoded.Length == 0) return null;
            char[] chars = new char[encoded.Length];
            int mir = ModInverse(r, q); //мультипликативное обратное r по модулю q (для нахождения используем расширенный алгоритм Евклида)
            if (mir == 0)
            {
                Console.WriteLine("Мультипликативное обратное не существует. Дешифрование прервано");
                return null;
            }
            /*
             находим самый «тяжелый» элемент из супервозрастающей последовательности;
             вычитаем его; 
             повторяем  шаги, пока процесс не закончится; 
             на местах вычитаемых элементов – единицы, остальные – нули.
            */
            for (int i = 0; i < encoded.Length; i++)
            {
                char[] bin = new char[8];
                for (int j = 0; j < 8; j++) bin[j] = '0';
                int temp = encoded[i] * mir % q;
                while (temp > 0)
                {
                    int index = 7;
                    for (int j = 1; j < w.Length; j++)
                    {
                        if (w[j] > temp)
                        {
                            index = j - 1; break;
                        }
                    }
                    bin[index] = '1';
                    temp -= w[index];
                }
                chars[i] = ConvertFromBinary(new string(bin));
            }
            return new string(chars);

        }

        static string ConvertToBinary(char c) //перевод текста в двоичный вид
        {
            return Convert.ToString((int)c, 2).PadLeft(8, '0');
        }

        static char ConvertFromBinary(string bin) //перевод из двоичного вида в текст
        {
            return (char)Convert.ToInt32(bin, 2);
        }

        static int ModInverse(int r, int q) //расширенный алгоритм Евклида
        {
            int i = q, v = 0, d = 1;
            while (r > 0)
            {
                int t = i / r, x = r;
                r = i % x; i = x; x = d; d = v - t * x; v = x;
            }
            v %= q;
            if (v < 0) v = (v + q) % q;
            return v;

        }
    }
}