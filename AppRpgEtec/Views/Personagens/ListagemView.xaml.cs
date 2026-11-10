using AppRpgEtec.ViewModels.Personagens;

namespace AppRpgEtec.Views.Personagens;

public partial class ListagemView : ContentPage
{
    private ListagemPersonagemViewModel viewModel;
    public ListagemView()
	{
		InitializeComponent();
        viewModel = new ListagemPersonagemViewModel();
        BindingContext = viewModel;
        Title = "Lista de Personagens";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.ObterPersonagens();
    }
}