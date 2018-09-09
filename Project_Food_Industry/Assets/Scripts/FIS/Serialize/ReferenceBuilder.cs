using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Industry
{
    public class ReferenceBuilder
    {
        Dictionary<int, object> refs = new Dictionary<int, object>();

        public void AddReference(int id, object obj)
        {
            refs.Add(id, obj);
        }

        public object GetReference(int id)
        {
            return refs[id];
        }
    }

}
