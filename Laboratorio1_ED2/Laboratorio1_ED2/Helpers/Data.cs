using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Laboratorio1_ED2.Controllers;
using Laboratorio1_ED2.Models;
using Laboratorio1_ED2.HuffmanTree;
using Laboratorio1_ED2.LZW;
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

        CompresorLZW Compresor = new CompresorLZW();

        public bool Inicial = true;
        const int bufferLength = 1000;
        string letters;

        public void LecturaArchivo(string ruta, string nombre, string rutaEscritura, int def) //LEE EL ARCHIVO
        {
            HuffmanTree.Arbol arbol = new Arbol();

            if (def == 0)
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

                arbol.EscrituraArchivo(nombre, rutaEscritura, arbol.armarArbol(arbol.ArmarDiccionario(letters), letters), arbol.ArmarDiccionario(letters));
            }
            else if (def == 1)
            {


                string[] nuevo = nombre.Split('.');
                nuevo[1] = "OUTPUTHUFF.txt";

                string[] arreglo = ruta.Split('.');
                string RutaOut = arreglo[0] + nuevo[1];//CAMBIAR NOMBRE

                if (!File.Exists(RutaOut))
                {
                    StreamWriter streamWriter = new StreamWriter(RutaOut);
                    streamWriter.WriteLine(arbol.Desifrado(ruta));
                    streamWriter.Close();
                }
            }
            else if (def == 2)
            {

                using (var stream = new FileStream(ruta, FileMode.Open))//LEER ARCHIVO
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

                string NuevaRuta = "";
                string[] Direccion = rutaEscritura.Split('\\');
                string[] NuevoNombre = nombre.Split('.');
                for (var i = 0; i < Direccion.Length; i++)
                {
                    NuevaRuta += Direccion[i] + "/";
                }
                NuevaRuta += NuevoNombre[0] + ".LZW";

                Compresor.EscrituraLZW(NuevaRuta, letters);

            }

            else if (def == 3)
            {

                string[] nuevo = nombre.Split('.');
                nuevo[1] = "OUTPUTLZW.txt";

                string[] arreglo = ruta.Split('.');
                string RutaOut = arreglo[0] + nuevo[1];//CAMBIAR NOMBRE

                if (!File.Exists(RutaOut))
                {
                    StreamWriter streamWriter = new StreamWriter(RutaOut);
                    streamWriter.WriteLine(Compresor.Descomprimido(ruta));
                    streamWriter.Close();
                }
            }
        }
        public ListaComprimidos Operaciones(string name, double original, long comprimido)//Datos para vista de Comprimidos
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
        public List<ListaComprimidos> lista(ListaComprimidos archivo)//Agregar los comprimidos a la lista 
        {
            Comprimidos.Add(archivo);
            return Comprimidos;
        }
    }
}