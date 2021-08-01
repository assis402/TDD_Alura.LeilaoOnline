using System.Collections.Generic;
using System.Linq;

namespace Alura.LeilaoOnline.Core
{
    public enum EstadoLeilao 
    {
        LeilaoNaoIniciado = 0,
        LeilaoEmAndamento = 1,
        LeilaoFinalizado = 2
    }

    public class Leilao
    {
        private IList<Lance> _lances;
        private IModalidadeAvaliacao _avaliador;

        public IEnumerable<Lance> Lances => _lances;
        public string Peca { get; }
        public Lance Ganhador { get; private set; }
        public EstadoLeilao Estado { get; private set; }
        public double ValorDestino { get; }

        public Leilao(){
            Estado = EstadoLeilao.LeilaoNaoIniciado;
        }

        public Leilao(string peca, IModalidadeAvaliacao avaliador)
        {
            Peca = peca;
            _lances = new List<Lance>();
            _avaliador = avaliador;
       }

        public void RecebeLance(Interessada cliente, double valor)
        {
            if (Estado.Equals(EstadoLeilao.LeilaoEmAndamento))
                _lances.Add(new Lance(cliente, valor));
        }

        public void IniciaPregao()
        {if (ValorDestino > 0)
            {
                //modalidade oferta superior mais próxima
                Ganhador = Lances
                    .Where(l => l.Valor > ValorDestino)
                    .OrderBy(l => l.Valor)
                    .FirstOrDefault();
            }
            else 
            {
                //modalidade maior valor
                Ganhador = Lances
                    .DefaultIfEmpty(new Lance(null, 0))
                    .OrderBy(l => l.Valor)
                    .LastOrDefault();
            }
            Estado = EstadoLeilao.LeilaoEmAndamento;
        }

        public void TerminaPregao()
        {
            if (Estado != EstadoLeilao.LeilaoEmAndamento)
                throw new System.InvalidOperationException();

            Ganhador = _avaliador.Avalia(this);
            Estado = EstadoLeilao.LeilaoFinalizado;
        }
    }
}
