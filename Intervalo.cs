using System;
using System.Collections.Generic;
using System.Linq;

//Classe utilizada para representar e manipular intervalos de números reais.
public class Intervalo
{
    private bool inf_fechado;
    private double inf;
    private double sup;
    private bool sup_fechado;
    private Intervalo posterior;//Utilizado quando se faz união entre intervalos



    public Intervalo(bool inf_fechado, double inf, double sup, bool sup_fechado, Intervalo uniaoPosterior = null)
    {
        this.inf_fechado = inf_fechado;
        this.inf = inf;
        this.sup = sup;
        this.sup_fechado = sup_fechado;
        this.posterior = uniaoPosterior;
    }
    public bool Contem(double x)//Retorna "true" quando x está contido no intervalo que chamou esse método
    {
        if (this.posterior != null)
            if (this.posterior.Contem(x))//Checa se a união posterior contem x
                return true;
        //Checa se o atual contem x
        if (this.inf_fechado && x == inf)
            return true;
        else if (this.sup_fechado && x == sup)
            return true;
        else if (x > this.inf && x < this.sup)
            return true;
        else
            return false;

    }
    private (double, int) Soma() //Realiza a soma de todos inteiros contidos no intervalo que chamou esse método, 
                                 //retornando essa soma e a quantidade de inteiros que  foram usados na soma na forma de tupla (soma,qtd)
                                 //private  pois é utrilizada apenas para realizar a média
    {
        double ini, fim, soma = 0;
        int qtd = 0;
        if (this.inf_fechado == true)
            ini = (double)this.inf;
        else
            ini = (double)this.inf + 1;
        if ((int)ini < ini)//Caso seja um real maior q seu inteiro, mudo inicio para o proximo inteiro
            ini = (int)ini + 1;
        if (this.sup_fechado == true)
            fim = (double)this.sup;
        else
            fim = (double)this.sup - 1;
        if ((int)ini < ini)//Caso seja um real maior q seu inteiro, mudo inicio para o inteiro
            fim = (int)fim;
        for (int i = (int)ini; i <= fim; i++)
        {
            soma += i;
            qtd++;
        }

        if (this.posterior != null)
        {
            var resp = this.posterior.Soma();
            soma += resp.Item1;
            qtd += resp.Item2;
        }
        return (soma, qtd);
    }
    public double Media()//Realiza a média entre os valores inteiros contidos dentro do intervalo que chamou esse método
    {
        var resp = this.Soma();
        return resp.Item1 / resp.Item2;
    }

    public Intervalo Produto(Intervalo b)  // Realiza o produto entre o intevalo que chamou esse método e o intervalo b passado como parâmetro
                                           // utilizando a definição do produto dado no .pdf fornecido pelo professor:
                                           // [min (infa*infb, infa*supb, supa*infb, supa*supb), max (infa*infb, infa*supb,supa* infb, supa*supb)]
    {
        Intervalo aux = this.posterior;
        double thisSup = this.sup;
        while (aux != null)
        {
            thisSup = aux.sup;
            aux = aux.posterior;
        }
        aux = b.posterior;
        double bSup = b.sup;
        while (aux != null)
        {
            bSup = aux.sup;
            aux = aux.posterior;
        }

        var lista = new List<double> { this.inf * b.inf, this.inf * bSup, thisSup * b.inf, thisSup * bSup };
        double inf = lista.Min();
        double sup = lista.Max();
        Intervalo p = new Intervalo(true, inf, sup, true);
        return p;

    }

