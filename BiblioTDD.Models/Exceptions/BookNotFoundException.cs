using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTDD.Models.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException()
            : base("Le livre spécifié est introuvable.") { }
    }
}
