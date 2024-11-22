using System.Text.Json.Serialization;
using OneOf;

namespace Application.Common.Dto.Extraction
{
    public class FirstProductAttributeInfo
    {
        public StorageInstructions StorageInstructions { get; set; }
        public UsageInstructions UsageInstructions { get; set; }
        public CookingInstructions CookingInstructions { get; set; }
        public InformationInstructions InformationInstructions { get; set; }
        public List<Dictionary<string, string>> LabelingInfo { get; set; }
        public List<Dictionary<string, string>> LabelingInfoAnalysis { get; set; }
        public AllergenInfo AllergenInfo { get; set; }
        public HeaderInfo HeaderInfo { get; set; }
        public List<Dictionary<string, string>> BaseCertifierClaims { get; set; }
        public List<Dictionary<string, string>> IngredientInfo { get; set; }
        public MarketingInfo MarketingInfo { get; set; }
        public List<AddressData> AddressData { get; set; }
        public SupplyChainInfo? SupplyChainInfo { get; set; }
        public List<Dictionary<string, string>> AttributeInfo { get; set; }
    }

    // public class LabelingInfoAnalysisValue {
    //     public OneOf<string, string[]>? Value { get; set; }
    //     public LabelingInfoAnalysisValue(string value) => Value = value;
    //     public LabelingInfoAnalysisValue(string[] value) => Value = value;
    // } 

    public class StorageInstructions
    {
        [JsonPropertyName("storage instructions")]
        public List<string> Instructions { get; set; }
    }

    public class UsageInstructions
    {
        [JsonPropertyName("usage instructions")]
        public List<string> Instructions { get; set; }
    }

    public class CookingInstructions
    {
        public List<Recipe> Recipes { get; set; }
        [JsonPropertyName("all other text or paragraph about cooking info")]
        public List<string> AdditionalInfo { get; set; }
    }

    public class Recipe
    {
        [JsonPropertyName("recipe name")]
        public string Name { get; set; }
        [JsonPropertyName("recipe ingredient list")]
        public List<string> Ingredients { get; set; }
        [JsonPropertyName("cooking steps")]
        public List<string> Steps { get; set; }
    }

    public class InformationInstructions
    {
        [JsonPropertyName("information instructions")]
        public List<string> Instructions { get; set; }
    }

    public class AllergenInfo
    {
        [JsonPropertyName("allergens contain")]
        public AllergenContain AllergensContain { get; set; }
        [JsonPropertyName("allergens on equipments or in facility")]
        public AllergenFacility AllergensFacility { get; set; }
        [JsonPropertyName("allergens product info state not contain")]
        public AllergenNotContain AllergensNotContain { get; set; }
        [JsonPropertyName("allergen information statements")]
        public List<string> InformationStatements { get; set; }
    }

    public class HeaderInfo
    {
        [JsonPropertyName("product info")]
        public ProductBasicInfo ProductInfo { get; set; }
        [JsonPropertyName("product size")]
        public ProductSize Size { get; set; }
    }

    public class MarketingInfo
    {
        public List<Website> Websites { get; set; }
    }

    public class Website
    {
        [JsonPropertyName("website link")]
        public string Link { get; set; }
    }

    public class AddressData
    {
        // Add properties as needed
    }

    public class SupplyChainInfo
    {
        [JsonPropertyName("address and phone number info")]
        public List<AddressInfo>? AddressAndPhoneInfo { get; set; }
        [JsonPropertyName("country info")]
        public CountryInfo? CountryInfo { get; set; }
    }


    public class AddressInfo
    {
        [JsonPropertyName("prefix address")]
        public string PrefixAddress { get; set; }
        [JsonPropertyName("address type")]
        public string AddressType { get; set; }
        [JsonPropertyName("full address statement")]
        public string FullAddressStatement { get; set; }
        [JsonPropertyName("company name")]
        public string CompanyName { get; set; } 
        [JsonPropertyName("street number")]
        public string StreetNumber { get; set; }
        [JsonPropertyName("street name")]
        public string StreetName { get; set; }  
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
        [JsonPropertyName("zipCode")]
        public string ZipCode { get; set; }
        [JsonPropertyName("phone number")]
        public string PhoneNumber { get; set; }
    }   

    public class CountryInfo
    {
        [JsonPropertyName("statement indicate from which nation product was made in")]
        public List<string> MadeInStatements { get; set; }
        [JsonPropertyName("country of origin from made in statement")]
        public List<string> CountriesOfOrigin { get; set; }
    }

    // Additional nested classes
    public class AllergenContain
    {
        [JsonPropertyName("all statements about allergens product contain")]
        public List<string> Statements { get; set; }
        [JsonPropertyName("allergens contain statement break-down list")]
        public List<string> BreakdownList { get; set; }
    }

    public class AllergenFacility
    {
        [JsonPropertyName("all statements about allergens on manufacturing equipments or from facility")]
        public List<string> Statements { get; set; }
        [JsonPropertyName("allergens list from manufacturing equipments or from facility")]
        public List<string> AllergensList { get; set; }
        [JsonPropertyName("allergens list not present in facility")]
        public List<string> NotPresentList { get; set; }
    }

    public class AllergenNotContain
    {
        [JsonPropertyName("exact all texts or statements on images about allergens that product does not contain")]
        public List<string> Statements { get; set; }
        [JsonPropertyName("allergens product does not contain break-down list")]
        public List<string> BreakdownList { get; set; }
    }

    public class ProductBasicInfo
    {
        [JsonPropertyName("product name")]
        public string ProductName { get; set; }
        [JsonPropertyName("company name")]
        public string CompanyName { get; set; }
        [JsonPropertyName("brand name")]
        public string BrandName { get; set; }
    }

    public class ProductSize
    {
        [JsonPropertyName("full statement about product size")]
        public string FullStatement { get; set; }
        [JsonPropertyName("primary size")]
        public string PrimarySize { get; set; }
        [JsonPropertyName("secondary size")]
        public string SecondarySize { get; set; }
        [JsonPropertyName("third size")]
        public string? ThirdSize { get; set; }
        [JsonPropertyName("count")]
        public string? Count { get; set; }
        [JsonPropertyName("count uom")]
        public string? CountUom { get; set; }
    }
}