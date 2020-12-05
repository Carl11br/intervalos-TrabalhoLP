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
            Intervalo A = new Intervalo(true, 1, 4, true);
            Intervalo B = new Intervalo(true, 4, 6, false);
            Intervalo C = new Intervalo(false, 6, 10, false);
            Intervalo AuB = A.Uniao(B);
            Intervalo AuBuC = AuB.Uniao(C);
            Intervalo AxB = A.Produto(B);
            Console.Write("Intervalo A:"); A.Imprime();
            Console.Write("Intervalo B:"); B.Imprime();
            Console.Write("Intervalo C:"); C.Imprime();
            Console.Write("Intervalo A U B:"); AuB.Imprime();
            Console.Write("Intervalo (A U B) U C:"); AuBuC.Imprime();
            Console.Write("Intervalo A x B:"); AxB.Imprime();
            Console.WriteLine("Média do intervalo B: {0}", B.Media());
            Console.WriteLine("O intervalo A intecepeta B? {0} ", A.Intercepta(B));
            Console.ReadLine();


        }
    }
}
