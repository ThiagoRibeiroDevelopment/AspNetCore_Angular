using QuickBuy.Dominio.Contratos;
using QuickBuy.Dominio.Entidades;
using System.Linq;

namespace QuickBuy.Repositorio.Repositorios
{
    public class UsuarioRepositorio : BaseRepositorio<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio()
        {
        }

        public Usuario Obter(string email, string senha)
        {
            throw new System.NotImplementedException();
        }

        public Usuario Obter(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}