    public bool Intercepta(Intervalo b) //Retorna True quando o intervalo que chamou esse método intercepta o intervalo parâmetro b, ou vice-versa
    {
        bool resp = false;
        Intervalo aux = this;
        while (aux != null)
        {
            if ((aux.inf > b.inf && aux.inf < b.sup) || (aux.sup > b.inf && aux.sup < b.sup))
                return true;
            if (aux.inf_fechado)
            {
                if (b.inf_fechado)
                    if (b.inf == aux.inf)
                        return true;
                if (b.sup_fechado)
                    if (b.sup == aux.inf)
                        return true;
            }
            if (aux.sup_fechado)
            {
                if (b.sup_fechado)
                    if (b.sup == aux.sup)
                        return true;
                if (b.inf_fechado)
                    if (b.inf == aux.sup)
                        return true;
            }
            aux = aux.posterior;
        }
        if (b.posterior != null)
            resp = this.Intercepta(b.posterior);
        return resp;
    }
    public Intervalo Uniao(Intervalo b)
    {
        //OBS.: intervalos parciais são os intervalos que formam uma união
        //Ex.: no intervalo união [1,2] U [3,4] U ]4,6], [1,2] é um intervalo parcial
        var limites = new List<(double, bool)>();//Lista contendo todos limites superiors e inferiores e seus respectivos estados de fechado
        var intervalos = new LinkedList<Intervalo>();//Lista contendo os intervalos parciais que estão sendo criado para a uniao
        Intervalo uniao = null;
        Intervalo auxb = b;
        Intervalo auxthis = this;
        (double, bool) tupla1, tupla2;
        while (auxb != null)//percorre todos intervalos parciais de b
        {
            //Adiciona limites superior e inferior e seus respectivos estados de fechado , na forma de tupla, na lista
            limites.Add((auxb.inf, auxb.inf_fechado));
            limites.Add((auxb.sup, auxb.sup_fechado));
            auxb = auxb.posterior;
        }
        while (auxthis != null)//percorre todos intervalos parciais de this(intervalo que chamou o método união)
        {
            //Adiciona limites superior e inferior e seus respectivos estados de fechado , na forma de tupla, na lista
            limites.Add((auxthis.inf, auxthis.inf_fechado));
            limites.Add((auxthis.sup, auxthis.sup_fechado));
            auxthis = auxthis.posterior;
        }
        limites = limites.OrderBy(t => t.Item1).ToList();//Ordena a lista limites pelo valores double
        //limites = limites.Distinct().ToList();//remove tuplas identicas
        for (int i = 0; i < limites.Count - 1; i++)
        {
            //dado duas tuplas que possuem o mesmo double, mas diferem se são fechado ou não, remove a que tem o valor de fechado igual a false
            //pois ao unir intervalos cujos limites apenas diferem por um estar aberto e o outro estar fechado, manté-sem o intervalo fechado
            if ((limites.ElementAt(i).Item1 == limites.ElementAt(i + 1).Item1))
            {
                if (limites.ElementAt(i).Item2 != limites.ElementAt(i + 1).Item2)
                    limites.Remove((limites.ElementAt(i).Item1, false));
                else if(limites.ElementAt(i).Item2==true)
                {
                    limites.Remove((limites.ElementAt(i).Item1, true));
                }
            }
        }

        for (int i = 0; i < limites.Count - 1; i += 2)//Percorre a lista de limites, criando intervalos parciais e adicionando na  lista de intervalos
        {
            if ((limites.Count % 2 != 0) && (i == limites.Count - 3))//Se a qtd de limites for impar e tiver faltando apenas 3 para chegar ao fim da lista 
            {
                tupla1 = limites.ElementAt(i);//Pega o menor limite
                tupla2 = limites.ElementAt(i + 2);//Pega o maior limite 
                uniao = new Intervalo(tupla1.Item2, tupla1.Item1, tupla2.Item1, tupla2.Item2);
                intervalos.AddLast(uniao);
            }
            else
            {
                tupla1 = limites.ElementAt(i);//Pega o menor limite
                tupla2 = limites.ElementAt(i + 1);//Pega o segundo menor limite 
                uniao = new Intervalo(tupla1.Item2, tupla1.Item1, tupla2.Item1, tupla2.Item2);
                intervalos.AddLast(uniao);
            }
        }

        for (int i = intervalos.Count - 1; i > 0; i--)//Linka os intervalos parcias entre si, através do atributo posterior
        {
            intervalos.ElementAt(i - 1).posterior = intervalos.ElementAt(i);
        }

        return intervalos.ElementAt(0);
    }

    private void auxImprime()
    {
        if (this.inf_fechado && this.sup_fechado)
            Console.Write("[{0}..{1}]", this.inf, this.sup);
        if (!this.inf_fechado && this.sup_fechado)
            Console.Write("]{0}..{1}]", this.inf, this.sup);
        else if (this.inf_fechado && !this.sup_fechado)
            Console.Write("[{0}..{1}[", this.inf, this.sup);
        else if (!this.inf_fechado && !this.sup_fechado)
            Console.Write("]{0}..{1}[", this.inf, this.sup);
    }
    public void Imprime()//Imprime o intervalo que chamou esse método
    {
        Intervalo aux = this;
        aux.auxImprime();
        aux = aux.posterior;
        while (aux != null)
        {
            Console.Write(" U ");
            aux.auxImprime();
            aux = aux.posterior;
        }
        Console.WriteLine();
    }



}
