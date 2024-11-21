namespace Application.Common.Dto.Extraction
{
    public class SecondAttributeProductInfo
    {
        public List<Dictionary<string, string>>? SugarClaim { get; set; }
        public List<Dictionary<string, string>>? FatClaim { get; set; }
        public List<Dictionary<string, string>>? ProcessClaim { get; set; }
        public List<Dictionary<string, string>>? CalorieClaim { get; set; }
        public List<Dictionary<string, string>>? SaltClaim { get; set; }
        public List<Dictionary<string, string>>? FirstExtraClaim { get; set; }
        public List<Dictionary<string, string>>? SecondExtraClaim { get; set; }
        public List<Dictionary<string, string>>? ThirdExtraClaim { get; set; }

    }
}