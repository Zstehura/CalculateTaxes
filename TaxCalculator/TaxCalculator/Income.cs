using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator
{   
    public class NegativeIncomeError : System.Exception
    {
        public NegativeIncomeError() : base() { }
        public NegativeIncomeError(string msg) : base(msg) { }
        public NegativeIncomeError(string msg, System.Exception inner) : base(msg, inner) { }

        protected NegativeIncomeError(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    class Income
    {
        private double[] taxR =             { 0.1, 0.12, 0.22, 0.24, 0.32, 0.35, 0.37 };
        private double[,] brackCieling = {  { 9875, 40125, 85525, 163300, 207350, 518400},			//Single
											{ 19750, 80250, 171050, 326600, 414700, 622050},		//Married
											{ 9875, 40125, 85525, 163300, 207350, 311025},			//Separated
											{ 14100, 53700, 85500, 163300, 207350, 518400} };		//HoH
        private string strName;
        private string strFileAs;
        private double dGrossInc;
        private double dTaxPaid;
        private double dNetInc;

        public string Name
        {
            get { return strName; }
            set { strName = Name; }
        }
        public string FileAs
        {
            get { return strFileAs; }
            set { strFileAs = FileAs; }
        }
        public double GrossIncome
        {
            get { return dGrossInc; }
            set { dGrossInc = GrossIncome; }
        }
        public double TaxPaid
        {
            get { return dTaxPaid; }
        }
        public double NetIncome
        {
            get { return dNetInc; }
        }

        public Income()
        {
            strName = "[NoName]";
            strFileAs = "Single";
            dGrossInc = 20000;
        }
        public Income(string name, string status, double inc)
        {
            strName = name;
            strFileAs = status;
            dGrossInc = inc;

            CalculateVals();
        }

        private void CalculateVals()
        {
            int iBracket = 0;
            int iFiler = 0;
            double taxAdd = 0;

            if (strFileAs == "Joint Married or Widow")
            {
                // Married or qualifying widow
                iFiler = 1;
            }
            else if (strFileAs == "Separate Married")
            {
                // Married filing separately
                iFiler = 2;
            }
            else if (strFileAs == "Head of Household")
            {
                // Filing as Head of Household
                iFiler = 3;
            }
            else
            {
                // Single
                iFiler = 0;
            }

            while (dGrossInc > brackCieling[iFiler, iBracket])
            {

                if (iBracket == 0)
                {
                    taxAdd = taxR[iBracket] * brackCieling[iFiler, iBracket];
                }
                else
                {
                    taxAdd += taxR[iBracket] * (brackCieling[iFiler, iBracket] - brackCieling[iFiler, iBracket - 1]);
                }

                iBracket++;
                if (iBracket == 6) break;
            }

            if (iBracket == 0)
            {
                dTaxPaid = taxR[iBracket] * dGrossInc;
            }
            else
            {
                dTaxPaid = taxAdd + taxR[iBracket] * (dGrossInc - brackCieling[iFiler, iBracket - 1]);
            }

            dNetInc = dGrossInc - dTaxPaid;
        }
    }
}
