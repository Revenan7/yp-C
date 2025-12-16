using ShoesShop.Pages;
using System.Windows;
using System.Windows.Navigation;

namespace ShoesShop
{
    public partial class MainWindow : Window
    {
        public enum UserRole
        {
            Admin = 1,
            Manager = 2,
            Client = 3
        }

        public Пользователи CurrentUser { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            UpdateRoleAccess();
            ShowAuthPage();
        }

        // ================== AUTH ==================

        public void ShowAuthPage()
        {
            MainFrame.Navigate(new AuthPage(this));
            CurrentUser = null;
            HideUserInfo();
            UpdateRoleAccess();
            UpdateBackButton();
        }

        public void LoginUser(Пользователи user)
        {
            CurrentUser = user;
            UpdateUserInfo();
            UpdateRoleAccess();
            MainFrame.Navigate(new ProductsPage(this));
        }

        public void LoginAsGuest()
        {
            CurrentUser = null;
            HideUserInfo();
            UpdateRoleAccess();
            MainFrame.Navigate(new ProductsPage(this));
        }

        private void UpdateUserInfo()
        {
            UserInfoPanel.Visibility = Visibility.Visible;
            UsernameTextBlock.Text = CurrentUser.ФИО;
            RoleTextBlock.Text = CurrentUser.Роли?.Роль ?? "Неизвестно";
        }

        private void HideUserInfo()
        {
            UserInfoPanel.Visibility = Visibility.Collapsed;
            UsernameTextBlock.Text = string.Empty;
            RoleTextBlock.Text = string.Empty;
        }

        // ================== ROLES ==================

        private void UpdateRoleAccess()
        {
            HideAllButtons();

            // гость
            if (CurrentUser == null)
            {
                ProductsButton.Visibility = Visibility.Visible;
                return;
            }

            switch ((UserRole)CurrentUser.ID_роли)
            {
                case UserRole.Admin:
                    ShowAllButtons();
                    break;

                case UserRole.Manager:
                    ProductsButton.Visibility = Visibility.Visible;
                    EditProductButton.Visibility = Visibility.Visible;
                    OrdersButton.Visibility = Visibility.Visible;
                    EditOrderButton.Visibility = Visibility.Visible;
                    break;

                case UserRole.Client:
                    ProductsButton.Visibility = Visibility.Visible;
                    OrdersButton.Visibility = Visibility.Visible;


                    break;
            }
        }

        private void HideAllButtons()
        {
            ProductsButton.Visibility = Visibility.Collapsed;
            EditProductButton.Visibility = Visibility.Collapsed;
            OrdersButton.Visibility = Visibility.Collapsed;
            EditOrderButton.Visibility = Visibility.Collapsed;

        }

        private void ShowAllButtons()
        {
            ProductsButton.Visibility = Visibility.Visible;
            EditProductButton.Visibility = Visibility.Visible;
            OrdersButton.Visibility = Visibility.Visible;
            EditOrderButton.Visibility = Visibility.Visible;

        }

        // ================== NAVIGATION ==================

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new ProductsPage(this));

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new EditProductPage(this));

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new OrdersPage(this));

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
            => MainFrame.Navigate(new EditOrderPage(this));



        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
                MainFrame.GoBack();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            BackButton.Visibility =
                MainFrame.CanGoBack && !(MainFrame.Content is AuthPage)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        // ================== LOGOUT ==================

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?",
                                "Выход",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ShowAuthPage();
            }
        }

        // ================== MESSAGES ==================

        public void ShowError(string message, string title = "Ошибка")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
