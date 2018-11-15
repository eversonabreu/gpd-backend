using System;
using System.Collections.Generic;

namespace EAN.GPD.Infrastructure.Database.Migrations
{
    public class MigrationBuilder
    {
        private readonly Dictionary<int, string> scripts;
        private readonly string nameTableMigration;

        public MigrationBuilder(string nameTableMigration)
        {
            this.nameTableMigration = nameTableMigration;
            scripts = new Dictionary<int, string>();
            AddScripts();
        }

        private int ObterUltimaExecucaoDeScritps()
        {
            var query = DatabaseProvider.NewQuery($"select code from {nameTableMigration}");
            query.ExecuteQuery();
            return query.GetInt("CODE");
        }

        public void Execute()
        {
            int ultimaExecucao = ObterUltimaExecucaoDeScritps();
            foreach (var item in scripts)
            {
                if (item.Key > ultimaExecucao)
                {
                    DatabaseProvider.NewPersistence(item.Value).Execute();
                    DatabaseProvider.NewPersistence($"update {nameTableMigration} set code = {item.Key}").Execute();
                }
            }
        }

        private void AddScripts()
        {
            scripts.Add(2, @"create table UnidadeMedida (
                            IdUnidadeMedida bigint not null,
                            Codigo int not null,
                            Descricao varchar(255) not null,
                            constraint PkUnidadeMedida primary key(IdUnidadeMedida),
                            constraint UkUnidadeMedida unique (codigo)
                            ); create sequence SeqUnidadeMedida start with 1 increment by 1;");

            scripts.Add(3, @"create table Cargo (
                            IdCargo bigint not null,
                            Codigo int not null,
                            Descricao varchar(255) not null,
                            constraint PkCargo primary key(IdCargo),
                            constraint UkCargo unique (codigo)
                            ); create sequence SeqCargo start with 1 increment by 1;");

            scripts.Add(4, @"create table Departamento (
                            IdDepartamento bigint not null,
                            Codigo int not null,
                            Descricao varchar(255) not null,
                            constraint PkDepartamento primary key(IdDepartamento),
                            constraint UkDepartamento unique (codigo)
                            ); create sequence SeqDepartamento start with 1 increment by 1;");

            scripts.Add(5, @"create table Projeto (
                            IdProjeto bigint not null,
                            Nome varchar(255) not null,
                            Ativo varchar(1) not null,
                            DataInicio timestamp not null,
                            DataTermino timestamp not null,
                            constraint PkProjeto primary key(IdProjeto),
                            constraint UkProjeto unique (Nome)
                            ); create sequence SeqProjeto start with 1 increment by 1;");

            scripts.Add(6, @"create table Indicador (
                            IdIndicador bigint not null,
                            Nome varchar(255) not null,
                            Ativo varchar(1) not null,
                            Identificador varchar(10) not null,
                            ValorPercentualPeso decimal(15,2) not null,
                            TipoRemuneracao int not null,
                            TipoCardinalidade int not null,
                            TipoAcumuloMeta int not null,
                            TipoAcumuloRealizado int not null,
                            IdUnidadeMedida bigint not null,
                            IdUsuarioResponsavel bigint not null,
                            TipoPeriodicidade int not null,
                            Corporativo varchar(1) not null,
                            ValorPercentualCriterio decimal(15,2) not null,
                            TipoCalculo int not null,
                            Formula text,
                            Observacao text,
                            ValorMinimoAtingimento decimal(15,2),
                            ValorMaximoAtingimento decimal(15,2),
                            ValorMinimoPonderado decimal(15,2),
                            ValorMaximoPonderado decimal(15,2),
                            constraint PkIndicador primary key(IdIndicador),
                            constraint UkIndicador unique (Identificador),
                            constraint FkIndicadorUnidadeMedida foreign key (IdUnidadeMedida) references UnidadeMedida(IdUnidadeMedida),
                            constraint FkIndicadorUsuario foreign key (IdUsuarioResponsavel) references Usuario(IdUsuario)
                            ); create sequence SeqIndicador start with 1 increment by 1;");

            scripts.Add(7, @"create table Movimento (
                            IdMovimento bigint not null,
                            IdProjeto bigint not null,
                            IdIndicador bigint not null,
                            DataLancamento timestamp not null,
                            ValorMeta decimal(15, 2) not null,
                            ValorRealizado decimal(15, 2) not null,
                            constraint PkMovimento primary key(IdMovimento),
                            constraint FkMovimentoProjeto foreign key (IdProjeto) references Projeto(IdProjeto),
                            constraint FkMovimentoIndicador foreign key (IdIndicador) references Indicador(IdIndicador)
                            ); create sequence SeqMovimento start with 1 increment by 1;");

            scripts.Add(8, @"create table Arvore (
                            IdArvore bigint not null,
                            Descricao varchar(255) not null,
                            TipoArvore int not null,
                            IdProjeto bigint not null,
                            IdIndicador bigint,
                            IdDepartamento bigint,
                            IdUsuarioGrupo bigint,
                            IdCargo bigint,
                            IdUsuario bigint,
                            NomeIndicador varchar(255),
                            ValorPercentualPeso decimal(15, 2),
                            ValorPercentualCriterio decimal(15, 2),
                            IdArvoreSuperior bigint,
                            constraint PkArvore primary key(IdArvore),
                            constraint FkArvoreProjeto foreign key (IdProjeto) references Projeto(IdProjeto),
                            constraint FkArvoreIndicador foreign key (IdIndicador) references Indicador(IdIndicador),
                            constraint FkArvoreDepartamento foreign key (IdDepartamento) references Departamento(IdDepartamento),
                            constraint FkArvoreUsuarioGrupo foreign key (IdUsuarioGrupo) references UsuarioGrupo(IdUsuarioGrupo),
                            constraint FkArvoreCargo foreign key (IdCargo) references Cargo(IdCargo),
                            constraint FkArvoreUsuario foreign key (IdUsuario) references Usuario(IdUsuario)
                            ); create sequence SeqArvore start with 1 increment by 1;
                            alter table Arvore add constraint FkArvoreArvore foreign key (IdArvoreSuperior) references Arvore(IdArvore);");
        }
    }
}
