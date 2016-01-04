using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AndroAdminTests
{
    public class TestHelper
    {
        public static string CheckForModelError(Controller controller, string property, string expectedErrorMessage)
        {
            string errorMessage = "";

            // Model should be invalid
            if (controller.ModelState.IsValid)
            {
                errorMessage = "Model is valid but should be invalid";
            }

            // Get the model state for the property we're checking
            ModelState modelState = controller.ModelState[property];

            if (errorMessage.Length == 0)
            {
                // There should be errors
                if (modelState.Errors == null)
                {
                    errorMessage = "No model errors.  Expected 1";
                }
            }

            if (errorMessage.Length == 0)
            {
                // There should be one error
                if (modelState.Errors.Count != 1)
                {
                    errorMessage = "Wrong number of model errors.  Expected 1 got " + modelState.Errors.Count.ToString();
                }
            }

            if (errorMessage.Length == 0)
            {
                // Check the error message is correct
                if (expectedErrorMessage != modelState.Errors[0].ErrorMessage)
                {
                    errorMessage = "Wrong error message.  Expected \"" + expectedErrorMessage + "\" got \"" + modelState.Errors[0].ErrorMessage + "\"";
                }
            }

            return errorMessage;
        }

        public static void ValidateModel(object model, Controller controller)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);

            foreach (ValidationResult validationResult in validationResults)
            {
                foreach (string memberName in validationResult.MemberNames)
                {
                    controller.ModelState.AddModelError(memberName, validationResult.ErrorMessage);
                }
            }
        }
    }
}
