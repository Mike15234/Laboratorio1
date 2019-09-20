using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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