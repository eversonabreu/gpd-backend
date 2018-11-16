using System;

namespace EAN.GPD.Domain.Utils
{
    public struct UserLogged
    {
        public long IdUsuario { get; set; }
        public DateTime DataGeracaoToken { get; set; }
        public bool Administrador { get; set; }
        public string Cpf { get; set; }
        public string Matricula { get; set; }
    }
}
