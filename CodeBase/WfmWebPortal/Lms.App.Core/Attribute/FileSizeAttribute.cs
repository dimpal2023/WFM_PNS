using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Wfm.App.Core.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class FileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        public int? MaxBytes { get; set; }

        public FileSizeAttribute(int maxBytes) : base("Please upload a supported file.")
        {
            MaxBytes = maxBytes;
            if (MaxBytes.HasValue)
            {
                ErrorMessage = "Please upload a file of less than " + (MaxBytes.Value/1024) + " K.";
                // 150K upload limit test
            }
        }

        public override bool IsValid(object value)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            if (file != null)
            {
                bool result = true;

                if (MaxBytes.HasValue)
                {
                    result &= (file.ContentLength < MaxBytes.Value);
                }

                return result;
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "filesize",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            rule.ValidationParameters["maxbytes"] = MaxBytes;
            yield return rule;
        }
    }
}