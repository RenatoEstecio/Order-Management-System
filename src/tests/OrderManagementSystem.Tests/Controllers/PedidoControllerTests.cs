using System.Net;
using Library.DTO;
using Library.Enums;
using Library.RequestDTO;
using Library.ResponseDTO;
using Library.BLL;
using Library.ResponseDTO.Pedido;
using Library.UTIL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Controllers;
using Xunit;

namespace OrderManagementSystem.Tests.Controllers
{
    public class PedidoControllerTests
    {
        private readonly Mock<IPedidoService> _serviceMock;
        private readonly PedidoController _controller;

        public PedidoControllerTests()
        {
            _serviceMock = new Mock<IPedidoService>();
            _controller = new PedidoController(_serviceMock.Object);
        }

        [Fact]
        public async Task Create_DeveRetornarOk_QuandoPedidoForCriado()
        {
            var request = new ListaPedidoRequest
            {
                cliente = Guid.NewGuid(), // ✔ obrigatório
                lista = new()
            };

            _serviceMock
                .Setup(x => x.ExecutarPedido(request))
                .Returns(Task.CompletedTask);

            var result = await _controller.Create(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseBase>(okResult.Value);

            Assert.Equal("Pedido criado com Sucesso", response.Message);
        }

        [Fact]
        public async Task Create_DeveRetornarBadRequest_QuandoClienteNaoEncontrado()
        {
            var request = new ListaPedidoRequest
            {
                cliente = Guid.NewGuid(), // ✔ obrigatório
                lista = new()
            };

            _serviceMock
                .Setup(x => x.ExecutarPedido(request))
                .ThrowsAsync(new CustomException(
                    "Cliente não encontrado",
                    HttpStatusCode.BadRequest));

            var result = await _controller.Create(request);

            var obj = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, obj.StatusCode);
        }

        [Fact]
        public async Task Create_DeveRetornarUnauthorized_QuandoClienteNaoAutorizado()
        {
            var request = new ListaPedidoRequest
            {
                cliente = Guid.NewGuid(), // ✔ obrigatório
                lista = new()
            };

            _serviceMock
                .Setup(x => x.ExecutarPedido(request))
                .ThrowsAsync(new CustomException(
                    "Cliente não autorizado",
                    HttpStatusCode.Unauthorized));

            var result = await _controller.Create(request);

            var obj = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status401Unauthorized, obj.StatusCode);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOk_QuandoListaExistir()
        {
            var query = "abc";

            var responseMock = new PedidosGetAllResponse
            {
                Message = "Sucesso"
            };

            _serviceMock
                .Setup(x => x.Listar(query, 10, 1))
                .ReturnsAsync(responseMock);

            var result = await _controller.GetAll(query, 10, 1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PedidosGetAllResponse>(okResult.Value);

            Assert.Equal("Sucesso", response.Message);
        }

        [Fact]
        public async Task GetById_DeveRetornarOk_QuandoPedidoExistir()
        {
            var id = Guid.NewGuid();

            var pedido = new PedidoGetDetailsResponse
            {
                Id = id,
                Message = "Sucesso"
            };

            _serviceMock
                .Setup(x => x.Buscar(id))
                .ReturnsAsync(pedido);

            var result = await _controller.GetById(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PedidoGetDetailsResponse>(okResult.Value);

            Assert.Equal("Sucesso", response.Message);
        }

        [Fact]
        public async Task UpdateStatus_DeveRetornarOk_QuandoAtualizarStatus()
        {
            var id = Guid.NewGuid();

            var request = new PedidoAlteracaoRequest
            {
                alteracao = StatusPedidoAlteracao.Pago, // ✔ CORRIGIDO (não é int)
                Motivo = null
            };

            _serviceMock
                .Setup(x => x.AlterarStatus(
                    id,
                    StatusPedido.Pago,
                    request.Motivo))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateStatus(id, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseBase>(okResult.Value);

            Assert.Equal("Alterado", response.Message);
        }

        [Fact]
        public async Task UpdateStatus_DeveRetornarUnauthorized_QuandoTransicaoInvalida()
        {
            var id = Guid.NewGuid();

            var request = new PedidoAlteracaoRequest
            {
                alteracao = StatusPedidoAlteracao.Enviado // ✔ CORRIGIDO
            };

            _serviceMock
                .Setup(x => x.AlterarStatus(
                    id,
                    StatusPedido.Enviado,
                    request.Motivo))
                .ThrowsAsync(new CustomException(
                    "Transição inválida",
                    HttpStatusCode.Unauthorized));

            var result = await _controller.UpdateStatus(id, request);

            var obj = Assert.IsType<ObjectResult>(result);

            Assert.Equal(StatusCodes.Status401Unauthorized, obj.StatusCode);
        }
    }
}