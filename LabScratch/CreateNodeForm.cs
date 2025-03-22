using System.Windows.Forms;

namespace LabScratch
{
    public partial class CreateNodeForm : Form
    {
        Dictionary<string, int> variables;
        public CreateNodeForm(Dictionary<string, int> variables)
        {
            InitializeComponent();
            numericUpDown1.Minimum = int.MinValue;
            numericUpDown1.Maximum = int.MaxValue;
            this.variables = variables;
            foreach (var variable in variables)
            {
                string str = variable.Key + " = " + variable.Value;
                listBox1.Items.Add(str);
                comboBox3.Items.Add(str);
                comboBox4.Items.Add(str);
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
                comboBox2.Items.AddRange(new string[] { "V1=V2", "V=C" });
            else if (comboBox1.SelectedIndex == 1)
                comboBox2.Items.AddRange(new string[] { "INPUT V", "PRINT V" });
            else
                comboBox2.Items.AddRange(new string[] { "V==C", "V<C" });
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

        }
    }
}
