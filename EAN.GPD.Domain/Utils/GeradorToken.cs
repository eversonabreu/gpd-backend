using EAN.GPD.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace EAN.GPD.Domain.Utils
{
    public static class GeradorToken
    {
        //posicão 2 = Data de geração do token
        //posição 3 = ID do usuário
        public static string GetToken(long idUsuario)
        {
            char charMaiusculo1 = (char)new Random().Next(65, 90);
            var listaCaracteresEspeciais = new List<char> { '!', '@', '#', '$', '%', '&', '*', '|', '?' };
            int indiceCharEspecial = new Random().Next(0, 8);
            char charEspecial = listaCaracteresEspeciais[indiceCharEspecial];

            var sb = new StringBuilder();
            int seconds = DateTime.Now.TimeOfDay.Seconds;
            seconds = seconds < 14 ? 38 : seconds;
            for (int tot = 0; tot <= seconds; tot++)
            {
                int valor = new Random().Next(64, 123);
                if ((valor >= 65 && valor <= 90) || (valor >= 97 && valor <= 122))
                {
                    sb.Append((char)valor);
                }
                else
                {
                    sb.Append(valor);
                }
            }

            Thread.Sleep(5);
            var listaSeparadores = new List<char> { ';', '.', ',', '"' };
            int indiceSeperador = new Random().Next(0, 3);
            char separdor = listaSeparadores[indiceSeperador];
            string numeroAleatorio = new Random().Next(1, 9999).ToString().PadLeft(4, '0');
            string dataCorrente = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            char charMaiusculo2 = (char)new Random().Next(65, 90);
            char charMinusculo1 = (char)new Random().Next(97, 122);
            int numeroAletorioComplementar = new Random().Next(-57852, 99685);
            char charMinusculo2 = (char)new Random().Next(97, 122);
            return $"{charMaiusculo1}{charEspecial}{sb.ToString()}{separdor}{numeroAleatorio}{separdor}{dataCorrente}{separdor}{idUsuario}{separdor}{charMaiusculo2}{charMinusculo1}{numeroAletorioComplementar}{charMinusculo2}";
        }

        private static UserLogged GetUserByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            char separador = ';';
            if (token.Contains('.'))
            {
                separador = '.';
            }
            else if (token.Contains(','))
            {
                separador = ',';
            }
            else if (token.Contains('"'))
            {
                separador = '"';
            }

            string[] colunas = token.Split(separador);
            return new UserLogged
            {
                DataGeracaoToken = DateTime.ParseExact(colunas[2], "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture),
                IdUsuario = long.Parse(colunas[3])
            };
        }

        public static bool ValidateToken(string token, out UserLogged userLogged)
        {
            try
            {
                userLogged = GetUserByToken(token);
                if (DateTime.Now.Subtract(userLogged.DataGeracaoToken).TotalDays > 1)
                {
                    throw new Exception("Token expirado.");
                }

                var query = DatabaseProvider.NewQuery($"select administrador, cpf, matricula from usuario where idusuario = {userLogged.IdUsuario} and ativo = 'S'");
                query.ExecuteQuery();

                if (!query.IsNotEmpty)
                {
                    throw new Exception("Usuário inexistente.");
                }

                userLogged.Administrador = query.GetBoolean("ADMINISTRADOR");
                userLogged.Cpf = query.GetString("CPF");
                userLogged.Matricula = query.GetStringOrNull("MATRICULA");
                return true;
            }
            catch
            {
                userLogged = new UserLogged();
                return false;
            }
        }
    }
}
