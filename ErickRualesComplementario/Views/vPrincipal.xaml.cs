using ErickRualesComplementario.Modelos;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace ErickRualesComplementario.Views;

public partial class vPrincipal : ContentPage
{

    private const string Url = "http://192.168.100.2/Examen/estudiantews.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Modelos.Estudiantes> est;


    public vPrincipal()
	{
		InitializeComponent();
        Mostrar();
	}

    public async void Mostrar()
    {
        var content = await cliente.GetStringAsync(Url);
        List<Modelos.Estudiantes> mostrar = JsonConvert.DeserializeObject<List<Modelos.Estudiantes>>(content);
        est = new ObservableCollection<Modelos.Estudiantes>(mostrar);
        ListaEstudiantes.ItemsSource = est;
    }

    private async void btnInsertar_Clicked(object sender, EventArgs e)
    {
        try
        {
            using (HttpClient cliente = new HttpClient())
            {
                var param = new Dictionary<string, string>
            {
                { "nombre", txtNombre.Text },
                { "apellido", txtApellido.Text },
                { "curso", txtCurso.Text },
                { "paralelo", txtParalelo.Text },
                { "nota_final", txtNotafinal.Text }
            };

                var content = new FormUrlEncodedContent(param);
                HttpResponseMessage response = await cliente.PostAsync("http://192.168.100.2/Examen/estudiantews.php", content);

                response.EnsureSuccessStatusCode();

                // Navegar a la nueva página solo si la respuesta fue exitosa
                await Navigation.PushAsync(new vPrincipal());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("alerta", ex.Message, "OK");
        }
    }

    private void Limpiar()
    {
        txtCod_estudiante.Text = string.Empty;
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtCurso.Text = string.Empty;
        txtParalelo.Text = string.Empty;
        txtNotafinal.Text = string.Empty;
    }

    private void btnListar_Clicked(object sender, EventArgs e)
    {
        Mostrar();
        Limpiar();
    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {

        try
        {
            using (HttpClient cliente = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
            {
                // Construir la URL con el ID del estudiante a eliminar
                string url = $"http://192.168.100.2/Examen/estudiantews.php?cod_estudiante={txtCod_estudiante.Text}";

                // Enviar la solicitud DELETE
                HttpResponseMessage response = await cliente.DeleteAsync(url);

                // Verificar si la solicitud fue exitosa
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response from server: {responseString}");

                // Navegar a la nueva página solo si la respuesta fue exitosa
                await Navigation.PushAsync(new vPrincipal());
            }
        }
        catch (HttpRequestException httpEx)
        {
            await DisplayAlert("Error de HTTP", $"Mensaje: {httpEx.Message}", "OK");
        }
        catch (TaskCanceledException ex) when (ex.CancellationToken == default)
        {
            // Manejo específico para el tiempo de espera agotado
            await DisplayAlert("Alerta", "La solicitud se canceló debido a que se agotó el tiempo de espera configurado.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Alerta", ex.Message, "OK");
        }
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        if (selectedEstudiante == null)
        {
            await DisplayAlert("Error", "No se ha seleccionado ningún estudiante", "OK");
            return;
        }

        try
        {
            using (HttpClient cliente = new HttpClient())
            {
                var param = new Dictionary<string, string>
            {
                { "cod_estudiante", txtCod_estudiante.Text },
                { "nombre", txtNombre.Text },
                { "apellido", txtApellido.Text },
                { "curso", txtCurso.Text },
                { "paralelo", txtParalelo.Text },
                { "nota_final", txtNotafinal.Text }
            };

                var content = new FormUrlEncodedContent(param);
                HttpResponseMessage response = await cliente.PutAsync("http://192.168.100.2/Examen/estudiantews.php", content);

                response.EnsureSuccessStatusCode();

                await DisplayAlert("Éxito", "Estudiante actualizado correctamente", "OK");

                // Recargar la lista de estudiantes
                await Navigation.PushAsync(new vPrincipal());
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
    private Estudiantes selectedEstudiante;
    private void listaEstudiantes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            selectedEstudiante = e.SelectedItem as Estudiantes;

            // Enviar los datos a los Entry correspondientes
            txtCod_estudiante.Text = selectedEstudiante.cod_estudiante.ToString();
            txtNombre.Text = selectedEstudiante.nombre;
            txtApellido.Text = selectedEstudiante.apellido;
            txtCurso.Text = selectedEstudiante.curso;
            txtParalelo.Text = selectedEstudiante.paralelo;
            txtNotafinal.Text = selectedEstudiante.nota_final.ToString();

            // Deseleccionar el ítem
            ListaEstudiantes.SelectedItem = null;
        }
    }
}