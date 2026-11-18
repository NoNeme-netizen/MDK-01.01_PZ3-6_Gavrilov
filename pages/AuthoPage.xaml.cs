using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;
using PZ3.Models;
using PZ3.Services;

namespace PZ3.pages
{
    public partial class AuthoPage : Page
    {
        private int failedAttempts;              // неудачно пытались
        private AuthService authService = new AuthService();

        public AuthoPage()
        {
            InitializeComponent();
        }

        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage(null, "Гость"));
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (login.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            failedAttempts = failedAttempts + 1;

            if (failedAttempts > 1 && txtCaptcha.Text.Trim() != tblCaptcha.Text)
            {
                MessageBox.Show("Неверно введена капча!");
                GenerateCaptcha();
                return;
            }

            AuthorizedUser user = authService.CheckUser(login, password);

            if (user == null)
            {
                MessageBox.Show("Вы ввели логин или пароль неверно!");
                txtPassword.Password = "";
                GenerateCaptcha();     
                return;
            }

            failedAttempts = 0;
            MessageBox.Show("Вы вошли как: " + user.Login + " (" + user.Role + ")");
            OpenPageForRole(user);
        }
        private void OpenPageForRole(AuthorizedUser user)
        {
            switch (user.Role)
            {
                case "Клиент":
                case "Client":
                    NavigationService.Navigate(new ClientPage(user, user.Role));
                    break;

                default:
                    MessageBox.Show("Для роли \"" + user.Role + "\" страница пока не создана.");
                    break;
            }
        }

        private void GenerateCaptcha()
        {
            tblCaptcha.Visibility = Visibility.Visible;
            txtCaptcha.Visibility = Visibility.Visible;
            txtCaptcha.Text = "";

            tblCaptcha.Text = CaptchaGenerator.GenerateCaptchaText(6);
            tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }
    }
}