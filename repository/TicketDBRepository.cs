using System;
using System.Data.SQLite;
using log4net;
using MusicFest.domain;
using NUnit.Framework;

namespace MusicFest.repository
{
    public class TicketDBRepository : ITicketRepository
    {
        private static readonly ILog log = LogManager.GetLogger("TicketDBRepository");

        public TicketDBRepository()
        {
            log.Info("Creating a TicketDBRepository");
        }

        public void Save(Ticket ticket)
        {
            log.InfoFormat("Entering save for ticket {0}", ticket.ToString());
            String sql = "INSERT INTO Tickets values (@id, @number, @buyerName)";
            var conn = DBUtils.getConnection();
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                var paramId = cmd.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = ticket.IdShow;
                cmd.Parameters.Add(paramId);

                var paramNumber = cmd.CreateParameter();
                paramNumber.ParameterName = "@number";
                paramNumber.Value = ticket.NrTicket;
                cmd.Parameters.Add(paramNumber);

                var paramName = cmd.CreateParameter();
                paramName.ParameterName = "@buyerName";
                paramName.Value = ticket.BuyerName;
                cmd.Parameters.Add(paramName);

                var result = cmd.ExecuteNonQuery();
                DBUtils.closeConnection();
                log.Info("Save was successful");
                if (result == 0)
                {
                    log.Info("Save was unsucccessful");
                    throw new Exception("No ticket was saved!");
                }
            }
        }

        public Ticket FindOne(string id, int number)
        {
            log.InfoFormat("Entering findOne for ticket nr {0}, show {1}", number, id);
            String sql = "SELECT * FROM Tickets where idShow = @id and numberTicket = @number";
            var conn = DBUtils.getConnection();
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                var param1 = cmd.CreateParameter();
                param1.ParameterName = "@id";
                param1.Value = id;
                cmd.Parameters.Add(param1);

                var param2 = cmd.CreateParameter();
                param2.ParameterName = "@number";
                param2.Value = number;
                cmd.Parameters.Add(param2);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Ticket t = new Ticket(reader.GetString(0), reader.GetInt32(1), reader.GetString(2));
                        log.InfoFormat("Exiting findOne with value {0}", t);
                        DBUtils.closeConnection();
                        return t;
                    }

                    log.Info("Exiting findOne with value null");
                    DBUtils.closeConnection();
                    return null;
                }
            }
        }

        public void Delete(string id, int number)
        {
            log.InfoFormat("Entering delete for ticket nr {0}, show {1}", number, id);
            String sql = "DELETE FROM Tickets where idShow = @id and numberTicket = @number";
            var conn = DBUtils.getConnection();
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                var param1 = cmd.CreateParameter();
                param1.ParameterName = "@id";
                param1.Value = id;
                cmd.Parameters.Add(param1);

                var param2 = cmd.CreateParameter();
                param2.ParameterName = "@number";
                param2.Value = number;
                cmd.Parameters.Add(param2);

                var dataR = cmd.ExecuteNonQuery();
                DBUtils.closeConnection();
                log.Info("Delete was successful");
                if (dataR == 0)
                {
                    log.Info("Delete was unsuccessful");
                    throw new Exception("No ticket was deleted!");
                }
            }
        }
    }

    [TestFixture]
    public class TicketsRepoTests
    {
        ITicketRepository _ticketRepository = new TicketDBRepository();

        [Test]
        public void TicketExists()
        {
            Ticket ticket = _ticketRepository.FindOne("1", 3);
            Assert.IsTrue(ticket.BuyerName == "Oana Vrabie");
        }

        [Test]
        public void NoTicketFound()
        {
            Ticket ticket = _ticketRepository.FindOne("1", 1500);
            Assert.IsNull(ticket);
        }

        [Test]
        public void SuccessfulSave()
        {
            Ticket ticket = new Ticket("3", 3, "Andreea Vrabie");
            _ticketRepository.Save(ticket);
            Ticket found = _ticketRepository.FindOne("3", 3);
            Assert.IsTrue(found.BuyerName == "Andreea Vrabie");
            _ticketRepository.Delete(ticket.IdShow, ticket.NrTicket);
        }

        [Test]
        public void UnSuccessfulSave()
        {
            try
            {
                Ticket ticket = new Ticket("1", 3, "Andreea Vrabie");
                _ticketRepository.Save(ticket);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.Pass();
            }
        }
    }
}