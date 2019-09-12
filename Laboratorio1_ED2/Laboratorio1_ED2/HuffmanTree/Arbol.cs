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

        //

        public static byte[] BitsaBytes(BitArray bits)
        {
            byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
            bits.CopyTo(ret, 0);
            return ret;
        }

        public void EscrituraArchivo(string nombreArchivo, string ruta, BitArray bits, Dictionary<char,double> Diccionario, double F)
        {
            string NuevaRutaA = "";
            string RutaArchivos = ruta + @"\" + "Comprimidos";
            string[] Direccion2 = RutaArchivos.Split('\\');
            string[] Nombresucci = nombreArchivo.Split('.');

            for (var i = 0; i < Direccion2.Length; i++)
            {
                NuevaRutaA += Direccion2[i] + "/";
            }
           byte[] Bytes = BitsaBytes(bits);
            string texto = BitConverter.ToString(BitsaBytes(bits));
            //;
            //for (var i = 0; i < bits.Length; i++)
            //{
            //    texto+=Bytes[i];
            //}


            NuevaRutaA += Nombresucci[0].ToUpper() + ".huff";
            //var Adecimal = Convert.ToInt32(,2);
            if (!File.Exists(NuevaRutaA))
            {
                //getfileName para nombrar el archivo comprimido
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(NuevaRutaA))
                {
                    streamWriter.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Diccionario));
                    streamWriter.WriteLine(texto);
                    streamWriter.Close();
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
