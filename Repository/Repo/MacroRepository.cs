using BusinessObject.Dto.Nutrition;
using DataAccess;
using Repository.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repo
{
    public class MacroRepository : IMacroRepository
    {
       public Task<MacroNutrientDto> GetMacroNutrientsByDate(int memberId, DateTime date) => MacroDAO.Instance.GetMacroNutrientsByDate(memberId, date);
    }
}
