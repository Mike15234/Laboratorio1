using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Laboratorio1_ED2.Controllers;
using Laboratorio1_ED2.Models;
using Laboratorio1_ED2.HuffmanTree;
using System.Collections;

namespace Laboratorio1_ED2.Helpers
{
    public class Data
    {
        //ESTO ES EL SINGLETON
        private static Data instancia = null;
        public static Data Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Data();
                }
                return instancia;
            }
        }


        Arbol ArbolHuffman = new Arbol();
        Nodo nodo = new Nodo();
    
        public bool Inicial = true;
        int totalCaracteres;
        string leerlineas = "";
        string store = string.Empty; //distintos caracteres en un string

        List<Nodo> OrdenProbabilidades = new List<Nodo>();
        const int bufferLength = 1000;
        string letters;
        public void LecturaArchivo(string ruta) //LEE EL ARCHIVO
        {
            using (var stream = new FileStream(ruta, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var byteBuffer = new byte[bufferLength];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(bufferLength);
                    }
                    letters = System.Text.Encoding.ASCII.GetString(byteBuffer);

                }
                ArbolHuffman.armarArbol(letters);
            }
        }
        public void EscrituraArchivo(string ruta, BitArray bits)
        {

            if (!File.Exists(ruta + @"\" + "FoldrA Guardar" + @"\" + "datosa meter" + "ArchivoComprimido.huff"))
            {
                //getfileName para nombrar el archivo comprimido
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(ruta + @"\" + "Arbol" + @"\" + "VALORES" + tabla.ToUpper() + ".arbol"))
                {

                    streamWriter.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(bits));
                    streamWriter.Close();
                }

            }



            //string completa = ruta + ".huff"; //PASARLO SOLO A .HUFF DENTRO DE UNA CARPETA
            //var writer = new StreamWriter(ruta);
            //for (int i = 0; i < bits.Length; i++)
            //{
            //    writer.Write(bits[i]);
            //}
            //writer.Close();
        }
    }
}

//Guardar diccionario con prefijos y caracteres.
//Reescribir el texto con codigos 
//Documento ".Huff" que guarde el diccionario y el codigo.
