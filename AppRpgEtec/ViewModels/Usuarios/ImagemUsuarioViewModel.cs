using AppRpgEtec.Services.Usuarios;
using AppRpgEtec.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using Azure.Storage.Blobs;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Usuarios
{
    // Herdando de BaseViewModel (1). Será necessário o using para AppRpgEtec.Services.Usuarios. Em (2) 
    // faremos a declaração da classe de serviço para uso da API. Em (3) colocaremos os dados de 
    // armazenamento do azure e em (4) será o construtor da classe.
    public class ImagemUsuarioViewModel : BaseViewModel
    {
        // Em (2) faremos a declaração da classe de serviço para uso da API.
        private UsuarioService uService;

        // Em (3) colocaremos os dados de armazenamento do azure
        private static string conexaoAzureStorage = "COLE a chave de acesso da conta de armazenamento";
        private static string container = "arquivos"; // //nome do container criado

        // Em (4) será o construtor da classe.
        public ImagemUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);

            // 2
            FotografarCommand = new Command(Fotografar);
            SalvarImagemCommand = new Command(SalvarImagemAzure);
            AbrirGaleriaCommand = new Command(AbrirGaleria);
        }

        // 3. Adicione o atributo/propriedade fonteImagem para armazenar a imagem para exibir na view e Foto para 
        // atribuir ao futuro objeto do tipo Usuário.
        private ImageSource fonteImagem;
        public ImageSource FonteImagem
        {
            get { return fonteImagem; }
            set
            {
                fonteImagem = value;
                OnPropertyChanged();
            }
        }

        private byte[] foto; // //CTRL + R, E
        public byte[] Foto
        {
            get => foto;
            set
            {
                foto = value;
                OnPropertyChanged();
            }
        }


        public async void Fotografar()
        {
            try
            {

                if (MediaPicker.Default.IsCaptureSupported)
                {

                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        using (Stream sourceStream = await photo.OpenReadAsync())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                await sourceStream.CopyToAsync(ms);


                                Foto = ms.ToArray();

                                FonteImagem = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }


        // 15. Volte à viewModel ImagemUsuarioViewModel.cs e faça uma cópia do método fotografar e altere 
        // basicamente as partes sinalizadas abaixo. Como pode perceber vamos utilizar a mesma Biblioteca 
        // MediaPicker, alterando apenas o método que desta vez será para escolher uma imagem da galeria.
        public async void AbrirGaleria()
        {
            try
            {
                // //Chamada para a galeria do dispositivo. Fica aguardando usuário escolher a foto da galeria.
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null)
                {
                    using (Stream sourceStream = await photo.OpenReadAsync()) // //Leitura dos bytes da foto para Stream
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await sourceStream.CopyToAsync(ms);

                            Foto = ms.ToArray();

                            FonteImagem = ImageSource.FromStream(() => new MemoryStream(ms.ToArray()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }


        public async void SalvarImagemAzure()
        {
            try
            {
                Usuario u = new Usuario();
                u.Foto = foto;
                u.Id = Preferences.Get("UsuarioId", 0);

                string fileName = $"{u.Id}.jpg";


                var blobClient = new BlobClient(conexaoAzureStorage, container, fileName);

                if (blobClient.Exists())
                    blobClient.Delete();

                using (var stream = new MemoryStream(u.Foto))
                {
                    blobClient.Upload(stream);
                }

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");
                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }


        public ICommand FotografarCommand { get; }
        public ICommand SalvarImagemCommand { get; }
        // 16. Declare abaixo do construtor um ICommand chamado AbrirGaleriaCommand (1) e faça a vinculação dele com o método AbrirGaleria na última linha do construtor (2)
        public ICommand AbrirGaleriaCommand { get; }
    }
}
