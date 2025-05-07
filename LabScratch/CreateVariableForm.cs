namespace LabScratch
{
    public partial class CreateVariableForm : Form
    {
        Dictionary<string, int> variables;
        public string res;
        public CreateVariableForm(Dictionary<string, int> variables)
        {
            InitializeComponent();
            res = "";
            numericUpDown1.Minimum = int.MinValue;
            numericUpDown1.Maximum = int.MaxValue;
            this.variables = variables;
            foreach (var variable in variables)
                listBox1.Items.Add(variable.Key + " = " + variable.Value);
            contextMenuStrip1.Items.Add("Check all variables");
            listBox1.ContextMenuStrip = contextMenuStrip1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && !variables.ContainsKey(textBox1.Text))
            {
                res = textBox1.Text + "=" + numericUpDown1.Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show("Variable with this name already exists!", "Variable name error");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)8)
                return;

            if (!Char.IsLetterOrDigit(e.KeyChar) && !e.KeyChar.Equals('_'))
            {
                e.Handled = true;
                return;
            }

            if (textBox1.Text.Length == 0 && !Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            else if (Char.IsDigit(e.KeyChar))
                return;

            if (!Char.IsLetter(e.KeyChar) || !IsEnglishLetter(e.KeyChar))
                e.Handled = true;
        }

        private bool IsEnglishLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            Form viewForm = new Form();
            ListBox listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.Items.AddRange(listBox1.Items.Cast<object>().ToArray());
            viewForm.Controls.Add(listBox);
            viewForm.StartPosition = FormStartPosition.CenterScreen;
            viewForm.Font = new Font(viewForm.Font.FontFamily, 12);
            viewForm.ShowDialog();
        }
    }
}
