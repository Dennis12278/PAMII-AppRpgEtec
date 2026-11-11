using AppRpgEtec.ViewModels;
using AppRpgEtec.Views.Personagens;
using AppRpgEtec.Views.Armas;

namespace AppRpgEtec
{
    public partial class AppShell : Shell
    {
        AppShellViewModel viewModel;

        public AppShell()
        {
            InitializeComponent();

            viewModel = new AppShellViewModel();
            BindingContext = viewModel;

            string login = Preferences.Get("UsuarioUsername", string.Empty);
            lblLogin.Text = $"Login: {login}";

            Routing.RegisterRoute("cadPersonagemView", typeof(CadastroPersonagemView));

            Routing.RegisterRoute("cadArmaView", typeof(CadastroArmaView));
        }
    }
}