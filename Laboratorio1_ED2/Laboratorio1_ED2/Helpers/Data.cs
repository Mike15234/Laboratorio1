using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Laboratorio1_ED2.Controllers;
using Laboratorio1_ED2.Models;
using Laboratorio1_ED2.HuffmanTree;

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
        //diccionario de letras is missing
    
        public bool Inicial = true;
        int totalCaracteres;
        string leerlineas = "";
        string store = string.Empty; //distintos caracteres en un string

        //uso de lista para el ordenamiento del probabilidades y letras **pendientee
        List<Nodo> OrdenProbabilidades = new List<Nodo>();
        const int bufferLength = 1000;
        string letters;
        public void LecturaArchivo(string ruta) //YO NO LO ENTIENDOS
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

                    //String creada luego de lectura ascii
                    letters = System.Text.Encoding.ASCII.GetString(byteBuffer);

                }
                Conteo_y_Probabilidad(letters);
                ArbolHuffman.armarArbol(letters);
            }
            // var leerlineas = "";
           // var objReader = new StreamReader(ruta);
           // leerlineas = objReader.ReadLine();
           // while (objReader.ReadLine() != null)
           // {
           //     leerlineas += objReader.ReadLine();
           // }
           //// leerlineas += objReader.ReadLine();

           // objReader.Close();
          
        }

        public void Conteo_y_Probabilidad(string leerlineas)
        {
            char letra;
            //arreglo de caracteres del archivo
            char[] caracteres = leerlineas.ToArray() ;
            char[] contado = leerlineas.ToArray();//Del mismo tamaño de varios "B y A"(No importa)


            for (int i = 0; i < leerlineas.Length; i++)
            {
                letra = caracteres[i];
                if (!store.Contains(letra))
                {
                    store += letra;
                }
            }
            char[] varios = store.ToArray();//Vector de caracteres distintos
            int[] cvarios = new int[store.Length];//vector del conteo de los caracteres is repetir
            for (var i = 0; i < leerlineas.Length; i++)
            {
                contado[i] = 'a';
            }
            totalCaracteres = caracteres.Length;

            string resultado = string.Empty;
            var cantCaracter = 0;
            for (var i = 0; i < leerlineas.Length; i++)
            {
                letra = caracteres[i];

                //revisa cuales fueron contados
                for (var j = 0; j < leerlineas.Length; j++)
                {
                    if ((caracteres[j] == letra) && (contado[j] != 'b'))
                    {
                        cantCaracter++;
                        contado[j] = 'b';
                    }
                }
                for (var h= 0; h< store.Length; h++)
                {
                    if (varios[h]==letra)
                    {
                        cvarios[h] = cantCaracter;
                    }
                    
                }
                
                cantCaracter = 0;
            }
            //SE OBTIENEN LAS PROBABILIDADES DE CADA LETRA
            double[] probabilidad = new double[cvarios.Length];
            for (var i = 0; i < cvarios.Length; i++)
            {
                probabilidad[i] = (cvarios[i] / totalCaracteres);
               // nodo.Probabilidad = probabilidad[i];
                //se añaden a donde se ordenara para el arbol
                       
            }
        }

    }

}

//Enviar el arreglo de varios y probabilidad al arbol.
//Crear una lista contenga el caracter y su probabilidad, eso se manda al arbol....
//Arbol(varios[],probabilidad[]);
//Recorrido del arbol para los prefijos.
//Guardar diccionario con prefijos y caracteres.
//Reescribir el texto con codigos 
//Documento ".Huff" que guarde el diccionario y el codigo.
