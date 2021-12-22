using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseWorker.Models
{
    internal interface IInteractable : IEntity
    {
        void Interact();
    }
}
