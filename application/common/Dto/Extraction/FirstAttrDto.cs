using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace Application.Common.Dto.Extraction
{
    public class FirstProductAttributeInfo
    {
        public StorageInstructions? StorageInstructions { get; set; }
        public UsageInstructions? UsageInstructions { get; set; }
        public CookingInstructions? CookingInstructions { get; set; }
        public InformationInstructions? InformationInstructions { get; set; }
        public List<Dictionary<string, string>>? LabelingInfo { get; set; }
        public List<Dictionary<string, string>>? LabelingInfoAnalysis { get; set; }
        public AllergenInfo? AllergenInfo { get; set; }
        public HeaderInfo? HeaderInfo { get; set; }
        public List<Dictionary<string, string>>? BaseCertifierClaims { get; set; }
        public List<Dictionary<string, string>>? IngredientInfo { get; set; }
        public MarketingInfo? MarketingInfo { get; set; }
        public List<AddressData>? AddressData { get; set; }
        public SupplyChainInfo? SupplyChainInfo { get; set; }
        public List<Dictionary<string, string>>? AttributeInfo { get; set; }
    }

    public class StorageInstructions
    {
        [JsonProperty("storage instructions")]
        public List<string>? Instructions { get; set; }
    }

    public class UsageInstructions
    {
        [JsonProperty("usage instructions")]
        public List<string>? Instructions { get; set; }
    }

    public class CookingInstructions
    {
        public List<Recipe>? Recipes { get; set; }
        [JsonProperty("all other text or paragraph about cooking info")]
        public List<string>? AdditionalInfo { get; set; }
    }

    public class Recipe
    {
        [JsonProperty("recipe name")]
        public string? Name { get; set; }
        [JsonProperty("recipe ingredient list")]
        public List<string>? Ingredients { get; set; }
        [JsonProperty("cooking steps")]
        public List<string>? Steps { get; set; }
    }

    public class InformationInstructions
    {
        [JsonProperty("information instructions")]
        public List<string>? Instructions { get; set; }
    }

    public class AllergenInfo
    {
        [JsonProperty("allergens contain")]
        public AllergenContain? AllergensContain { get; set; }
        [JsonProperty("allergens on equipments or in facility")]
        public AllergenFacility? AllergensFacility { get; set; }
        [JsonProperty("allergens product info state not contain")]
        public AllergenNotContain? AllergensNotContain { get; set; }
        [JsonProperty("allergen information statements")]
        public List<string>? InformationStatements { get; set; }
    }

    public class HeaderInfo
    {
        [JsonProperty("product info")]
        public ProductBasicInfo? ProductInfo { get; set; }
        [JsonProperty("product size")]
        public ProductSize? Size { get; set; }
    }

    public class MarketingInfo
    {
        public List<Website>? Websites { get; set; }
    }

    public class Website
    {
        [JsonProperty("website link")]
        public string? Link { get; set; }
    }

    public class AddressData
    {
        [JsonProperty("address info text")]
        public string? AddressText { get; set; }
    }

    public class SupplyChainInfo
    {
        [JsonProperty("address and phone number info")]
        public List<AddressInfo>? AddressAndPhoneInfo { get; set; }
        [JsonProperty("country info")]
        public CountryInfo? CountryInfo { get; set; }
    }


    public class AddressInfo
    {
        [JsonProperty("prefix address")]
        public string? PrefixAddress { get; set; }
        [JsonProperty("address type")]
        public string? AddressType { get; set; }
        [JsonProperty("full address statement")]
        public string? FullAddressStatement { get; set; }
        [JsonProperty("company name")]
        public string? CompanyName { get; set; }
        [JsonProperty("street number")]
        public string? StreetNumber { get; set; }
        [JsonProperty("street name")]
        public string? StreetName { get; set; }
        [JsonProperty("city")]
        public string? City { get; set; }
        [JsonProperty("state")]
        public string? State { get; set; }
        [JsonProperty("zipCode")]
        public string? ZipCode { get; set; }
        [JsonProperty("phone number")]
        public string? PhoneNumber { get; set; }
    }

    public class CountryInfo
    {
        [JsonProperty("statement indicate from which nation product was made in")]
        public List<string>? MadeInStatements { get; set; }
        [JsonProperty("country of origin from made in statement")]
        public List<string>? CountriesOfOrigin { get; set; }
    }

    // Additional nested classes
    public class AllergenContain
    {
        [JsonProperty("all statements about allergens product contain")]
        public List<string>? Statements { get; set; }
        [JsonProperty("allergens contain statement break-down list")]
        public List<string>? BreakdownList { get; set; }
    }

    public class AllergenFacility
    {
        [JsonProperty("all statements about allergens on manufacturing equipments or from facility")]
        public List<string>? Statements { get; set; }
        [JsonProperty("allergens list from manufacturing equipments or from facility")]
        public List<string>? AllergensList { get; set; }
        [JsonProperty("allergens list not present in facility")]
        public List<string>? NotPresentInFacilityList { get; set; }
    }

    public class AllergenNotContain
    {
        [JsonProperty("exact all texts or statements on images about allergens that product does not contain")]
        public List<string>? Statements { get; set; }
        [JsonProperty("allergens product does not contain break-down list")]
        public List<string>? BreakdownList { get; set; }
    }

    public class ProductBasicInfo
    {
        [JsonProperty("product name")]
        public string? ProductName { get; set; }
        [JsonProperty("company name")]
        public string? CompanyName { get; set; }
        [JsonProperty("brand name")]
        public string? BrandName { get; set; }
    }

    public class ProductSize
    {
        [JsonPropertyName("full statement about product size")]
        public string? FullStatement { get; set; }
        [JsonProperty("primary size")]
        public string? PrimarySize { get; set; }
        [JsonProperty("secondary size")]
        public string? SecondarySize { get; set; }
        [JsonProperty("third size")]
        public string? ThirdSize { get; set; }
        [JsonProperty("count")]
        public string? Count { get; set; }
        [JsonProperty("count uom")]
        public string? CountUom { get; set; }
    }
}