namespace LabScratch
{
    public partial class CreateNodeForm : Form
    {
        Dictionary<string, int> variables;
        public NodeType nodeType;
        public string res;
        public CreateNodeForm(Dictionary<string, int> variables)
        {
            InitializeComponent();
            nodeType = 0;
            res = "";
            numericUpDown1.Minimum = int.MinValue;
            numericUpDown1.Maximum = int.MaxValue;
            this.variables = variables;
            foreach (var variable in variables)
            {
                listBox1.Items.Add(variable.Key + " = " + variable.Value);
                comboBox3.Items.Add(variable.Key);
                comboBox4.Items.Add(variable.Key);
            }
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.SelectedIndex = 0;
                comboBox4.SelectedIndex = 0;
            }
            contextMenuStrip1.Items.Add("Check all variables");
            listBox1.ContextMenuStrip = contextMenuStrip1;
            comboBox1.Items.AddRange(new string[] { NodeType.Assignment.ToString(), NodeType.Console.ToString(), NodeType.Condition.ToString() });
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            numericUpDown1.Visible = false;
            if (comboBox2.Items.Count > 0)
                comboBox2.Items.Clear();
            if (comboBox1.SelectedIndex == 0)
            {
                nodeType = NodeType.Assignment;
                comboBox2.Items.AddRange(new string[] { "V1=V2", "V=C" });
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                nodeType = NodeType.Console;
                comboBox2.Items.AddRange(new string[] { "INPUT V", "PRINT V" });
            }
            else
            {
                nodeType = NodeType.Condition;
                comboBox2.Items.AddRange(new string[] { "V==C", "V<C" });
            }
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            numericUpDown1.Visible = false;

            if (comboBox1.SelectedIndex == 0)
            {
                label2.Visible = true;
                comboBox3.Visible = true;
                label2.Text = "||";
                if (comboBox2.SelectedIndex == 0)
                    comboBox4.Visible = true;
                else
                    numericUpDown1.Visible = true;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                label2.Visible = true;
                comboBox4.Visible = true;
                if (comboBox2.SelectedIndex == 0)
                    label2.Text = "INPUT ↓";
                else
                    label2.Text = "PRINT ↓";
            }
            else
            {
                comboBox3.Visible = true;
                label2.Visible = true;
                numericUpDown1.Visible = true;
                if (comboBox2.SelectedIndex == 0)
                    label2.Text = "↑==↓";
                else
                    label2.Text = "↑<↓";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "";

            if (nodeType == NodeType.Assignment)
            {
                str = comboBox3.SelectedItem + "=";
                if (comboBox2.SelectedIndex == 0)
                    str += comboBox4.SelectedItem;
                else
                    str += numericUpDown1.Value.ToString();
            }
            else if (nodeType == NodeType.Console)
            {
                if (comboBox2.SelectedIndex == 0)
                    str = "INPUT ";
                else
                    str = "PRINT ";
                str += comboBox4.SelectedItem;
            }
            else
            {
                str = comboBox3.SelectedItem.ToString();
                if (comboBox2.SelectedIndex == 0)
                    str += "==";
                else
                    str += "<";
                str += numericUpDown1.Value.ToString();
            }

            res = str;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
