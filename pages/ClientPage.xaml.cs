using System.Windows.Controls;
using PZ3.Models;

namespace PZ3.pages
{
    public partial class ClientPage : Page
    {
        public ClientPage(AuthorizedUser user, string role)
        {
            InitializeComponent();

            if (user == null)
            {
                tblWelcome.Text = "Вы вошли как гость";
            }
            else
            {
                tblWelcome.Text = "Добро пожаловать, " + user.Login + "!\nВаша роль: " + role;
            }
        }
    }
}