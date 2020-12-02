using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntervalosTrabalhoLP
{
    class Program
    {
        static void Main(string[] args)//Simples utilizações da classe Intervalo com objetivo de demonstração de suas funcionalidades
        {
            Intervalo b = new Intervalo(false, -1, 1.3, true);
            Console.Write("b= ");
            b.Imprime();
            Intervalo i = new Intervalo(true,5, 7, false);
            Console.Write("i= ");
            i.Imprime();
            Intervalo p = b.Produto(i);
            Console.Write("Produto de b com i: ");
            p.Imprime();
            Intervalo u = b.Uniao(i);
            Console.Write("União de b com i: ");
            u.Imprime();
            Console.WriteLine("b intercepta i? " + b.Intercepta(i).ToString());
            Console.WriteLine("i intercepta b? " + i.Intercepta(b).ToString());
            Console.WriteLine("União intercepta b? " + u.Intercepta(b).ToString());
            Console.WriteLine("União intercepta i? " + u.Intercepta(i).ToString());
            Console.WriteLine("Media da i: "+i.Media().ToString());
            Console.WriteLine("Media da b: " + b.Media().ToString());
            Console.WriteLine("Media da Uniao: " + u.Media().ToString());
            Console.WriteLine("União contém 3? "+u.Contem(3));
            Console.WriteLine("União contém 5? "+u.Contem(5));
            Console.WriteLine("b contém -1? " + u.Contem(-1));
            Console.ReadLine();
        }
    }
}
