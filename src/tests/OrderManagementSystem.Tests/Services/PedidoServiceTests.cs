using FluentAssertions;
using Library.BLL;
using Library.DTO;
using EFCore;
using Library.Enums;
using Library.Repository;
using Library.ResponseDTO;
using Library.ResponseDTO.Pedido;
using Library.UTIL;
using Moq;
using Xunit;

namespace OrderManagementSystem.Tests.Services
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepo = new();
        private readonly Mock<IClienteRepository> _clienteRepo = new();
        private readonly Mock<IPedidoItemRepository> _itemRepo = new();
        private readonly Mock<IPedidoHistoricoRepository> _historicoRepo = new();

        private readonly PedidoService _service;

        public PedidoServiceTests()
        {
            _service = new PedidoService(
                _pedidoRepo.Object,
                _clienteRepo.Object,
                _itemRepo.Object,
                _historicoRepo.Object
            );
        }

        [Fact]
        public async Task Listar_DeveLancarExcecao_QuandoQuantidadeInvalida()
        {
            Func<Task> act = async () =>
                await _service.Listar("teste", 50, 1);

            await act.Should()
                .ThrowAsync<CustomException>();
        }

        [Fact]
        public async Task Listar_DeveLancarExcecao_QuandoPageMenorQue1()
        {
            Func<Task> act = async () =>
                await _service.Listar("teste", 10, 0);

            await act.Should()
                .ThrowAsync<CustomException>();
        }

        [Fact]
        public async Task Buscar_DeveRetornarPedido_ComSucesso()
        {
            var pedidoGuid = Guid.NewGuid();

            var pedido = new EFCore.Pedido
            {
                PedidoId = 1,
                Id = pedidoGuid,
                ClienteId = 1,
                PedidoStatusId = (int)StatusPedido.Criado,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var cliente = new EFCore.Cliente
            {
                ClienteId = 1,
                Nome = "Teste",
                Email = "teste@email.com",
                Documento = "123",
                Ativo = true
            };

            _pedidoRepo.Setup(x => x.ObterPedido(pedidoGuid))
                .ReturnsAsync(pedido);

            _clienteRepo.Setup(x => x.Buscar(1))
                .ReturnsAsync(cliente);

            _itemRepo.Setup(x => x.ObterProdutos(1))
                .ReturnsAsync(new List<PedidoItemList>());

            _historicoRepo.Setup(x => x.ObterHistorico(1))
                .ReturnsAsync(new List<PedidoHistorico>());

            var result = await _service.Buscar(pedidoGuid);

            result.Should().NotBeNull();
            result.Id.Should().Be(pedidoGuid);
        }

        [Fact]
        public async Task ExecutarPedido_DeveLancarExcecao_ClienteNull()
        {
            _clienteRepo.Setup(x => x.Buscar(It.IsAny<int>()))
                .ReturnsAsync((EFCore.Cliente?)null);

            var request = new ListaPedidoRequest
            {
                cliente = Guid.NewGuid(),
                lista = new List<Itens>()
            };

            Func<Task> act = async () =>
                await _service.ExecutarPedido(request);

            await act.Should()
                .ThrowAsync<CustomException>();
        }
    }
}