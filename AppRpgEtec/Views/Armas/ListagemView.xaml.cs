using AppRpgEtec.ViewModels.Armas;

namespace AppRpgEtec.Views.Armas
{
    public partial class ListagemView : ContentPage
    {
        public ListagemView()
        {
            InitializeComponent();
            BindingContext = new ListagemArmaViewModel();
        }
    }
}