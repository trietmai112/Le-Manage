using mtv_management_leave.Attributes;
using System;
using System.Web.Mvc;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class ModelMetadataExtendsion
    {
        public static string GetPlaceHolder(this ModelMetadata metadata)
        {
            var result = metadata.PropertyName;
            var prop = metadata.ContainerType.GetProperty(metadata.PropertyName);
            var attribute = Attribute.GetCustomAttribute(prop, typeof(PlaceHolderAttribute));
            if (attribute == null) return "";
            return (attribute as PlaceHolderAttribute).Text;
        }
    }
}
