using System.Linq;
using System.Collections.Generic;
using System;

//Classe utilizada para representar e manipular intervalos de números reais.
public  class Intervalo
{
	private bool inf_fechado;
	private double inf;
	private double sup;
	private bool sup_fechado;
    private  Intervalo posterior;//Utilizado quando se faz união entre intervalos
	
	public Intervalo(bool inf_fechado, double inf, double sup, bool sup_fechado, Intervalo uniaoPosterior=null)
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
	private (double ,int)Soma() //Realiza a soma de todos inteiros contidos no intervalo que chamou esse método, 
								//retornando essa soma e a quantidade de inteiros que  foram usados na soma na forma de tupla (soma,qtd)
								//private  pois é utrilizada apenas para realizar a média
	{
		double ini, fim, soma = 0;
		int qtd = 0;
		if (this.inf_fechado == true)
			ini = (double)this.inf;
		else
			ini = (double)this.inf+1;
		if ((int)ini < ini)//Caso seja um real maior q seu inteiro, mudo inicio para o proximo inteiro
			ini = (int)ini + 1;
		if (this.sup_fechado == true)
			fim = (double)this.sup;
		else 
			fim = (double)this.sup - 1;
		if ((int)ini < ini)//Caso seja um real maior q seu inteiro, mudo inicio para o inteiro
			fim = (int)fim;
		for(int i =(int)ini;i<=fim;i++)
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
		return (soma,qtd);
	}
	public double Media()//Realiza a média entre os valores inteiros contidos dentro do intervalo que chamou esse método
    {
		var resp = this.Soma();
		return resp.Item1 / resp.Item2;
    }

	public Intervalo Produto (Intervalo b)  // Realiza o produto entre o intevalo que chamou esse método e o intervalo b passado como parâmetro
											// utilizando a definição do produto dado no .pdf fornecido pelo professor:
											// [min (infa*infb, infa*supb, supa*infb, supa*supb), max (infa*infb, infa*supb,supa* infb, supa*supb)]
    {
		Intervalo aux = this.posterior;
		double thisSup = this.sup;
		while (aux!=null)
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
		Intervalo p = new Intervalo(true,inf,sup,true);
		return p;

	}
	
	public bool Intercepta(Intervalo b) //Retorna True quando o intervalo que chamou esse método intercepta o intervalo parâmetro b, ou vice-versa
    {
		bool resp= false;
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
	public Intervalo Uniao(Intervalo b)//Retorna um novo intervalo que represent a união entre o intervalo que chamo esse método e o intervalo parâmetro b
	{
		Intervalo uniao=null;
		if(this.Intercepta(b)==false)//Se não se interceptam
        {
			if(b.sup <= this.inf)//b vem antes
            {
				if(b.sup==this.inf)
					uniao = new Intervalo(b.inf_fechado,b.inf,this.sup,this.sup_fechado);
				else
					uniao = new Intervalo(b.inf_fechado, b.inf, b.sup, b.sup_fechado,this);
			}
			else if (this.sup < b.inf)//b vem dps
            {
				if (b.inf == this.sup)
					uniao = new Intervalo(this.inf_fechado, this.inf, b.sup, b.sup_fechado);
				else
					uniao = new Intervalo(this.inf_fechado, this.inf, this.sup, this.sup_fechado, b);
			}
			
        }
		else//Se interceptam-se:
        {
			double infu, supu;
			bool fechadoInf,fechadoSup;
			if (this.inf >= b.inf)
			{
				if (this.inf == b.inf)
					fechadoInf = (this.inf_fechado || b.inf_fechado);
				infu = b.inf;
				fechadoInf = b.inf_fechado;
			}
			else
            {
				infu = this.inf;
				fechadoInf = this.inf_fechado;

			}
				
			if (this.sup >= b.sup)
			{
				if (this.sup == b.sup)
					fechadoSup = (this.sup_fechado || b.sup_fechado);
				supu = this.sup;
				fechadoSup = this.sup_fechado;

			}
			else
            {
				supu = b.sup;
				fechadoSup = b.sup_fechado;
			}
				
			uniao = new Intervalo(fechadoInf, infu, supu, fechadoSup);

		}
		return uniao;
	}
	public void Imprime()//Imprime o intervalo que chamou esse método
    {
		if(this.inf_fechado && this.sup_fechado)
			Console.Write("[{0}..{1}]",this.inf,this.sup);
		if (!this.inf_fechado && this.sup_fechado)
			Console.Write("]{0}..{1}]", this.inf, this.sup);
		else if (this.inf_fechado && !this.sup_fechado)
			Console.Write("[{0}..{1}[", this.inf, this.sup);
		else if (!this.inf_fechado && !this.sup_fechado)
			Console.Write("]{0}..{1}[", this.inf, this.sup);
		if (this.posterior != null)
		{
			Console.Write(" U ");
			this.posterior.Imprime();
		}
		else
			Console.WriteLine();

	}
	
	

}
