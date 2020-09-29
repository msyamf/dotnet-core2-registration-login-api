using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Helpers.Validation
{

    public class CustomValidation
    {
        public static string validate(IDictionary<string, string> param_values, IDictionary<string, string> rules)
        {
            
            foreach (KeyValuePair<string, string> param in param_values)
            {
                
                string val = param.Value;
                string ruleText = rules[param.Key].Trim();
                string[] ruleContent = ruleText.Split("|");
                bool is_required = false;
                string msg = "";

                // check if required
                if (ruleText.Contains("required"))
                    is_required = true;

                // break validation if empty and not required
                if (!is_required && string.IsNullOrEmpty(val))
                    break;

                // continue to
                if (is_required && string.IsNullOrEmpty(val))
                    return param.Key + " value is required.";

                foreach (string rule in ruleContent)
                {
                    msg = checkRule(rule, param.Key, val);
                    if (msg != "")
                        return msg;
                }
            }

            return "";
        }

        static string checkRule(string rule=null, string key=null, string val=null)
        {
            try
            {
                if (rule == "email")
                { 
                    var foo = new EmailAddressAttribute(); 
                    return (!foo.IsValid(val)) ?  key + " value is not a email." : "";
                }

                if (rule == "is_number")
                {
                    if (!checkIsNumber(val))
                        return key + " value is not a number.";
                    
                    return "";
                }

                if (rule == "is_bool")
                {
                    if (!checkIsBoolean(val))
                        return key + " value is not boolean.";
                    
                    return "";
                }

                if (rule.Contains("intMax"))
                {
                    string[] ruleDetil = rule.Split(":");
                    int intMax = Convert.ToInt32(ruleDetil[1]);
                    if (Convert.ToInt32(val) > intMax)
                        return key + " value cann't be more than " + intMax + ".";
                    
                    return "";
                }

                if (rule.Contains("intMin"))
                {
                    string[] ruleDetil = rule.Split(":");
                    int intMin = Convert.ToInt32(ruleDetil[1]);
                    if (Convert.ToInt32(val) < intMin)
                        return key + " value cann't be less than " + intMin + ".";
                    
                    return "";
                }

                if (rule.Contains("charMax"))
                {
                    string[] ruleDetil = rule.Split(":");
                    int charMax = Convert.ToInt32(ruleDetil[1]);
                    if (val.Length > charMax)
                        return key + " length cann't be more than " + charMax + ".";
                    
                    return "";
                }

                if (rule.Contains("charMin"))
                {
                    string[] ruleDetil = rule.Split(":");
                    int charMin = Convert.ToInt32(ruleDetil[1]);
                    if (val.Length < charMin)
                        return key + " length cann't be less than " + charMin + ".";
                    
                    return "";
                }

                if (rule.Contains("in"))
                {
                    string[] ruleDetil = rule.Split(":");
                    string[] inValue = ruleDetil[1].Trim().Split(",");
                    if (!Array.Exists(inValue, ele => ele == val))
                        return key + " only must contain " + ruleDetil[1] + ".";
                    
                    return "";
                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Custom validation failed.";
            }
            
        }

        public static bool checkIsNumber(string val=null)
        {
            var isNumeric = int.TryParse(val, out int n);
            if (!isNumeric)
                return false;

            return true;
        }

        public static bool checkIsBoolean(string val=null)
        {
            Boolean parsedValue;
            if (!Boolean.TryParse(val, out parsedValue))
                return false;

            return true;
        }
    }
}