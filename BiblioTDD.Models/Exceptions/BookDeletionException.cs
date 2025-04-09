using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioTDD.Models.Exceptions
{
    public class BookDeletionException : Exception
    {
        public BookDeletionException(string? message = null)
            : base(message ?? "Le livre ne peut pas être supprimé car il a des emprunts actifs.") { }
    }
}
