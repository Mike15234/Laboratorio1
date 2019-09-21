using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;


namespace Laboratorio1_ED2.LZW
{
    public class CompresorLZW
    {
        string escritura = string.Empty;
        public string EscrituraDiccionario<Key, Value>(Dictionary<string, int>Diccionario)
        {
            return "{" + string.Join(",", Diccionario.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";

        }

        public Byte[] Comprimir(string TextoCompleto)
        {
            Dictionary<string, int> DiccionarioLZW = new Dictionary<string, int>();
            int contador = 1;
            for (int i = 0; i < TextoCompleto.Length; i++)
            {
                if (!DiccionarioLZW.ContainsKey(TextoCompleto[i].ToString()))
                {
                    DiccionarioLZW.Add(TextoCompleto[i].ToString(), contador);
                    contador++;
                }
            }

            escritura=EscrituraDiccionario<string, int>(DiccionarioLZW);

            string previo = string.Empty;
            List<int> ListaSalida = new List<int>();

            foreach (var siguiente in TextoCompleto)
            {
                string Concatenado = previo + siguiente;
                if (DiccionarioLZW.ContainsKey(Concatenado))
                {
                    previo = Concatenado;
                }
                else
                {
                    ListaSalida.Add(DiccionarioLZW[previo]);
                    DiccionarioLZW.Add(Concatenado, DiccionarioLZW.Count+1);
                    previo = siguiente.ToString();
                }
            }
            if (!string.IsNullOrEmpty(previo))
            {
                ListaSalida.Add(DiccionarioLZW[previo]);
            }
            string Escrito = string.Join(",", ListaSalida);
            
            string[] Arreglo = Escrito.Split(',');
            byte[] Bytes = new byte[ListaSalida.Count];
            for (var i = 0; i < ListaSalida.Count; i++)
            {
                Bytes[i] = Convert.ToByte(Arreglo[i]);
            }
            return Bytes;
        }



        public string Descomprimido(string ruta)
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

            bytes = bytes.Where((item, index) => index < indice).ToArray();//Recupera los bytes escritos

            string texto = string.Empty;
            using (StreamReader streamReader = new StreamReader(ruta))
            {
                streamReader.BaseStream.Position = indice + 1;
                texto = streamReader.ReadLine();
                streamReader.Close();
            }

            int final = texto.Length - 2;

            string diccionario = texto.Substring(1, final);

            Dictionary<int, string> DiccionarioComprimido = diccionario.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
         .Select(part => part.Split('=')).ToDictionary(split => int.Parse(split[1]), split => split[0]);//Genera el diccionario escrito 

            string[] Arreglo = new string[bytes.Length];

            for (var i = 0; i < bytes.Length; i++)
            {
                Arreglo[i] = Convert.ToString(bytes[i]);
            }

            List<int>Comprimido=Arreglo.Select(x => Int32.Parse(x)).ToList();
            string Previo = DiccionarioComprimido[Comprimido[0]];
            Comprimido.RemoveAt(0);
            StringBuilder Descomprimido = new StringBuilder(Previo);

            foreach (int Cuenta in Comprimido)
            {
                string Entrada = null;
                if (DiccionarioComprimido.ContainsKey(Cuenta))
                    Entrada = DiccionarioComprimido[Cuenta];
                else if (Cuenta== DiccionarioComprimido.Count)
                    Entrada = Previo + Previo[0];

                Descomprimido.Append(Entrada);

                // new sequence; add it to the dictionary

                DiccionarioComprimido.Add(DiccionarioComprimido.Count+1, Previo + Entrada[0]);

                Previo = Entrada;
            }


            return Descomprimido.ToString();//CAMBIAR

        }

        public void EscrituraLZW(string ruta,string Texto)
        {
            byte[] Bytes = new byte[Texto.Length];

            Bytes = Comprimir(Texto);
            string Diccionario = '|'+escritura;
            if (!File.Exists(ruta))
            {
                using (var writeStream1 = new FileStream(ruta, FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(writeStream1))
                    {
                        foreach (var item in Bytes)
                        {
                            writer.Write(item);
                        }
                        writer.Close();
                    }
                    writeStream1.Close();
                    StreamWriter WriteReportFile = File.AppendText(ruta);
                    WriteReportFile.WriteLine(Diccionario);
                    WriteReportFile.Close();
                }
            }
        }
        

    }
}