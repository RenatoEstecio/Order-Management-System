using System.Net;
using EFCore;
using Library.BLL;
using Library.DTO;
using Library.RequestDTO;
using Library.ResponseDTO;
using Library.UTIL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Controllers;
using Xunit;

namespace OrderManagementSystem.Tests.Controllers
{
    public class ProdutoControllerTests
    {
        private readonly Mock<IProdutoService> _serviceMock;
        private readonly ProdutoController _controller;

        public ProdutoControllerTests()
        {
            _serviceMock = new Mock<IProdutoService>();
            _controller = new ProdutoController(_serviceMock.Object);
        }

        [Fact]
        public async Task Create_DeveRetornarOk_QuandoProdutoForCriado()
        {
            // Arrange
            var request = new ProdutoRequest
            {
                Nome = "Produto teste",
                Descricao = "Descricao teste",
                Preco = 10.0m,
                Estoque = 5
            };

            var produto = new Produto
            {
                ProdutoId = 1, // ✔ FIX PRINCIPAL (int)
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                EstoqueDisponivel = request.Estoque,
                Ativo = true
            };

            _serviceMock
                .Setup(x => x.Criar(It.IsAny<ProdutoRequest>()))
                .ReturnsAsync(produto);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<ProdutoCreateResponse>(okResult.Value);

            Assert.Equal("Produto criado com Sucesso", response.Message);
            Assert.NotNull(response.Produto);
        }

        [Fact]
        public async Task Create_DeveRetornarConflict_QuandoProdutoJaExiste()
        {
            // Arrange
            var request = new ProdutoRequest
            {
                Nome = "Produto teste",
                Descricao = "Descricao teste",
                Preco = 10.0m,
                Estoque = 5
            };

            _serviceMock
                .Setup(x => x.Criar(It.IsAny<ProdutoRequest>()))
                .ThrowsAsync(new CustomException(
                    "Produto já cadastrado",
                    HttpStatusCode.Conflict));

            // Act
            var result = await _controller.Create(request);

            // Assert
            var obj = Assert.IsType<ObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status409Conflict, obj.StatusCode);

            var response = Assert.IsType<ProdutoCreateResponse>(obj.Value);

            Assert.Equal("Produto já cadastrado", response.Message);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOk_QuandoExistiremProdutos()
        {
            // Arrange
            var responseMock = new ProdutoListAllResponse
            {
                Message = "Sucesso"
            };

            _serviceMock
                .Setup(x => x.Listar(It.IsAny<string>(), 10, 1))
                .ReturnsAsync(responseMock);

            // Act
            var result = await _controller.GetAll(null, 10, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<ProdutoListAllResponse>(okResult.Value);

            Assert.Equal("Sucesso", response.Message);
        }

        [Fact]
        public async Task GetById_DeveRetornarOk_QuandoProdutoExistir()
        {
            // Arrange
            var produto = new Produto
            {
                ProdutoId = 1,
                Nome = "Produto teste",
                Descricao = "Descricao teste",
                Preco = 10,
                EstoqueDisponivel = 5,
                Ativo = true
            };

            _serviceMock
                .Setup(x => x.Buscar("1"))
                .ReturnsAsync(new ProdutoDetailsResponse(produto));

            // Act
            var result = await _controller.GetById("1");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<ProdutoGetDetailsResponse>(okResult.Value);

            Assert.Equal("Sucesso", response.Message);
            Assert.NotNull(response.Produto);
        }

        [Fact]
        public async Task UpdateStatus_DeveRetornarOk_QuandoAtualizar()
        {
            // Arrange
            var id = "1";

            _serviceMock
                .Setup(x => x.AtivarOuDesativar(id, true))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateStatus(id, true);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<ResponseBase>(okResult.Value);

            Assert.Equal("Alterado", response.Message);
        }

        [Fact]
        public async Task UpdateEstoque_DeveRetornarOk_QuandoAtualizar()
        {
            // Arrange
            var id = "1";

            _serviceMock
                .Setup(x => x.AtualizarEstoque(id, 5))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateEstoque(id, 5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<ResponseBase>(okResult.Value);

            Assert.Equal("Alterado", response.Message);
        }
    }
}