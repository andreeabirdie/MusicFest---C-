using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MusicFest.business;
using MusicFest.domain;
using MusicFest.repository;

namespace MusicFest
{
    public partial class Ticketing : Form
    {
        private TicketingService _ticketingService;
        public Ticketing()
        {
            InitializeComponent();
            _ticketingService = new TicketingService(new TicketDBRepository(), new ShowDBRepository());
            initData();
        }

        private void initData()
        {
            dataGridView1.Rows.Clear();
            foreach (Show s in _ticketingService.getAllShows())
            {
                var index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["artistName"].Value = s.ArtistName;
                dataGridView1.Rows[index].Cells["idShow"].Value = s.Id;
                dataGridView1.Rows[index].Cells["date"].Value = s.DateOfShow.Date;
                dataGridView1.Rows[index].Cells["venue"].Value = s.Venue;
                dataGridView1.Rows[index].Cells["remainingTickets"].Value = s.RemainingTickets;
                dataGridView1.Rows[index].Cells["soldTickets"].Value = s.TotalTickets - s.RemainingTickets;
                if (s.RemainingTickets == 0)
                    dataGridView1.Rows[index].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#BF2A36");
            }
        }

        private void search_Click(object sender, EventArgs e)
        {
            List<Show> shows = _ticketingService.getShowsByDate(dateTimePicker1.Value.Date);
            if (shows.Count == 0)
            {
                MessageBox.Show("Nu s-a gasit niciun spectacol pentru data specificata");
                return;
            }
            SearchResult searchResult = new SearchResult(shows);
            searchResult.ShowDialog();
        }

        private void buy_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Nu ati selectat spectacolul");
                return;
            }
            else
            {
                if (nameBox.Text == "")
                {
                    MessageBox.Show("Nu ati introdus numele");
                    return;
                }
                else
                {
                    int index = dataGridView1.SelectedRows[0].Index;
                    if ((int)dataGridView1.Rows[index].Cells["remainingTickets"].Value==0)
                    {
                        MessageBox.Show("Nu se mai pot cumpara bilete pentru acest spectacol");
                        return;
                    }
                    else
                    {
                        _ticketingService.buyTickets(dataGridView1.Rows[index].Cells["idShow"].Value.ToString(),
                            nameBox.Text,(int)numericUpDown1.Value);
                        dataGridView1.Rows[index].Cells["remainingTickets"].Value = 
                            (int)dataGridView1.Rows[index].Cells["remainingTickets"].Value-numericUpDown1.Value;
                        dataGridView1.Rows[index].Cells["soldTickets"].Value = 
                            (int)dataGridView1.Rows[index].Cells["soldTickets"].Value+numericUpDown1.Value;
                        if (Int32.Parse(dataGridView1.Rows[index].Cells["remainingTickets"].Value.ToString())==0)
                        {
                            dataGridView1.Rows[index].DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#BF2A36");
                        }
                        dataGridView1.ClearSelection();
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells["remainingTickets"].Value == null ||
                Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells["remainingTickets"].Value.ToString())==0)
            {
                numericUpDown1.Maximum = 0;
                numericUpDown1.Minimum = 0;
                return;
            }

            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = (int) dataGridView1.Rows[e.RowIndex].Cells["remainingTickets"].Value;
        }

        private void logOut_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Application.Exit();
        }
    }
}