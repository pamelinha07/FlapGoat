using Plugin.Maui.Audio;

namespace FlapGoat;

public partial class MainPage : ContentPage
{
	const int gravidade = 3;
	const int tempoEntreFrames = 25;
	const int forcaPulo = 30;
	const int maxTempoPulando = 3; //frames
	const int aberturaMinima = 100;
	bool estaPulando = false;
	int tempoPulando = 0;
	bool estaMorto = true;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 10;
	int score = 0;

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
			if(estaPulando)
				AplicaPulo();
			else 
				AplicaGravidade();

			GerenciaCanos();

			if (VerificaColisao())
			{
				estaMorto = true;
				frameGameOver.IsVisible = true;
				labelGameOver.Text="Você passou \n por "+ score + " \n Troncos";
				break;
			}
			if (estaPulando)
			   AplicaPulo();
			else
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
		score = 0;
		imgTroncoCima.TranslationX = - larguraJanela;
		imgTroncoBaixo.TranslationX = - larguraJanela;
		imgPersonagem.TranslationX = 0;
		imgPersonagem.TranslationY = 0;
		score = 0;
		velocidade = 5;
		GerenciaCanos();
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
			 var alturaMax = -100;
			 var alturaMin = -imgTroncoBaixo.HeightRequest;
			 imgTroncoCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			 imgTroncoBaixo.TranslationY=imgTroncoCima.TranslationY+aberturaMinima+imgTroncoBaixo.HeightRequest;
			 score ++;
			 labelScore.Text = "Canos: " + score.ToString ("D3");
			 score++;
			 if (score % 2 == 0)
			 velocidade++;

		}
	}
	void AplicaPulo()
	{
		imgPersonagem.TranslationY -= forcaPulo;
		tempoPulando++ ;
		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}
	void OnGridCliked(object s, TappedEventArgs a)
	{
		estaPulando = true;
	}
	bool VerificaColisao()
	{
		if(!estaMorto)
		{	
			if(VerificaColisaoTeto () || VerificaColisaoChao () || VerificaColisaoCanoCima() ||  VerificaColisaoCanoBaixo())
			{
				return true;
			}
		}		
		return false;	
	}

	bool VerificaColisaoTeto()
	{
		var minY =- alturaJanela/2;
		if (imgPersonagem.TranslationY <= minY)
		    return true;
		else
		    return false;
	}
	 bool VerificaColisaoChao()
	 {
		var maxY = alturaJanela/2 - imgChao.HeightRequest;
		if (imgPersonagem.TranslationY >= maxY)
		    return true;
		else
		   return false;	
	 }

	 bool VerificaColisaoCanoCima()
	 {
		var posicaoHorizontalPardal = (larguraJanela - 50) - (imgPersonagem.WidthRequest / 2);
    var posicaoVerticalPardal   = (alturaJanela / 2) - (imgPersonagem.HeightRequest / 2) + imgPersonagem.TranslationY;

    if (
         posicaoHorizontalPardal >= Math.Abs(imgTroncoCima.TranslationX) - imgTroncoCima.WidthRequest &&
         posicaoHorizontalPardal <= Math.Abs(imgTroncoCima.TranslationX) + imgTroncoCima.WidthRequest &&
         posicaoVerticalPardal   <= imgTroncoCima.HeightRequest + imgTroncoCima.TranslationY
       )
      return true;
    else
      return false;
 	 }

 	bool VerificaColisaoCanoBaixo()
	{
		var posHCabrito = larguraJanela - 60 - (imgPersonagem.WidthRequest/2);
		var posVCabrito = (alturaJanela/2) + (imgPersonagem.HeightRequest/2) + imgPersonagem.TranslationY;
		var yMaxTronco = imgTroncoCima.HeightRequest + imgTroncoCima.TranslationY + aberturaMinima;
		if (posHCabrito >= Math.Abs(imgTroncoBaixo.TranslationX) - imgTroncoCima.WidthRequest && 
			posHCabrito <= Math.Abs(imgTroncoBaixo.TranslationX) + imgTroncoCima.WidthRequest &&
			posVCabrito >=yMaxTronco)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}