using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class Reference<T> where T : ID
    {
        public int id { get; private set; }
        T _ref;

        public Reference(T obj)
        {
            id = obj.id;
            _ref = obj;
        }

        [JsonConstructor]
        public Reference(int id)
        {
            this.id = id;
        }

        public void SetReference(ReferenceBuilder builder)
        {
            _ref = (T)builder.GetReference(id);
        }

        public T Get()
        {
            return _ref;
        }
    }
}
