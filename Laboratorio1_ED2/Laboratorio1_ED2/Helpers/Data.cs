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
                if(def == 0)
                {
                    arbol.EscrituraArchivo(nombre, rutaEscritura, ArbolHuffman.armarArbol(letters), arbol.DiccionarioFrecuencia);
                }
                else
                {
                    string NuevaRutaD = "";
                    string RutaArchivos = ruta;// + @"\" + "Descomprimidos";
                    string[] Direccion1 = RutaArchivos.Split('\\');
                    string[] nombredes = nombre.Split('.');

                    for (var i = 0; i < Direccion1.Length; i++)
                    {
                        NuevaRutaD += Direccion1[i] + "/";
                    }
                    NuevaRutaD += nombredes[0].ToUpper() + ".txt";

                    if (!File.Exists(NuevaRutaD))
                    {
                        using (var writeStream1 = new FileStream(NuevaRutaD, FileMode.OpenOrCreate))
                        {
                            using (var writer = new BinaryWriter(writeStream1))
                            {
                                writer.Write(arbol.Desifrado(NuevaRutaD));
                                writer.Close();
                            }
                            writeStream1.Close();
                        }
                    }
                    
                }
                
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
            
            return archivo;
        }
        public List<ListaComprimidos> lista(ListaComprimidos archivo)
        {
            Comprimidos.Add(archivo);
            return Comprimidos;
        }

    }
}