using System;
using System.Collections.Generic;
using MusicFest.domain;
using MusicFest.repository;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace MusicFest.business
{
    public class TicketingService
    {
        private ITicketRepository _ticketRepository;
        private IShowRepository _showRepository;

        public TicketingService(ITicketRepository ticketRepository, IShowRepository showRepository)
        {
            _ticketRepository = ticketRepository;
            _showRepository = showRepository;
        }

        public List<Show> getAllShows()
        {
            return _showRepository.FindAll();
        }

        public List<Show> getShowsByDate(DateTime date)
        {
            return _showRepository.findByDate(date);
        }

        public void buyTickets(String idShow, String buyer, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                Show s = _showRepository.FindOne(idShow);
                int number = s.TotalTickets - s.RemainingTickets + 1;
                _ticketRepository.Save(new Ticket(idShow, number, buyer));
                s.RemainingTickets = s.RemainingTickets - 1;
                _showRepository.Update(s);
            }
        }
    }
}