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


        public string Byte(BitArray bits)
        {
            
            if (bits.Count < 8)
            {
                throw new ArgumentException("bits");
            }
            string resultado = "";
            byte[] bytes = new byte[1000];
            for (var j = 0; j < bits.Count; j++)
            {
                    while ((j + 1) % 8 != 0)
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
                    resultado += "-";
                
            }
            // Si si asi ya solo le hacemos substring
            int contador = 0;
            string agregado = "";
            string[] cadenas = resultado.Split('-');
            if(cadenas[cadenas.Length-1].Length <8)
            {
                contador=8-cadenas[cadenas.Length - 1].Length;
                for (int i = 0; i < contador; i++)
                {
                    agregado += "0";
                }
                cadenas[cadenas.Length - 1] = agregado + cadenas[cadenas.Length - 1];
            }
            //Probemoslo
            return resultado;
        }
        public static ulong BitArrayToU64(BitArray ba)
        {
            var len = Math.Min(64, ba.Count);
            ulong n = 0;
            for (int i = 0; i < len; i++)
            {
                if (ba.Get(i))
                    n |= 1UL << i;
            }
            return n;
        }
        public static string ToDebugString<Key, Value>(Dictionary<char, double> dictionary)//SI SIRVE
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }
        public void EscrituraArchivo(string nombreArchivo, string ruta, BitArray bits, Dictionary<char, double> Diccionario, double F)
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
            var resultado = Convert.ToInt16(Byte(bits), 2);
            string esto = Byte(bits);
            string[] cadenas=esto.Split('-');
            //Cadenas es lo que luego van a escribir? 
            //CADENAS es el codigo de binario
            //for (var i = 0; i < cadenas.Length; i++)
            //{
            //    cadenas[i]=System.Text.Encoding.ASCII.GetString(cadenas[i]);
            //}
            string codigo = Byte(bits)+d;
            byte[] array = System.Text.Encoding.ASCII.GetBytes(codigo);
            ulong pruebucci = BitArrayToU64(bits);
            int sos=Convert.ToInt32(pruebucci);
            codigo = System.Text.ASCIIEncoding.GetEncoding(sos)+d;

            //var Adecimal = Convert.ToInt32(,2);
            if (!File.Exists(NuevaRutaA))
            {
                //BinaryWriter Writer = new BinaryWriter(File.OpenWrite(NuevaRutaA));
                //Writer.Write(Diccionario);
                //getfileName para nombrar el archivo comprimido
                //using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(NuevaRutaA))
                //{

                    //streamWriter.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(Diccionario));
                    File.WriteAllBytes(NuevaRutaA, array);
                //    streamWriter.Close();

                //} 
                
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
