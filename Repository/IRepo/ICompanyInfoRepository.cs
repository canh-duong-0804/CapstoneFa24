using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepo
{
    public interface ICompanyInfoRepository
    {
		Task<CompanyInfo?> GetCompanyInfoAsync();

		// Add or update the company information
		Task<CompanyInfo> AddOrUpdateCompanyInfoAsync(CompanyInfo companyInfo);

		// Delete the company information
		Task<bool> DeleteCompanyInfoAsync();
	}
}
