using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audity.Interfaces;
using Audity.Models;
using Microsoft.WindowsAzure.MobileServices;

namespace Audity.Services
{
    class AzureReceiptService : IReceiptService
    {

        private MobileServiceClient client;
        private string azureendpoint = "http://mobileappsprueba.azurewebsites.net/";
        IMobileServiceTable<Receipt> reiciptsTable;

        private void Initialize()
        {
            if (client != null)
                return;


            client = new MobileServiceClient(azureendpoint);
            reiciptsTable = client.GetTable<Receipt>();

        }

        public Task<IEnumerable<Receipt>> GetReceiptsAsync()
        {
            Initialize();
            return reiciptsTable.ReadAsync();
            
        }

        public async Task<IEnumerable<Receipt>> GetReiciptAsync(string reiciptId)
        {
            Initialize();
            return await reiciptsTable
            .Where(r => r.titulo == reiciptId)
            .OrderByDescending(r => r.UpdatedAt)
            .Take(100).ToEnumerableAsync();
            
        }

        public Task AddOrUpdateReceiptAsync(Receipt response)
        {
            Initialize();
            if (string.IsNullOrEmpty(response.Id))
            {
                return reiciptsTable.InsertAsync(response);
            }
            return reiciptsTable.UpdateAsync(response);

        }

        public Task DeleteReceiptAsync(Receipt response)
        {
            return reiciptsTable.DeleteAsync(response);
            
        }
    }
}
