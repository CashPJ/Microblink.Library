using Microblink.Extensions;
using System;

namespace Microblink.Common.MrzValidator
{
    public abstract class MrzValidator
    {
        private const int _letterMinusValue = 55;
        private readonly int[] _weightArray = { 7, 3, 1 };
        private int _weightArrayCurrentIndex = 0;
        
        protected const char _asterix = '<';        
        protected readonly string _mrzString;
        protected readonly string[] _mrzStringRows;

        public MrzValidator(string mrzString)
        {
            _mrzString = mrzString;
            _mrzStringRows = _mrzString.Split("\n");
        }

        /// <summary>
        /// Sum of calculated products for section (E.g. DateOfBirth)
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        protected int SumSectionProducts(string section)
        {
            ResetWeightArrayCurrentIndex();
            var sum = 0;
            var chars = section.ToCharArray();
            foreach (char c in chars)
            {
                var prod = GetCharProduct(c);
                sum += prod;
                NextWeightArrayIndex();
            }

            return sum;
        }

        /// <summary>
        /// Resets _weightArrayCurrentIndex value to zero
        /// </summary>
        protected void ResetWeightArrayCurrentIndex()
        {
            _weightArrayCurrentIndex = 0;
        }

        /// <summary>
        /// Loops _weightArrayCurrentIndex to the next position
        /// </summary>
        protected void NextWeightArrayIndex()
        {
            _weightArrayCurrentIndex++;

            if (_weightArrayCurrentIndex == _weightArray.Length)
                ResetWeightArrayCurrentIndex();
        }

        /// <summary>
        /// Calculates char weight acording current <see cref="WeightArrayCurrentIndex"/>
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected virtual int GetCharProduct(char c)
        {
            var value = GetCharValue(c);
            var weightValue = value * _weightArray[_weightArrayCurrentIndex];
            return weightValue;
        }

        /// <summary>
        /// Gets char MRZ integer value
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected virtual int GetCharValue(char c)
        {
            if (c == _asterix)
                return 0;

            if (char.IsLetter(c))
                return LetterCharToMrzIntValue(c);

            if (char.IsDigit(c))
                return int.Parse(c.ToString());

            throw new ArgumentException("Invalid MRZ char");
        }

        /// <summary>
        /// Gets char MRZ integer value
        /// </summary>
        /// <param name="singleCharacter"></param>
        /// <returns></returns>
        protected int GetCharValue(string singleCharacter)
        {
            return GetCharValue(singleCharacter.ToChar());
        }

        /// <summary>
        /// Converts letter char to MRZ integer value
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected virtual int LetterCharToMrzIntValue(char c)
        {
            return c - _letterMinusValue;
        }

        /// <summary>
        /// Validates if MRZ string is in right format
        /// </summary>
        /// <param name="mrzString"></param>
        /// <returns></returns>
        public abstract bool IsValidMrzString(string mrzString);

        /// <summary>
        /// Validates data in MRZ string
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValidMrz();
    }
}
