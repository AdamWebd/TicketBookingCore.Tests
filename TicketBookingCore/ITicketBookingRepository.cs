using System.Reflection.Metadata.Ecma335;

namespace TicketBookingCore
{
    public interface ITicketBookingRepository
    {
        void Save(TicketBooking ticket);
    }
}
