using BusinessObject.Models;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
	public class CompanyInfoRepository : ICompanyInfoRepository
	{
		public Task<CompanyInfo> AddOrUpdateCompanyInfoAsync(CompanyInfo companyInfo) =>
		   CompanyInfoDAO.Instance.AddOrUpdateCompanyInfoAsync(companyInfo);

		public Task<bool> DeleteCompanyInfoAsync() =>
			CompanyInfoDAO.Instance.DeleteCompanyInfoAsync();

		public Task<CompanyInfo?> GetCompanyInfoAsync() =>
			CompanyInfoDAO.Instance.GetCompanyInfoAsync();
	}
}
