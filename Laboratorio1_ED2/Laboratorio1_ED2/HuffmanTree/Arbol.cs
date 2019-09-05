using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Laboratorio1_ED2.Helpers;

namespace Laboratorio1_ED2.HuffmanTree
{
    public class Arbol
    {
        private List<Nodo> ListaNodos = new List<Nodo>();
        public Nodo Raiz { get; set; }
        public Dictionary<char, double> DiccionarioFrecuencia = new Dictionary<char, double>();
        Nodo nodo = new Nodo();
        

        public void armarArbol(string TextoCompleto) //arma el arbol con su tabla de probabilidades
        {
            for (var i = 0; i < TextoCompleto.Length; i++)
            {
                if (!DiccionarioFrecuencia.ContainsKey(TextoCompleto[i]))
                {
                    DiccionarioFrecuencia.Add(TextoCompleto[i],0);
                }
                DiccionarioFrecuencia[TextoCompleto[i]]++;
            }

        foreach (KeyValuePair<char,double> caracter in DiccionarioFrecuencia)
            {
                ListaNodos.Add(new Nodo() { Caracter = caracter.Key, Probabilidad = caracter.Value });
                while (ListaNodos.Count > 1)
                {
                    List<Nodo> ListaOrdenada = ListaNodos.OrderBy(Nodo => Nodo.Probabilidad).ToList<Nodo>();
                    if (ListaOrdenada.Count>=2)
                    {
                        //se toman los primerovalores de menor probabilidad
                        List<Nodo> menoresTomados = ListaOrdenada.Take(2).ToList<Nodo>();
                        Nodo Daddy = new Nodo()//creamos el nodo papá para generar las n
                        {
                            Caracter = '*',
                            Probabilidad = menoresTomados[0].Probabilidad + menoresTomados[1].Probabilidad,
                            Izquierda = menoresTomados[0],
                            Derecha = menoresTomados[1]
                        };
                        ListaNodos.Remove(menoresTomados[0]);
                        ListaNodos.Remove(menoresTomados[1]);
                        ListaNodos.Add(Daddy);
                   }
                    this.Raiz = ListaNodos.FirstOrDefault();
                }

            }

            Cifrado(TextoCompleto);

    }
        public BitArray Cifrado (string textoCompleto)//cifra el texto de manera que se asignen los binario
        {
            List<bool> ConvertirTextoBinario = new List<bool>();
            for (var i = 0; i < textoCompleto.Length; i++)
            {
                List<bool> ConvertirCaracter = this.Raiz.Traverse(textoCompleto[i], new List<bool>());
                ConvertirTextoBinario.AddRange(ConvertirCaracter);
            }
            BitArray bits = new BitArray(ConvertirTextoBinario.ToArray());          
            return bits;
        }

       // public void 

        //public static string Comprimido (string Binario)
        //{
        //    string Convertido = Convert.ToString(Binario);
        //    char[] ArregloBits = Convertido.ToArray();
        //    System.Text.Encoding encEncoder = System.Text.ASCIIEncoding.ASCII;

        //    System.Text.Encoding.Default.GetString(Binario);
        //    System.Text.ASCIIEncoding.GetBytes(ArregloBits);
        //    string TextoComprimido;

        //    TextoComprimido = encEncoder.ge(Binario);

        //    return TextoComprimido;
        //}

        public string Desifrado (BitArray bits)//deshace el codigo ya cifrado
        {
            Nodo actual = this.Raiz;
            string decodificado = "";
            foreach (bool bit in bits)
            {
                if (bit)
                {
                    if (actual.Derecha != null)
                    {
                        actual = actual.Derecha;
                    }
                }
                else
                {
                    if (actual.Izquierda != null)
                    {
                        actual = actual.Izquierda;
                    }
                }
                if (Hoja(actual))
                {
                    decodificado += actual.Caracter;
                    actual = this.Raiz;
                }
            }
            return decodificado;
        }
        public bool Hoja (Nodo NodoValidado)//chequea los nodos ...
        {
            return (NodoValidado.Izquierda == null && NodoValidado.Derecha == null);
        }
    }
}
