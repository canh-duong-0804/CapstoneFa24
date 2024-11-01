using System.Threading.Tasks;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class CompanyInfoDAO
	{
		private static CompanyInfoDAO instance = null;
		private static readonly object instanceLock = new object();

		public static CompanyInfoDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new CompanyInfoDAO();
					}
					return instance;
				}
			}
		}

		// Retrieve the single company information record
		public async Task<CompanyInfo?> GetCompanyInfoAsync()
		{
			using (var context = new HealthTrackingDBContext())
			{
				return await context.CompanyInfos.FirstOrDefaultAsync();
			}
		}

		// Add or update the company information record
		public async Task<CompanyInfo> AddOrUpdateCompanyInfoAsync(CompanyInfo companyInfo)
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					var existingCompanyInfo = await context.CompanyInfos.FirstOrDefaultAsync();

					if (existingCompanyInfo != null)
					{
						// Update the existing record
						context.Entry(existingCompanyInfo).CurrentValues.SetValues(companyInfo);
					}
					else
					{
						// Add a new record
						context.CompanyInfos.Add(companyInfo);
					}
					await context.SaveChangesAsync();
					return companyInfo;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error in AddOrUpdateCompanyInfoAsync: " + ex.Message);
			}
		}

		// Delete the single company information record
		public async Task<bool> DeleteCompanyInfoAsync()
		{
			try
			{
				using (var context = new HealthTrackingDBContext())
				{
					var companyInfo = await context.CompanyInfos.FirstOrDefaultAsync();

					if (companyInfo != null)
					{
						context.CompanyInfos.Remove(companyInfo);
						await context.SaveChangesAsync();
						return true;
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error in DeleteCompanyInfoAsync: " + ex.Message);
			}
		}
	}
}
