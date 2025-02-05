using Moq;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;

namespace TicketBookingCore.Tests
{
    public class TicketBookingRequestProcessorTests
    {
        private readonly TicketBookingRequestProcessor _processor;
        private readonly Mock<ITicketBookingRepository> _ticketBookingRepositoryMock;

        public TicketBookingRequestProcessorTests()
        {
            _ticketBookingRepositoryMock = new Mock<ITicketBookingRepository>();
            _processor = new TicketBookingRequestProcessor(_ticketBookingRepositoryMock.Object);
        }

        [Fact]
        public void ShouldReturnTicketBookingResultWithRequestValues()
        {
            var request = new TicketBookingRequest
            {
                FirstName = "Adam",
                LastName = "Nilsson",
                Email = "MaxAdamNilsson@gmail.com"
            };

            //Act
            TicketBookingResponse response = _processor.Book(request);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(request.FirstName, response.FirstName);
            Assert.Equal(request.LastName, response.LastName);
            Assert.Equal(request.Email, response.Email);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            //Act
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.Book(null));
            //Assert
            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveToDatabase()
        {
            {
                var request = new TicketBookingRequest
                {
                    FirstName = "Adam",
                    LastName = "Nilsson",
                    Email = "MaxAdamNilsson@gmail.com"
                };

                TicketBooking savedTicket = null;
                _ticketBookingRepositoryMock
                    .Setup(r => r.Save(It.IsAny<TicketBooking>()))
                    .Callback<TicketBooking>(ticket => savedTicket = ticket);

                _processor.Book(request);

                _ticketBookingRepositoryMock.Verify(r => r.Save(It.IsAny<TicketBooking>()), Times.Once);

                Assert.NotNull(savedTicket);
                Assert.Equal(request.FirstName, savedTicket.FirstName);
                Assert.Equal(request.LastName, savedTicket.LastName);
                Assert.Equal(request.Email, savedTicket.Email);
            }

        }
    }
}