namespace LabScratch
{
    public partial class Form1 : Form
    {
        Graph[] graphs;
        Dictionary<string, int> variables;
        public Form1()
        {
            InitializeComponent();
            variables = new Dictionary<string, int>();
            graphs = new Graph[100];
            for (int i = 0; i < graphs.Length; i++)
                graphs[i] = new Graph();
            contextMenuStrip1.Items.Add("Check all variables");
            listBox1.ContextMenuStrip = contextMenuStrip1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (graphs[(int)numericUpDown1.Value].SelectedNodeId > -1)
                showSelectedNodeInfo();
            else
                hideSelectedNodeInfo();
                pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Graph graph = graphs[(int)numericUpDown1.Value];
            int? nodeId = graph.CheckNodeOnPos(e.Location);
            if (e.Button == MouseButtons.Left)
            {
                if (nodeId.HasValue)
                {
                    if (graph.SelectedNodeId != nodeId.Value)
                    {
                        graph.SelectedNodeId = nodeId.Value;
                        showSelectedNodeInfo();
                    }
                    else
                    {
                        graph.SelectedNodeId = -1;
                        hideSelectedNodeInfo();
                    }
                    pictureBox1.Invalidate();
                }
                else
                {
                    if (variables.Count > 1)
                    {
                        if (graph.checkCollision(e.Location))
                        {
                            CreateNodeForm form2 = new CreateNodeForm(variables);
                            if (form2.ShowDialog() == DialogResult.OK)
                            {
                                bool p = graph.AddNode(new Node(graph.GetAvailableId(), form2.nodeType, form2.res, e.Location));
                                if (p)
                                    pictureBox1.Invalidate();
                                else
                                    MessageBox.Show("You reached the limit of 100 nodes", "Node creating error");
                            }
                        }
                    }
                    else
                        MessageBox.Show("Create at least 2 variables!", "Node creating error");
                }
            }
        }

        private void showSelectedNodeInfo()
        {
            panel1.Visible = true;
            Node node = graphs[(int)numericUpDown1.Value].Nodes[graphs[(int)numericUpDown1.Value].SelectedNodeId];
            textBox1.Text = node.Id.ToString();
            textBox2.Text = node.Type.ToString();
            textBox3.Text = node.Operation;
            if(node.NextId > -1)
            {
                label4.Visible = true;
                textBox4.Visible = true;
                textBox4.Text = node.NextId.ToString();
            }
            else
            {
                label4.Visible = false;
                textBox4.Visible = false;
            }
            if (node.FalseId > -1)
            {
                label5.Visible = true;
                textBox5.Visible = true;
                textBox5.Text = node.FalseId.ToString();
            }
            else
            {
                label5.Visible = false;
                textBox5.Visible = false;
            }
        }

        private void hideSelectedNodeInfo()
        {
            panel1.Visible = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int lineWidth = 2;
            Graph graph = graphs[(int)numericUpDown1.Value];
            Pen edgePen = new Pen(Color.Black, lineWidth);
            foreach (var node in graph.Nodes.Values)
            {
                Point from = node.Position;
                if (node.NextId != -1)
                {
                    Point to = graph.Nodes[node.NextId].Position;
                    e.Graphics.DrawLine(edgePen, from, to);
                    DrawArrow(e.Graphics, from, to);
                }
                if (node.FalseId != -1)
                {
                    Point to = graph.Nodes[node.FalseId].Position;
                    e.Graphics.DrawLine(edgePen, from, to);
                    DrawArrow(e.Graphics, from, to);
                }
            }
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            foreach (int key in graph.Nodes.Keys)
            {
                Node n = graph.Nodes[key];
                Pen pen = new Pen(Color.Black, lineWidth);
                if (key == graph.SelectedNodeId)
                    pen.DashPattern = new float[] { 1.5f, 1.5f };
                SolidBrush brush = new SolidBrush(Color.Black);
                Rectangle rect = new Rectangle((int)(n.Position.X - n.Rad), (int)(n.Position.Y - n.Rad), n.Rad * 2, n.Rad * 2);
                e.Graphics.FillEllipse(Brushes.White, rect);
                e.Graphics.DrawEllipse(pen, rect);
                e.Graphics.DrawString((key).ToString(), new Font("Arial", 10), brush, rect, format);
            }
        }

        private void DrawArrow(Graphics g, Point from, Point to)
        {
            const float arrowAngle = 45;
            const float arrowLength = 10;

            float angle = (float)Math.Atan2(to.Y - from.Y, to.X - from.X);

            PointF arrowPoint1 = new PointF(to.X - arrowLength * (float)Math.Cos(angle - arrowAngle),
                                            to.Y - arrowLength * (float)Math.Sin(angle - arrowAngle));
            PointF arrowPoint2 = new PointF(to.X - arrowLength * (float)Math.Cos(angle + arrowAngle),
                                            to.Y - arrowLength * (float)Math.Sin(angle + arrowAngle));

            g.DrawLine(new Pen(Color.Black), to, arrowPoint1);
            g.DrawLine(new Pen(Color.Black), to, arrowPoint2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateVariableForm form2 = new CreateVariableForm(variables);
            if (form2.ShowDialog() == DialogResult.OK)
            {
                string res = form2.res;
                if (!res.Equals(""))
                {
                    string[] str = res.Split('=');
                    variables.Add(str[0], Convert.ToInt32(str[1]));
                    updateVariablesList();
                }
            }
        }

        private void updateVariablesList()
        {
            if (listBox1.Items.Count > 0)
                listBox1.Items.Clear();
            if (variables.Count > 0)
                foreach (var item in variables)
                    listBox1.Items.Add(item.Key + "=" + item.Value);
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(listBox1, e.Location);
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
