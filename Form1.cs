using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MusicFest.business;
using MusicFest.repository;

namespace MusicFest
{
    public partial class Form1 : Form
    {
        private UserService _userService;
        public Form1()
        {
            InitializeComponent();
            _userService = new UserService(new UserDBRepository());
            usernameTextBox.Text = "admin";
            passwordTextBox.Text = "admin";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(usernameTextBox.Text=="" || passwordTextBox.Text=="")
            {
                MessageBox.Show("Nu ati introdus username-ul si/sau parola");
                return;
            }

            if (_userService.validateUser(usernameTextBox.Text, passwordTextBox.Text))
            {
                this.Hide();
                Ticketing ticketing = new Ticketing();
                ticketing.Show();
            }
            else MessageBox.Show("Logare esuata");
        }
    }
}