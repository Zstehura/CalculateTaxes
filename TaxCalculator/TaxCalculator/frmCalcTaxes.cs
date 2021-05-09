using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaxCalculator
{
    public partial class frmCalcTaxes : Form
    {
        public frmCalcTaxes()
        {
            InitializeComponent();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            //Validate data entered
            bool isValid = true;
            if (txtFirstName.Text == "" || txtLastName.Text == "")
            {
                MessageBox.Show("You must enter a first and last name.");
                isValid = false;
            }
            if (cboStatus.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a Filing Status.");
                isValid = false;
            }
            if (txtIncome.Text == "")
            {
                MessageBox.Show("You must enter your yearly income.");
                isValid = false;
            }
            try
            {
                if (Convert.ToDecimal(txtIncome.Text) < 0) throw new NegativeIncomeError();
            }
            catch (NegativeIncomeError err)
            {
                isValid = false;
                MessageBox.Show("Income must be a positive number.");
            }
            catch (Exception err)
            {
                isValid = false;
                MessageBox.Show("You must enter a valid number in for your taxable income.");
            }

            if(isValid)
            {
                // all tests passed, proceed to process data
                Income inc = new Income(txtFirstName.Text + " " + txtLastName.Text, cboStatus.Text, Convert.ToDouble(txtIncome.Text));
                string output = "Hello " + inc.Name + "!";
                string nl = Environment.NewLine;
                double dblNetInc = inc.GrossIncome - inc.TaxPaid;
                output += nl + "Tax statement as printed on " + DateTime.Now.ToLocalTime().ToString();
                output += nl + "This tax statement is for income earned during the 2020 tax year." + nl;
                output += nl + $"Filing as: {inc.FileAs}";
                output += nl + $"Taxable income:     {inc.GrossIncome.ToString("C"),12}";
                output += nl + $"Federal income tax: {inc.TaxPaid.ToString("C"),12}";
                output += nl + $"Net Income:         {dblNetInc.ToString("C"),12}";

                txtOutput.Text = output;
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = "";
            txtIncome.Text = "";
            txtLastName.Text = "";
            txtOutput.Text = "";
            cboStatus.SelectedIndex = -1;
        }

        private void txtIncome_Leave(object sender, EventArgs e)
        {
            try
            {
                txtIncome.Text = Convert.ToDecimal(txtIncome.Text).ToString("N2");
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void txtIncome_Click(object sender, EventArgs e)
        {
            txtIncome.Select(0, txtIncome.Text.Length);
        }
    }
}
