using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audity.Interfaces;
using Audity.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;

namespace Audity.Services
{
    class AzureReceiptService : IReceiptService
    {

        private MobileServiceClient client;
        private string azureendpoint = "http://mobileappsprueba.azurewebsites.net/";
        IMobileServiceSyncTable<Receipt> reiciptsTable;

        private async Task Initialize()
        {
            if (client != null)
                return;
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<Receipt>();

            client = new MobileServiceClient(azureendpoint);
            await client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            reiciptsTable = client.GetSyncTable<Receipt>();

            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    await client.SyncContext.PushAsync();
                    await reiciptsTable.PullAsync(
                        "allReceipts", reiciptsTable.CreateQuery());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Got exception: {0}", ex.Message);
                }
            }
            

            //Ejemplo de como hacer otro query para un grupo de una tabla especifica
            //try
            //{

            //    await responseTable.PullAsync("syncResponses" + questionId,
            //                                      responseTable.Where(
            //                                      r => r.SurveyQuestionId == questionId));

            //}
            //catch (Exception ex)
            //{
            //    // TODO: handle error
            //    Debug.WriteLine("Got exception: {0}", ex.Message);
            //}




        }

        public async Task<IEnumerable<Receipt>> GetReceiptsAsync()
        {
            await Initialize();
            return await reiciptsTable.ReadAsync();
            
        }

        public async Task<IEnumerable<Receipt>> GetReiciptAsync(string reiciptId)
        {
            await Initialize();
            return await reiciptsTable
            .Where(r => r.titulo == reiciptId)
            .OrderByDescending(r => r.UpdatedAt)
            .Take(100).ToEnumerableAsync();
            
        }

        public async Task AddOrUpdateReceiptAsync(Receipt response)
        {
            await Initialize();
            if (string.IsNullOrEmpty(response.Id))
            {
                await reiciptsTable.InsertAsync(response);
            }
            await reiciptsTable.UpdateAsync(response);

        }

        public Task DeleteReceiptAsync(Receipt response)
        {
            return reiciptsTable.DeleteAsync(response);
            
        }
    }
}
