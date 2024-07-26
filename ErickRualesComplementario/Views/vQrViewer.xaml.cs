using IronBarCode;

namespace ErickRualesComplementario.Views;

public partial class vQrViewer : ContentPage
{
	public vQrViewer()
	{
		InitializeComponent();
	}

    private async void ImageSelect_Clicked(object sender, EventArgs e)
    {
        var images = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = "Pick image",
            FileTypes = FilePickerFileType.Images
        });
        var imageSource = images.FullPath.ToString();
        barcodeImage.Source = imageSource;
        var result = BarcodeReader.Read(imageSource).First().Text;
        outputText.Text = result;
    }

    private async void copyText_Clicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(outputText.Text);
        await DisplayAlert(" ", "Texto copiado!", "OK");
    }
}