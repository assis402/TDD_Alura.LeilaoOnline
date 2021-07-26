using System.Linq;
using Alura.LeilaoOnline.Core;
using Xunit;

namespace Alura.LeilaoOnline.Tests
{
    public class LeilaoRecebeOferta
    {
        [Fact]
        public void NaoPermiteNovosLancesDadoLeilaoFinalizado()
        {
            //Arrange
            var leilao = new Leilao("Van Gogh");
            var fulano = new Interessada("Fulano", leilao);

            leilao.RecebeLance(fulano, 800);
            leilao.RecebeLance(fulano, 900);
            leilao.TerminaPregao();
        
            //Act - método sob teste
            leilao.RecebeLance(fulano, 1000);

            //Assert
            var valorEsperado = 2;
            var valorObtido = leilao.Lances.Count(); 

            Assert.Equal(valorEsperado, valorObtido);
        }
    }
}