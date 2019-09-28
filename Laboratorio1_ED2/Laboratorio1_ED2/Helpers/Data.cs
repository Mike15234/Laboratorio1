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
        

        CompresorLZW Compresor = new CompresorLZW();

        public bool Inicial = true;
        const int bufferLength = 1000000000;
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

        public List<ListaComprimidos>Operaciones(FileInfo[] Archivos)//Datos para vista de Comprimidos
        {
            List<ListaComprimidos> lista = new List<ListaComprimidos>();
            
            double original=0;
            double comprimido=0;
            double factorCompresion = 0;
            double razonCompresion = 0;
            double porcentaje = 0;
            for (var i = 0; i < Archivos.Length; i++)
            {

                for (int j = 1; j < Archivos.Length; j++)
                {
                    ListaComprimidos archivo = new ListaComprimidos();

                    string[] nombre1=Archivos[i].Name.ToUpper().Split('.');
                    string[] nombre2= Archivos[j].Name.ToUpper().Split('.');
                    if ((nombre1[0].Equals(nombre2[0])) && (Archivos[i].Extension != Archivos[j].Extension))
                    {
                        if ((Archivos[j].Extension == ".txt") && (Archivos[i].Extension != ".txt"))
                        {
                            original = Archivos[i].Length;
                            comprimido = Archivos[j].Length;
                            factorCompresion = original / comprimido;
                            razonCompresion = comprimido / original;
                            porcentaje = 1 - razonCompresion;
                            archivo.NombreArchivo = Archivos[i].Name;                          
                            archivo.FactorCompresion = factorCompresion;
                            archivo.RazonCompresio = razonCompresion;
                            archivo.PorcentajeCompresion = porcentaje;
                            if (!lista.Contains(archivo))
                            {
                                lista.Add(archivo);
                            }
                            
                        }
                        else if ((Archivos[j].Extension == ".txt") && (Archivos[i].Extension != ".txt"))
                        {
                            original = Archivos[j].Length;
                            comprimido = Archivos[i].Length;
                            factorCompresion = original / comprimido;
                            razonCompresion = comprimido / original;
                            porcentaje = 1 - razonCompresion;
                            archivo.NombreArchivo = Archivos[j].Name;
                            archivo.FactorCompresion = factorCompresion;
                            archivo.RazonCompresio = razonCompresion;
                            archivo.PorcentajeCompresion = porcentaje;
                            if (!lista.Contains(archivo))
                            {
                                lista.Add(archivo);
                            }
                        }
                    }
                }
            }          
            return lista;

        }

    }
}