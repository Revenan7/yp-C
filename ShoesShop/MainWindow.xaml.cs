using ShoesShop.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ShoesShop
{
    public partial class MainWindow : Window
    {
        public Пользователи CurrentUser { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            ShowAuthPage();
        }

        #region Авторизация

        public void ShowAuthPage()
        {
            MainFrame.Navigate(new AuthPage(this));
            HideUserInfo();
            CurrentUser = null;
            UpdateBackButton();
        }

        public void LoginUser(Пользователи user)
        {
            CurrentUser = user;
            UpdateUserInfo();
            NavigateToMainPage();

            MessageBox.Show($"Успешная авторизация! Добро пожаловать, {user.ФИО}!",
                          "Успех",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        public void LoginAsGuest()
        {
            CurrentUser = null;
            HideUserInfo();
            NavigateToMainPage();

            MessageBox.Show("Вы вошли как гость. Доступен просмотр товаров.",
                          "Гостевой вход",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void UpdateUserInfo()
        {
            if (CurrentUser != null)
            {
                UserInfoPanel.Visibility = Visibility.Visible;
                UsernameTextBlock.Text = CurrentUser.ФИО;
                RoleTextBlock.Text = CurrentUser.Роли?.Роль ?? "Неизвестно";
            }
            else
            {
                HideUserInfo();
            }
        }

        private void HideUserInfo()
        {
            UserInfoPanel.Visibility = Visibility.Collapsed;
            UsernameTextBlock.Text = string.Empty;
            RoleTextBlock.Text = string.Empty;
        }

        #endregion

        #region Навигация

        private void NavigateToMainPage()
        {
            MainFrame.Navigate(new ProductsPage(this));
            UpdateBackButton();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
                UpdateBackButton();
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateBackButton();
        }

        private void UpdateBackButton()
        {
            BackButton.Visibility = MainFrame.CanGoBack &&
                                    !(MainFrame.Content is AuthPage)
                                    ? Visibility.Visible
                                    : Visibility.Collapsed;
        }

        #endregion

        #region Навигация по кнопкам

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductsPage(this));
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EditProductPage(this));
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrdersPage(this));
        }

        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EditOrderPage(this));
        }

        private void SelectProductsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SelectProductsWindow(this));
        }

        #endregion

        #region Выход

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти?",
                                        "Выход из системы",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                CurrentUser = null;
                HideUserInfo();
                ShowAuthPage();

                MessageBox.Show("Вы успешно вышли из системы.",
                              "Выход",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
            }
        }

        #endregion

        #region Сообщения

        public void ShowMessage(string message, string title = "Информация", MessageBoxImage icon = MessageBoxImage.Information)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, icon);
        }

        public void ShowError(string message, string title = "Ошибка")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion
    }
}
