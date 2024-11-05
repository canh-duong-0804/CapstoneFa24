using BusinessObject.Dto.FoodDiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto.MainDashBoardMobile
{
    public class MainDashResponseDTO
    {
       //public MainDashBoardMobileForMemberResponseDTO mainDashBoardInfo {  get; set; }   
        public FoodDiaryResponseDTO foodDiaryInforMember {  get; set; }
        public  List<FoodDiaryForMealResponseDTO> foodDiaryForMealBreakfast {  get; set; }
        public  List<FoodDiaryForMealResponseDTO> foodDiaryForMealLunch {  get; set; }
        public  List<FoodDiaryForMealResponseDTO> foodDiaryForMealDinner {  get; set; }
        public  List<FoodDiaryForMealResponseDTO> foodDiaryForMealSnack {  get; set; }
       
    }
}
