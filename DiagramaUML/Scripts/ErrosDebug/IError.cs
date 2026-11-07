using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagramaUML.Scripts.ErrosDebug
{
    public interface IError
    {
        public void Erro();
        public void Erro(string texto);
    }
}
