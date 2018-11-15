using System;

namespace EAN.GPD.Domain.Utils
{
    public struct UserLogged
    {
        public long IdUsuario { get; set; }
        public DateTime DataGeracaoToken { get; set; }
    }
}
