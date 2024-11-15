using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyApi.Application.Common.Utils {
    public class SectionA  {

    }

     public class SectionB  {

    }
    public class FirstAttributeData  {
        public SectionA? SectionAData { get; set; }
        public SectionB? SectionBData { get; set; }
    }

    public class FirstAttributeParserUtils {

        public static FirstAttributeData ParseFirstAttribute(string markdownResponse) {
            return new FirstAttributeData() {} ;
        }
    }
}