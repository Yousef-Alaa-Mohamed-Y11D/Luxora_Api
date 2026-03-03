using Luxora_api.Models.DTO;

namespace Luxora_api.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
              new VillaDTO {Id =1,Name ="Pool View" ,Occupancy =1 , Sqft=43},
                new VillaDTO {Id =2,Name = "Beach View" ,Occupancy =2 , Sqft=32}
            };
    }
}
