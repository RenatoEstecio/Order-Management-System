using System.Net;
using EFCore;
using Library.BLL;
using Library.DTO;
using Library.ResponseDTO;
using Library.ResponseDTO.Clientes;
using Library.UTIL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Controllers;
using Xunit;

namespace OrderManagementSystem.Tests.Controllers
{
    public class ClienteControllerTests
    {
        private readonly Mock<IClienteService> _serviceMock;
        private readonly ClienteController _controller;

        public ClienteControllerTests()
        {
            _serviceMock = new Mock<IClienteService>();
            _controller = new ClienteController(_serviceMock.Object);
        }

        [Fact]
        public async Task Create_DeveRetornarOk_QuandoClienteForCriado()
        {
            // Arrange
            var request = new ClienteRequest
            {
                Nome = "João",
                Email = "joao@email.com"
            };

            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = "João",
                Email = "joao@email.com",
                Ativo = true
            };

            _serviceMock
                .Setup(x => x.Criar(request))
                .ReturnsAsync(cliente);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<ClienteCreateResponse>(okResult.Value);

            Assert.Equal("Cliente criado com Sucesso", response.Message);
            Assert.NotNull(response.Cliente);
        }
		
		[Fact]
		public async Task Create_DeveRetornarBadRequest_QuandoCpfForInvalido()
		{
			// Arrange
			var request = new ClienteRequest
			{
				Nome = "João",
				Email = "joao@email.com",
				Documento = "111.111.111-11"
			};

			_serviceMock
				.Setup(x => x.Criar(request))
				.ThrowsAsync(new CustomException("CPF inválido", HttpStatusCode.BadRequest));

			// Act
			var result = await _controller.Create(request);

			// Assert
			var obj = Assert.IsType<ObjectResult>(result.Result);

			Assert.Equal(400, obj.StatusCode);
		}
		
		[Fact]
		public async Task Create_DeveRetornarBadRequest_QuandoEmailForInvalido()
		{
			// Arrange
			var request = new ClienteRequest
			{
				Nome = "João",
				Email = "@email",
				Documento = "859.279.600-81"
			};

			_serviceMock
				.Setup(x => x.Criar(request))
				.ThrowsAsync(new CustomException("CPF inválido", HttpStatusCode.BadRequest));

			// Act
			var result = await _controller.Create(request);

			// Assert
			var obj = Assert.IsType<ObjectResult>(result.Result);

			Assert.Equal(400, obj.StatusCode);
		}

        [Fact]
        public async Task Create_DeveRetornarConflict_QuandoCustomExceptionForLancada()
        {
            // Arrange
            var request = new ClienteRequest();

            _serviceMock
                .Setup(x => x.Criar(request))
                .ThrowsAsync(new CustomException(
                    "Cliente já cadastrado",
                    HttpStatusCode.Conflict));

            // Act
            var result = await _controller.Create(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status409Conflict, objectResult.StatusCode);

            var response = Assert.IsType<ClienteCreateResponse>(objectResult.Value);

            Assert.Equal("Cliente já cadastrado", response.Message);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOk_QuandoListaExistir()
        {
            // Arrange
            var responseMock = new ClienteListAllResponse
            {
                Message = "Sucesso"
            };

            var query = string.Empty;

            _serviceMock
                .Setup(x => x.Listar(query, 10, 1))
                .ReturnsAsync(responseMock);

            // Act
            var result = await _controller.GetAll(query, 10, 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<ClienteListAllResponse>(okResult.Value);

            Assert.Equal("Sucesso", response.Message);
        }

        [Fact]
        public async Task GetById_DeveRetornarOk_QuandoClienteExistir()
        {
            // Arrange
            var id = Guid.NewGuid();

            var cliente = new Cliente
            {
                Id = id,
                Nome = "Maria",
                Email = "maria@email.com",
                Ativo = true
            };

            _serviceMock
                .Setup(x => x.Buscar(id))
                .ReturnsAsync(new ClienteDetailsResponse(cliente));

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var response = Assert.IsType<ClienteGetDetailsResponse>(okResult.Value);

            Assert.Equal("Sucesso", response.Message);
            Assert.NotNull(response.Cliente);
        }

        [Fact]
        public async Task UpdateStatus_DeveRetornarOk_QuandoAtualizarStatus()
        {
            // Arrange
            var id = Guid.NewGuid();

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
        public async Task UpdateStatus_DeveRetornar404_QuandoClienteNaoExistir()
        {
            // Arrange
            var id = Guid.NewGuid();

            _serviceMock
                .Setup(x => x.AtivarOuDesativar(id, false))
                .ThrowsAsync(new CustomException(
                    "Cliente não encontrado",
                    HttpStatusCode.NotFound));

            // Act
            var result = await _controller.UpdateStatus(id, false);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);

            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);

            var response = Assert.IsType<ResponseBase>(objectResult.Value);

            Assert.Equal("Cliente não encontrado", response.Message);
        }
    }
}