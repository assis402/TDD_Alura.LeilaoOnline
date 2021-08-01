using System;
using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoTerminaPregao
    {
        [Theory]
        [InlineData(1200, 1250, new double[] { 800, 1150, 1400, 1250 })]
        public void RetornaValorSuperiorMaisProximoDadoLeilaoNessaModalidade(double valorDestino, double valorEsperado, double[] ofertas)
        {
            IModalidadeAvaliacao modalidade = new OfertaSuperiorMaisProxima(valorDestino);
            var leilao = new Leilao("Van Gogh", modalidade);
            leilao.IniciaPregao();

            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            for (int i = 0; i < ofertas.Length; i++)
            {
                if((i%2) == 0){
                    leilao.RecebeLance(fulano, ofertas[i]);
                }
                else {
                    leilao.RecebeLance(maria, ofertas[i]);
                }
            }
        
            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Theory]
        [InlineData(1000, new double[] { 800,900,1000,990 })]
        [InlineData(1200, new double[] { 800,900,1000,1200 })]
        [InlineData(800, new double[] { 800 })]
        public void RetornaMaiorDadoLeilaoComPeloMenosUmLance(double valorEsperado, double[] ofertas)
        {
            //Arrange
            IModalidadeAvaliacao modalidade = new MaiorValor();
            var leilao = new Leilao("Van Gogh", modalidade);
            leilao.IniciaPregao();

            var fulano = new Interessada("Fulano", leilao);
            var maria = new Interessada("Maria", leilao);

            foreach(var valor in ofertas)
            {
                leilao.RecebeLance(fulano, valor);
            }
        
            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }

        [Fact]
        public void LancaInvalidOperationExceptionDaDoPregaoNaoIniciado()
        {
             //Arrange
            var leilao = new Leilao();
            
            //Assert
            Assert.Throws<System.InvalidOperationException>(
                //Act
                () => leilao.TerminaPregao()
            );
        }     

        [Fact]
        public void RetornaZeroDadoLeilaoSemLance()
        {
            //Arrange
            var modalidade = new MaiorValor();
            var leilao = new Leilao("Van", modalidade);
            leilao.IniciaPregao();
        
            //Act - método sob teste
            leilao.TerminaPregao();

            //Assert
            var valorEsperado = 0;
            var valorObtido = leilao.Ganhador.Valor;

            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}
