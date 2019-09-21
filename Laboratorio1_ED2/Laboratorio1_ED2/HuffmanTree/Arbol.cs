using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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
        public Dictionary<char, List<bool>> DiccionarioAux = new Dictionary<char, List<bool>>();
        Nodo nodo = new Nodo();

        public byte[] Byte(BitArray bits)//Separacion de bits
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

            string[] cadenas = resultado.Split('-');
            int total = cadenas[cadenas.Length - 1].Length;
            
            byte[] sos = new byte[cadenas.Length];
            for (int i = 0; i < cadenas.Length; i++)
            {
                sos[i] = Convert.ToByte(cadenas[i], 2);
            }

            return sos;
        }

        public BitArray armarArbol(Dictionary<string,double> DiccionarioFrecuencia,string TextoCompleto) //arma el arbol con su tabla de probabilidades
        {
            
        foreach (KeyValuePair<string,double> caracter in DiccionarioFrecuencia)
            {
                ListaNodos.Add(new Nodo() { Caracter = caracter.Key.ToCharArray()[0], Probabilidad = caracter.Value });
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

        public void armarArbolComp(Dictionary<string, double> Diccionarioss)
        {
            foreach (KeyValuePair<string, double> caracter in Diccionarioss)
            {
                ListaNodos.Add(new Nodo() { Caracter = caracter.Key.ToCharArray()[0], Probabilidad = caracter.Value });
                while (ListaNodos.Count > 1)
                {
                    List<Nodo> ListaOrdenada = ListaNodos.OrderBy(Nodo => Nodo.Probabilidad).ToList<Nodo>();
                    if (ListaOrdenada.Count >= 2)
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

        public string ToDebugString<Key, Value>(Dictionary<string, double> dictionary)//Conversion diccionario a String para escritura
        {
            return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
        }

        public void EscrituraArchivo(string nombreArchivo, string ruta, BitArray bits, Dictionary<string, double> Diccionario)
        {
            FileInfo fi = new FileInfo(nombreArchivo);
            string d = "|"+ToDebugString<string,double>(Diccionario);
            string NuevaRutaA = "";
            string RutaArchivos = ruta;
            string[] Direccion2 = RutaArchivos.Split('\\');
            string[] Nombresucci = nombreArchivo.Split('.');
            var binFormatter = new BinaryFormatter();
            
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
                    StreamWriter WriteReportFile = File.AppendText(NuevaRutaA);
                    WriteReportFile.WriteLine(d);
                    WriteReportFile.Close();
                }

            }
            
            long length = new System.IO.FileInfo(NuevaRutaA).Length;
        }

        public Dictionary<string, double> ArmarDiccionario(string TextoCompleto)//Armar diccionario para arbol
        {
            Dictionary<string, double> DiccionarioA = new Dictionary<string, double>();
            for (var i = 0; i < TextoCompleto.Length; i++)
            {
                if (!DiccionarioA.ContainsKey(TextoCompleto[i].ToString()))
                {
                    DiccionarioA.Add(TextoCompleto[i].ToString(), 0);
                }
                DiccionarioA[TextoCompleto[i].ToString()]++;
            }
            return DiccionarioA;
        }

        public string Desifrado (string ruta)//deshace el codigo ya cifrado
        {
            const int bufferLength = 1000000000;
            var byteBuffer = new byte[bufferLength];
            
            using (var stream = new FileStream(ruta, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                        byteBuffer = reader.ReadBytes(bufferLength);
                    reader.Close();
                }
            }
            byte[] bytes = byteBuffer;
            int indice = 0;
            do
            {
                indice++;
            } while (bytes[indice] != 124);

            bytes=bytes.Where((item, index) => index < indice).ToArray();

            string texto = string.Empty;
            using (StreamReader streamReader = new StreamReader(ruta))
            {
                streamReader.BaseStream.Position = indice+1;
                texto = streamReader.ReadLine();
                streamReader.Close();
            }
            int final = texto.Length - 2;
            string diccionario= texto.Substring(1, final);

            Dictionary<string, double> dict = diccionario.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
       .Select(part => part.Split('='))
       .ToDictionary(split => split[0], split => double.Parse(split[1]));
            armarArbolComp(dict);

            //Splitear lo leido para sacar el diccionario y el texto comprimido aparte

            var result = string.Concat(bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            BitArray bits = new BitArray(result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i]=='1')
                {
                    bits[i] = true;
                }
                else
                {
                    bits[i] = false;
                }
            }
            string unoss = string.Empty;
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
                    if (actual.Izquierda != null )
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

        public bool Hoja (Nodo NodoValidado)//Verificar si es la hoja
        {
            return (NodoValidado.Izquierda == null && NodoValidado.Derecha == null);
        }
    }
}
