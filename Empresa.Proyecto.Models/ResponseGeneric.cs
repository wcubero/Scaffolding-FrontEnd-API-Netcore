using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresa.Proyecto.Models
{
    public class ResponseGeneric<T>
    {
        public T Model { get; set; }

        public string Exception { get; }

        public DateTime FechaEjecucion { get; }

        public ResponseGeneric() { }

        public ResponseGeneric(T model)
        {
            this.Model = model;

            this.FechaEjecucion = DateTime.Now;
        }

        public ResponseGeneric(Exception exception)
        {
            this.Exception = exception.ToString();

            this.FechaEjecucion = DateTime.Now;
        }
    }
}
