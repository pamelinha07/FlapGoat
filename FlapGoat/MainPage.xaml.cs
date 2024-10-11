namespace FlapGoat;

public partial class MainPage : ContentPage
{
	const int gravidade = 3;
	const int tempoEntreFrames = 25;
	bool estaMorto = true;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 15;

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
			GerenciaCanos();
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
	protected override void OnSizeAllocated(double w, double h)
	{
      base.OnSizeAllocated(w, h);
	  larguraJanela = w;
	  alturaJanela = h;
	}
	void GerenciaCanos()
	{
		imgTroncoCima.TranslationX -= velocidade;
		imgTroncoBaixo.TranslationX -= velocidade;
		if (imgTroncoBaixo.TranslationX<-larguraJanela)
		{
			imgTroncoBaixo.TranslationX = 0 ;
			imgTroncoCima.TranslationX = 0 ;

		}
	}




}

