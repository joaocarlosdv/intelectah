create table Fabricante (
	Id int IDENTITY(1,1) NOT NULL primary key,
	Nome varchar(100)  NOT NULL,
	PaisOrigem varchar(50) NOT NULL,
	WebSite varchar(100) NULL,
	AnoFundacao int NOT NULL,
	deletado bit DEFAULT 0 NOT NULL
);

create table veiculo (
	Id int IDENTITY(1,1) NOT NULL primary key,
	Modelo varchar(100)  NOT NULL,
	AnoFabricacao int NOT NULL,
	Preco numeric(17,2) NOT NULL,
	FabricanteId int NOT NULL foreign key references Fabricante (Id),
	TipoVeiculo int NOT NULL,
	Descricao text NULL,
	deletado bit DEFAULT 0 NOT NULL
);

create table Concessionaria (
	Id int IDENTITY(1,1) NOT NULL primary key,
	Nome varchar(100)  NOT NULL,
	Cep varchar(8) NULL,
	Logradouro varchar(100) NULL,
	Numero varchar(10) NULL,
	Bairro varchar(50) NULL,
	Cidade varchar(50) NULL, 
	Uf varchar(2) NULL,
	Telefone varchar(14) NULL,
	Email varchar(100) NULL,
	CapacidadeMaxima int NOT NULL,
	deletado bit DEFAULT 0 NOT NULL
);

create table Cliente (
	Id int IDENTITY(1,1) NOT NULL primary key,
	Nome varchar(100)  NOT NULL,
	Cpf varchar(11) NOT NULL unique,
	Telefone varchar(15) NULL,
	deletado bit DEFAULT 0 NOT NULL
);

create table Vendas (
	Id int IDENTITY(1,1) NOT NULL primary key,
	VeiculoID int NOT NULL foreign key references Veiculo (Id),
	ConcessionariaID int NOT NULL foreign key references Concessionaria (Id),
	ClienteID int NOT NULL foreign key references Cliente (Id),
	DataVenda DateTime not null,
	PrecoVenda numeric (17, 2) not null,
	ProtocoloVenda varchar(20) not null,
	deletado bit DEFAULT 0 NOT NULL
);

CREATE INDEX ix_Cliente_Cpf ON Cliente (Cpf);

create table Usuario (
	Id int IDENTITY(1,1) NOT NULL primary key,
	Nome varchar(50)  NOT NULL,
	Senha varchar(255) NOT NULL,
	Email varchar(100) NULL,
	NivelAcesso int NOT NULL,
	deletado bit DEFAULT 0 NOT NULL
);