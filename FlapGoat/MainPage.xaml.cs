namespace FlapGoat;

public partial class MainPage : ContentPage
{
	const int gravidade = 5;
	const int tempoEntreFrames = 25;
	bool estaMorto = true;

	public MainPage()
	{
		InitializeComponent();
	}
	void AplicaGravidade()
	{
		imgPersonagem.TranslationY += gravidade;
		
	}


	async Task Desenha()
	{
		while (!estaMorto)
		{
			AplicaGravidade();
			await Task.Delay(tempoEntreFrames);
		}
	}

	void OnGameOverCliked(object s, TappedEventArgs e)
	{
		frameGameOver.IsVisible = false;
		Inicializar();
		Desenha();
	}
	void Inicializar()
	{
		estaMorto = false;
		imgPersonagem.TranslationY = 0;
	}



}

