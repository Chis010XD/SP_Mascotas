using System;
using System.Collections.Generic;

namespace masco.Models
{
    public partial class Perro
    {
        public decimal IdMasc { get; set; }
        public string Nombre { get; set; } = null!;
        public string Raza { get; set; } = null!;
        public string Color { get; set; } = null!;
        public decimal Tamanio { get; set; }

  

        internal static object OrderBy(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
