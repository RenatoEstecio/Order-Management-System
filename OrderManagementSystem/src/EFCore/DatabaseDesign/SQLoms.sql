/*

Scaffold-DbContext "Server=localhost,1433;Initial Catalog=Oms_bd;Persist Security Info=False;User ID=sa;Password=Oms1600@SQL;MultipleActiveResultSets=False;Encrypt=True;Max Pool Size=1500;TrustServerCertificate=True;Connection Timeout=300;" Microsoft.EntityFrameworkCore.SqlServer -Context ContextEFCore -force -NoPluralize

*/

use oms_bd;
DROP TABLE IF EXISTS PedidoHistorico;
DROP TABLE IF EXISTS PedidoItem;
DROP TABLE IF EXISTS Pedido;
DROP TABLE IF EXISTS Produto;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS PedidoStatus;


CREATE TABLE PedidoStatus
(
    PedidoStatusId INT NOT NULL identity(1,1) PRIMARY KEY,
    
    Nome NVARCHAR(50) NOT NULL
);

INSERT INTO PedidoStatus (Nome)
VALUES
('Criado'),
('Pago'),
('Enviado'),
('Cancelado');

CREATE TABLE Cliente
(
    ClienteId int NOT NULL identity(1,1) PRIMARY KEY,
    
    Id UNIQUEIDENTIFIER NOT NULL unique default newid(),

    Nome NVARCHAR(200) NOT NULL,
    
    Email NVARCHAR(200) NOT NULL Unique,
    
    Documento NVARCHAR(20) NOT NULL Unique,
    
    Ativo BIT NOT NULL DEFAULT 1,
    
    CreatedAt DATETIME2 NOT NULL default getdate(),
    
    UpdatedAt DATETIME2 NOT NULL
);


CREATE TABLE Pedido
(
    PedidoId int NOT NULL identity(1,1) PRIMARY KEY,

    Id UNIQUEIDENTIFIER NOT NULL unique default newid(),
    
    ClienteId int NOT NULL,
    
    PedidoStatusId INT NOT NULL,
    
    ValorTotal DECIMAL(18,2) NOT NULL,
    
    CreatedAt DATETIME2 NOT NULL default getdate(),
    
    UpdatedAt DATETIME2 NOT NULL,

    CONSTRAINT FK_Pedidos_status
        FOREIGN KEY (PedidoStatusId)
        REFERENCES PedidoStatus(PedidoStatusId),

    CONSTRAINT FK_Pedidos_Clientes
        FOREIGN KEY (ClienteId)
        REFERENCES Cliente(ClienteId),

    CONSTRAINT CK_Pedidos_ValorTotal
        CHECK (ValorTotal >= 0)
);

CREATE TABLE Produto
(
    ProdutoId int NOT NULL identity(1,1) PRIMARY KEY,    

    Id nvarchar(20) unique default 'P-' + (LEFT(CAST(NEWID() AS VARCHAR(36)), 4)),
    
    Nome NVARCHAR(200) NOT NULL,
    
    Descricao NVARCHAR(1000) NULL,
    
    Preco DECIMAL(18,2) NOT NULL,
    
    EstoqueDisponivel INT NOT NULL default 0,
    
    Ativo BIT NOT NULL DEFAULT 1,
    
    CreatedAt DATETIME2 NOT NULL default getdate(),
    
    UpdatedAt DATETIME2 NOT NULL,

    CONSTRAINT CK_Produtos_Preco
        CHECK (Preco > 0),

    CONSTRAINT CK_Produtos_Estoque
        CHECK (EstoqueDisponivel >= 0)
);

CREATE TABLE PedidoHistorico
(
    PedidoHistoricoId int NOT NULL identity(1,1) PRIMARY KEY,
        
    PedidoId INT NOT NULL,
    
    PedidoStatusId INT NOT NULL,   

    Motivo Nvarchar(400) NULL,
    
    CreatedAt DATETIME2 NOT NULL default getdate(),
       
    CONSTRAINT FK_Pedidosstatus_PedidoHistorico
        FOREIGN KEY (PedidoStatusId)
        REFERENCES PedidoStatus(PedidoStatusId),

    CONSTRAINT FK_Pedidos_PedidoHistorico
        FOREIGN KEY (PedidoId)
        REFERENCES Pedido(PedidoId)

);


CREATE TABLE PedidoItem
(
    PedidoItemId int NOT NULL identity(1,1) PRIMARY KEY,    
    
    PedidoId int NOT NULL,
    
    ProdutoId int NOT NULL,
    
    Quantidade INT NOT NULL,
    
    PrecoUnitario DECIMAL(18,2) NOT NULL,
    
    ValorTotal DECIMAL(18,2) NOT NULL,
    
    CreatedAt DATETIME2 NOT NULL,
    
    UpdatedAt DATETIME2 NOT NULL,

    CONSTRAINT FK_PedidoItens_Pedidos
        FOREIGN KEY (PedidoId)
        REFERENCES Pedido(PedidoId),

    CONSTRAINT FK_PedidoItens_Produtos
        FOREIGN KEY (ProdutoId)
        REFERENCES Produto(ProdutoId),

    CONSTRAINT CK_PedidoItens_Quantidade
        CHECK (Quantidade > 0),

    CONSTRAINT CK_PedidoItens_Preco
        CHECK (PrecoUnitario > 0),

    CONSTRAINT CK_PedidoItens_ValorTotal
        CHECK (ValorTotal > 0)
);
