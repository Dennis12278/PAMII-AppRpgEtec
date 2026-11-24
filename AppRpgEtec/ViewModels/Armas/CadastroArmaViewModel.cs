using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Armas
{
    [QueryProperty(nameof(Id), "id")]
    public class CadastroArmaViewModel : BaseViewModel
    {
        private ArmaService aService;
        private PersonagemService pService;

        public CadastroArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);

            aService = new ArmaService(token);
            pService = new PersonagemService(token);

            Personagens = new ObservableCollection<Personagem>();

            _ = ObterPersonagens();

            SalvarCommand = new Command(SalvarArma);
        }

        public ICommand SalvarCommand { get; set; }

        #region Atributos_Propriedades

        private int id;
        private string nome;
        private int dano;
        private int personagemId;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));

                if (id != 0)
                    _ = ObterArma();
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged(nameof(Nome));
            }
        }

        public int Dano
        {
            get => dano;
            set
            {
                dano = value;
                OnPropertyChanged(nameof(Dano));
            }
        }

        public int PersonagemId
        {
            get => personagemId;
            set
            {
                personagemId = value;
                OnPropertyChanged(nameof(PersonagemId));
            }
        }

        private Personagem personagemSelecionado;

        public Personagem PersonagemSelecionado
        {
            get { return personagemSelecionado; }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    OnPropertyChanged(nameof(PersonagemSelecionado));
                }
            }
        }

        public ObservableCollection<Personagem> Personagens { get; set; }

        #endregion

        #region Metodos

        public async Task ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops", ex.Message, "Ok");
            }
        }

        public async Task ObterArma()
        {
            try
            {
                Arma arma = await aService.GetArmaAsync(Id);

                Nome = arma.Nome;
                Dano = arma.Dano;
                PersonagemId = arma.PersonagemId;

                await ObterPersonagens();

                PersonagemSelecionado =
                    Personagens.FirstOrDefault(p => p.Id == arma.PersonagemId);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops", ex.Message, "Ok");
            }
        }

        public async void SalvarArma()
        {
            try
            {
                Arma model = new Arma()
                {
                    Id = this.Id,
                    Nome = this.Nome,
                    Dano = this.Dano,
                    PersonagemId = this.PersonagemSelecionado.Id
                };

                if (model.Id == 0)
                    await aService.PostArmaAsync(model);
                else
                    await aService.PutArmaAsync(model);

                await Application.Current.MainPage.DisplayAlert(
                    "Mensagem", "Dados salvos com sucesso", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops!", ex.Message, "Ok");
            }
        }
        public async void CarregarArma()
        {
            try
            {
                Arma a = await aService.GetArmaAsync(
                    int.Parse(armaSelecionadaId));

                this.Nome = a.Nome;
                this.Dano = a.Dano;
                this.Id = a.Id;
                this.PersonagemId = a.PersonagemId;

                await ObterPersonagens();

                PersonagemSelecionado = Personagens
                    .FirstOrDefault(p => p.Id == a.PersonagemId);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Ops", ex.Message, "Ok");
            }
        }
        private string armaSelecionadaId;

        public string ArmaSelecionadaId
        {
            set
            {
                if (value != null)
                {
                    armaSelecionadaId = value;
                    CarregarArma();
                }
            }
        }

        #endregion
    }
}