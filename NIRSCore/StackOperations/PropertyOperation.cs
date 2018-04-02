using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRSCore.StackOperations
{
    public sealed class PropertyOperation<T> : IOperation
    {
        public bool IsUnDone { get; private set; }

        public bool IsDone { get; private set; }

        public string Name { get; private set; }

        
        /*public PropertyOperation(User, string nameProperty, T value)
        {
            _prevProperty = prevProperty;
            _postProperty = postProperty;
            IsUnDone = true;
            IsDone = false;
        }*/



        public void Done()
        {
            //_propertyObject = _postProperty;
        }

        public void UnDone()
        {
            throw new NotImplementedException();
        }
    }
}
