using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Laboratorio1_ED2.Helpers;
using Newtonsoft.Json;

namespace Laboratorio1_ED2.HuffmanTree
{
    public class Arbol
    {
        private List<Nodo> ListaNodos = new List<Nodo>();
        public Nodo Raiz { get; set; }
        public Dictionary<char, double> DiccionarioFrecuencia = new Dictionary<char, double>();
        Nodo nodo = new Nodo();
        

        public BitArray armarArbol(string TextoCompleto) //arma el arbol con su tabla de probabilidades
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

            return(Cifrado(TextoCompleto));

    }
        public BitArray Cifrado (string textoCompleto)//cifra el texto de manera que se asignen los binario
        {
            List<bool> ConvertirTextoBinario = new List<bool>();
            for (var i = 0; i < textoCompleto.Length; i++)
            {
                List<bool> ConvertirCaracter = this.Raiz.Patron(textoCompleto[i], new List<bool>());
                ConvertirTextoBinario.AddRange(ConvertirCaracter);
            }
            BitArray bits = new BitArray(ConvertirTextoBinario.ToArray());
            return bits;
        }


        public byte[] Byte(BitArray bits)
        {
            
            if (bits.Count < 8)
            {
                throw new ArgumentException("bits");
            }
            string resultado = "";
            byte[] bytes = new byte[1000];
            for (int i = 0; i <= 7; i++)
            {
                if (bits[i])
                {
                    resultado += "1";
                }
                else
                {
                    resultado += "0";
                }
            }
            
            for (var j = 8; j < bits.Count; j++)
            {
               
                    if ((j) % 8 != 0)
                    {
                        if (bits[j])
                        {
                            resultado += "1";
                        }
                        else
                        {
                            resultado += "0";
                        }
                    }
                else 
                {
                    resultado += "-";
                    if (bits[j])
                    {
                        resultado += "1";
                    }
                    else
                    {
                        resultado += "0";
                    }
                }
            }
            int contador = 0;
            string agregado = "";
            string[] cadenas = resultado.Split('-');
            int total = cadenas[cadenas.Length - 1].Length;
            if(total<8)
            {
                contador=8-total;
                for (int i = 0; i < contador; i++)
                {
                    agregado += "0";
                }
                cadenas[cadenas.Length - 1] = agregado + cadenas[cadenas.Length - 1];
            }

            byte[] sos = new byte [cadenas.Length];
            for (int i = 0; i < cadenas.Length; i++)
            {
                sos[i] = Convert.ToByte(cadenas[i], 2);

            }

            return sos;
        }
        
        public static string ToDebugString<Key, Value>(Dictionary<char, double> dictionary)//SI SIRVE
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }


        public void EscrituraArchivo(string nombreArchivo, string ruta, BitArray bits, Dictionary<char, double> Diccionario)
        {
            string d =ToDebugString<char,double>(Diccionario);
            string NuevaRutaA = "";
            string RutaArchivos = ruta + @"\" + "Comprimidos";
            string[] Direccion2 = RutaArchivos.Split('\\');
            string[] Nombresucci = nombreArchivo.Split('.');

            for (var i = 0; i < Direccion2.Length; i++)
            {
                NuevaRutaA += Direccion2[i] + "/";
            }
            
            NuevaRutaA += Nombresucci[0].ToUpper() + ".huff";

             byte[] este=  Byte(bits);//Vector con grupos de 8
            
            if (!File.Exists(NuevaRutaA))
            {
                using (var writeStream1 = new FileStream(NuevaRutaA, FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(writeStream1))
                    {
                        foreach (var item in este)
                        {
                            writer.Write(item);
                        }
                        writer.Close();
                    }
                    writeStream1.Close();
                }
            }
        }

       


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
