using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcClient.State.Navigators
{
    public interface IRenavigator
    {
        void Renavigate();
    }
}
