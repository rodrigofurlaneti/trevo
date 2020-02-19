using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class JsonResultViewModel<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; } = "";
        public string ProcessingTime { get; set; }
        public T Object { get; set; }
        public JsonResultViewModel() { }
    }
}
