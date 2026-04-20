using SwiftMT103ApiTask.Models;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace SwiftMT103ApiTask.Services.MT103
{
    // The base class which is responsible for parsing and extracting data from MT103 messages
    public class MT103Parser
    {

        /// <summary>
        /// Extracts data from a MT103 message
        /// </summary>
        /// <param name="message">The plain text from the uploaded file</param>
        /// <returns>A complete MT103Message object ready to be saved in the db</returns>
        public static MT103Message ParseMessage(string message)
        {
            if(string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message can not be empty.");
            }

            var match = Regex.Match(message, @"\{4:\s*\n(.*?)\n-\}", RegexOptions.Singleline);

            if (!match.Success)
            {
                match = Regex.Match(message, @"\{4:(.*?)-\}", RegexOptions.Singleline);
            }

            if (!match.Success)
            {
                throw new Exception("Could not find Block 4 with transaction data");
            }

            string block4 = match.Groups[1].Value;

            Dictionary<string, string> fields = ParseBlock4Fields(block4);

            MT103Message mt103Message = MapFieldsToMessage(fields);

            return mt103Message;
        }

        /// <summary>
        /// Parses and extracts data from the 4th block of the Swift MT103 message
        /// </summary>
        /// <param name="block">String content of the 4th block</param>
        /// <returns>Dictionary tag:value e.g referenceNumber: 01010...</returns>
        public static Dictionary<string, string> ParseBlock4Fields(string block)
        {
            string[] lines = block.Split(new[] { '\n', '\r' });

            Dictionary<string, string> fields = new Dictionary<string, string>();

            string currentTag = null;
            string currentValue = null;

            foreach(string line in lines)
            {
                string trimmed = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                if(trimmed.StartsWith(":"))
                {
                    var fieldMatch = Regex.Match(trimmed, @"^:([^:]+):(.*)$");

                    if(fieldMatch.Success)
                    {
                        // New field

                        if(currentTag != null)
                        {
                            // Save previous field
                            fields[currentTag] = currentValue;
                        }

                        currentTag = fieldMatch.Groups[1].Value;
                        currentValue = fieldMatch.Groups[2].Value;
                    }

                }
                else
                {
                    if(currentTag != null)
                    {
                        currentValue += "\n" + trimmed;
                    }
                }
            }

            if (currentTag != null)
            {
                fields[currentTag] = currentValue;
            }

            return fields;
        }

        /// <summary>
        /// Maps values from the dictionary to a MT103Message object
        /// </summary>
        /// <param name="fields">Dictionary of block4 fields</param>
        /// <returns>MT103Message object</returns>
        private static MT103Message MapFieldsToMessage(Dictionary<string, string> fields)
        {
            MT103Message message = new MT103Message();
            foreach(var field in fields)
            {
                switch(field.Key)
                {
                    case "20": message.ReferenceNumber = field.Value; break;
                    case "23B": message.BankOperationCode = field.Value; break;
                    case "32A": 
                        if(field.Value.Length >= 9)
                        {
                            // Example value: 160217EUR540,00 -> 160221 = date, EUR = currency, 540,0 = amount
                            string dateStr = field.Value.Substring(0, 6);
                            int year = 2000 + int.Parse(dateStr.Substring(0, 2));
                            int month = int.Parse(dateStr.Substring(2, 2));
                            int day = int.Parse(dateStr.Substring(4, 2));

                            message.ValueDate = new DateTime(year, month, day);

                            message.Currency = field.Value.Substring(6, 3);

                            message.InterbankSettled = decimal.Parse(field.Value.Substring(9).Replace(",", "."));
                        }
                        break;
                    case "50A":
                    case "50F":
                    case "50K":
                        message.OrderingCustomer = field.Value;
                        break;
                    case "57A":
                    case "57B":
                    case "57C":
                    case "57D":
                        message.AccountWithInstitution = field.Value;
                        break;

                    case "59":
                    case "59A":
                        message.Recepient = field.Value;
                        break;

                    case "70":
                        message.RemittanceInformation = field.Value;
                        break;

                    case "71A":
                        message.DetailsOfCharge = field.Value;
                        break;

                    default:
                        break;
                }
            }

            return message;
        }
    }
}
