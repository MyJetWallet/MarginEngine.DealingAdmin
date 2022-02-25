using DealingAdmin.Abstractions.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DealingAdmin.Abstractions
{
    public interface ISwapProfileUploadService
    {
        Task<List<SwapProfileModel>> ParseSwapItemsFromExcelFile(IBrowserFile file, string profileId = "");
    }
}