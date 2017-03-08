using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audity.Models;

namespace Audity.Interfaces
{
    interface IReceiptService
    {
        Task AddOrUpdateReceiptAsync(Receipt response);
        Task DeleteReceiptAsync(Receipt response);
        Task<IEnumerable<Receipt>> GetReceiptsAsync();
        Task<IEnumerable<Receipt>> GetReiciptAsync(string reiciptId);
        
    }
}
