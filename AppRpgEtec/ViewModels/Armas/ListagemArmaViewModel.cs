
using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Armas
{
    public class ListagemArmaViewModel : BaseViewModel
    {
        private ArmaService aService;

        public ObservableCollection<Arma> Armas { get; set; }

        public ICommand NovoArmaCommand { get; }
        public ICommand RemoverArmaCommand { get; }

        public ListagemArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);

            Armas = new ObservableCollection<Arma>();

            _ = ObterArmas();

            NovoArmaCommand = new Command(async () =>
            {
                await ExibirCadastroArma();
            });

            RemoverArmaCommand = new Command<Arma>(async (Arma a) =>
            {
                await RemoverArma(a);
            });
        }

        public async Task ObterArmas()
        {
            try
            {
                Armas = await aService.GetArmasAsync();
                OnPropertyChanged(nameof(Armas));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlertAsync(
                    "Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ExibirCadastroArma()
        {
            try
            {
                await Shell.Current.GoToAsync("cadArmaView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlertAsync(
                    "Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        private Arma armaSelecionada;

        public Arma ArmaSelecionada
        {
            get { return armaSelecionada; }
            set
            {
                if (value != null)
                {
                    armaSelecionada = value;

                    Shell.Current.GoToAsync(
                        $"cadArmaView?id={armaSelecionada.Id}");
                }
            }
        }

        public async Task RemoverArma(Arma a)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlertAsync(
                    "Confirmação",
                    $"Confirma a remoção de {a.Nome}?",
                    "Sim", "Não"))
                {
                    await aService.DeleteArmaAsync(a.Id);

                    await Application.Current.MainPage.DisplayAlertAsync(
                        "Mensagem", "Arma removida com sucesso!", "Ok");

                    _ = ObterArmas();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlertAsync(
                    "Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}