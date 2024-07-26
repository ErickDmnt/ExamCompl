using ErickRualesComplementario.Modelos;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace ErickRualesComplementario.Views;

public partial class Login : ContentPage
{
    private const string Url = "http://192.168.100.2/Examen/estudiante_loginws.php";
    private readonly HttpClient cliente = new HttpClient();

    public Login()
	{
		InitializeComponent();
        
	}


    private async void btnIngresarQR_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vQrViewer());
    }

    private async void btnIniciarSesion_Clicked(object sender, EventArgs e)
    {
        // Obtener el usuario y la contrase�a ingresados
        string usuario = txtUsuario.Text;
        string contrasena = txtContra.Text;

        // Validar los campos
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            await DisplayAlert("Error", "Por favor, ingrese usuario y contrase�a.", "OK");
            return;
        }

        // Crear el modelo de autenticaci�n
        var loginModel = new
        {
            usuario = usuario,
            contrasena = contrasena
        };

        // Serializar el modelo a JSON
        var json = JsonConvert.SerializeObject(loginModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            // Enviar la solicitud POST al servidor
            HttpResponseMessage response = await cliente.PostAsync($"{Url}/login.php", content);

            // Leer la respuesta del servidor
            var responseContent = await response.Content.ReadAsStringAsync();

            // Verificar si la respuesta es exitosa
            if (response.IsSuccessStatusCode)
            {
                // Deserializar la respuesta para verificar el estado
                bool loginSuccess = bool.Parse(responseContent);

                if (loginSuccess)
                {
                    // Redirigir a la p�gina principal o cualquier otra acci�n despu�s del inicio de sesi�n exitoso
                    await Navigation.PushAsync(new vPrincipal());
                }
                else
                {
                    await DisplayAlert("Error", "Usuario o contrase�a incorrectos.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Error al conectar con el servidor. Int�ntelo m�s tarde.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurri� un error: {ex.Message}", "OK");
        }
    }


}

