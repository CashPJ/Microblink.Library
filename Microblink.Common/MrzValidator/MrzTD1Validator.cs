using Microblink.Extensions;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microblink.Common.MrzValidator
{
    // <summary>
    /// MRZ TD1 validator
    /// <para>MRZ formats: https://en.wikipedia.org/wiki/Machine-readable_passport#Format</para>
    /// </summary>
    public class MrzTD1Validator : MrzValidator
    {
        #region Calculated fields
        private readonly string _documentNumberString;
        private readonly int _documentNumberWeight;
        private readonly string _documentNumberControlString;
        private readonly int _documentNumberControl;
        private readonly string _dateOfBirthString;
        private readonly int _dateOfBirthWeight;
        private readonly string _dateOfBirthControlString;
        private readonly int _dateOfBirthControl;
        private readonly string _expiryDateString;
        private readonly int _expiryDateWeight;
        private readonly string _expiryDateControlString;
        private readonly int _expiryDateControl;
        private readonly string _overallString;
        private readonly int _overallWeight;
        private readonly string _overallControlString;
        private readonly int _overallControl;
        #endregion

        /// <summary>
        /// Creates instance with TD1 raw string which should be validated
        /// </summary>
        /// <param name="mrzString"></param>
        public MrzTD1Validator(string mrzString) : base(mrzString)
        {
            //setting document number
            _documentNumberString = _mrzStringRows[0].Substring(5, 9);
            if (_documentNumberString.Contains(_asterix))
                _documentNumberString = _documentNumberString.Substring(0, _documentNumberString.IndexOf(_asterix));
            _documentNumberWeight = SumSectionProducts(_documentNumberString);

            //setting document number control
            _documentNumberControlString = _mrzStringRows[0].Substring(14, 1);
            _documentNumberControl = GetCharValue(_documentNumberControlString);

            //setting date of birth
            _dateOfBirthString = _mrzStringRows[1].Substring(0, 6);
            _dateOfBirthWeight = SumSectionProducts(_dateOfBirthString);

            //setting date of birth control
            _dateOfBirthControlString = _mrzStringRows[1].Substring(6, 1);
            _dateOfBirthControl = GetCharValue(_dateOfBirthControlString);

            //setting expiry date
            _expiryDateString = _mrzStringRows[1].Substring(8, 6);
            _expiryDateWeight = SumSectionProducts(_expiryDateString);

            //setting expiry date control
            _expiryDateControlString = _mrzStringRows[1].Substring(14, 1);
            _expiryDateControl = GetCharValue(_expiryDateControlString);

            //setting overall
            var capture1 = _mrzStringRows[0].Substring(5, _mrzStringRows[0].Length - 5);
            var capture2 = _mrzStringRows[1].Substring(0, 7);
            var capture3 = _mrzStringRows[1].Substring(8, 7);
            var capture4 = _mrzStringRows[1].Substring(18, 11);
            _overallString = $"{capture1}{capture2}{capture3}{capture4}".RemoveNewLines();
            _overallWeight = SumSectionProducts(_overallString);

            //setting overall control
            _overallControlString = _mrzStringRows[1].Substring(29, 1);
            _overallControl = GetCharValue(_overallControlString);
        }

        /// <summary>
        /// Validates data with control numbers in MRZ TD1 string
        /// </summary>
        /// <param name="mrzString"></param>
        /// <returns></returns>
        public override bool IsValidMrz()
        {
            var documentNumberValidation = _documentNumberWeight % 10 == _documentNumberControl;
            var dateOfBirthValidation = _dateOfBirthWeight % 10 == _dateOfBirthControl;
            var expiryDateValidation = _expiryDateWeight % 10 == _expiryDateControl;
            var overallValidation = _overallWeight % 10 == _overallControl;

            return documentNumberValidation && dateOfBirthValidation && expiryDateValidation && overallValidation;
        }

        /// <summary>
        /// Validates if MRZ TD1 string is in right format 
        /// </summary>
        /// <returns></returns>
        public override bool IsValidMrzString(string mrzString)
        {
            var singleLineMrzTd1String = mrzString.RemoveNewLines();

            //basic string length check
            if (string.IsNullOrEmpty(mrzString) || singleLineMrzTd1String.Length != 90)
                return false;

            //TD1 rows count check
            if (mrzString.TrimEnd("\n".ToCharArray()).Split("\n").Count() != 3)
                return false;

            //Allowed characters check
            Regex allowedChars = new Regex(@"^[A-Z0-9\\<]+$");
            return allowedChars.IsMatch(singleLineMrzTd1String);
        }
    }
}
