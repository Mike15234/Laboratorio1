using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Laboratorio1_ED2.HuffmanTree
{
    public class Nodo
    {
        public char Caracter { get; set; }
        public double Probabilidad { get; set; } //FRECUENCIA/N
        public Nodo Izquierda { get; set; }
        public Nodo Derecha { get; set; }
        public List<bool>

        Patron(char Caracter, List<bool> data)
        {
            if (Derecha==null && Izquierda==null)
            {
                if (Caracter.Equals(this.Caracter))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right= null;
                if (Izquierda != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Izquierda.Patron(Caracter, leftPath);
                }
                if (Derecha != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Derecha.Patron(Caracter, rightPath);
                }
                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}