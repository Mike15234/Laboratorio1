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
        List<ListaComprimidos> Comprimidos = new List<ListaComprimidos>();
        ListaComprimidos archivo = new ListaComprimidos();

        public bool Inicial = true;
        string store = string.Empty; //distintos caracteres en un string

        List<Nodo> OrdenProbabilidades = new List<Nodo>();
        const int bufferLength = 1000;
        string letters;
        public void LecturaArchivo(string ruta, string nombre, string rutaEscritura, int def) //LEE EL ARCHIVO
        {
            HuffmanTree.Arbol arbol = new Arbol();
            
                if (def == 0)//lo va a leer
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
                }

                arbol.EscrituraArchivo(nombre, rutaEscritura,arbol.armarArbol(arbol.ArmarDiccionario(letters),letters),arbol.ArmarDiccionario(letters));
                }

            else//si es uno lo va a escribir
                {
                string[] nuevo = nombre.Split('.');
                nuevo[1] = "OUTPUT.txt";
                string nuevoNombre = nuevo[0] + nuevo[1];
                string RutaOut = ruta + nuevoNombre;//CAMVIAR NOMBRE

            }

            
        }

        public ListaComprimidos Operaciones(string name, double original, long comprimido)
        {
            double factorCompresion = original / comprimido;
            double razonCompresion = comprimido / original;
            double porcentaje = 1 - razonCompresion;
            archivo.NombreArchivo = name;
            archivo.FactorCompresion = factorCompresion;
            archivo.RazonCompresio = razonCompresion;
            archivo.PorcentajeCompresion = porcentaje;
            Comprimidos.Add(archivo);
            return archivo;
        }
        public List<ListaComprimidos> lista(ListaComprimidos archivo)
        {
            Comprimidos.Add(archivo);
            return Comprimidos;
        }


    }
}