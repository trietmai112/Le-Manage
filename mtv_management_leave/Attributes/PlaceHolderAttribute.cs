using System;

namespace mtv_management_leave.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PlaceHolderAttribute: Attribute
    {
        public string Text { get; private set; }
        public PlaceHolderAttribute(string text)
        {
            Text = text;
        }
    }
}
