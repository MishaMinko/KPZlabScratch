using System.Drawing.Drawing2D;
using System.Numerics;

namespace LabScratch
{
    public partial class Form1 : Form
    {
        Graph[] graphs;
        Dictionary<string, int> variables;
        bool isMoved;
        Point prevPos;
        public Form1()
        {
            InitializeComponent();
            isMoved = false;
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

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
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
            else if (e.Button == MouseButtons.Middle)
            {
                if (graph.SelectedNodeId > -1)
                {
                    isMoved = true;
                    prevPos = e.Location;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (nodeId.HasValue)
                {
                    if (graph.SelectedNodeId > -1 && graph.SelectedNodeId != nodeId.Value)
                    {
                        Node selectedNode = graph.Nodes[graph.SelectedNodeId];
                        if (selectedNode.Type != NodeType.Condition)
                        {
                            if (selectedNode.NextId == nodeId.Value)
                                selectedNode.NextId = -1;
                            else
                                selectedNode.NextId = nodeId.Value;
                        }
                        else
                        {
                            if (selectedNode.NextId == -1 && selectedNode.FalseId != nodeId.Value)
                                selectedNode.NextId = nodeId.Value;
                            else if (selectedNode.FalseId == -1 && selectedNode.NextId != nodeId.Value)
                                selectedNode.FalseId = nodeId.Value;
                            else
                            {
                                selectedNode.NextId = nodeId.Value;
                                selectedNode.FalseId = -1;
                            }
                        }
                        showSelectedNodeInfo();
                        pictureBox1.Invalidate();
                    }
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
            if (node.NextId > -1)
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
            foreach (Node node in graph.Nodes.Values)
            {
                Point from = node.Position;
                if (node.NextId != -1)
                {
                    Point to = graph.Nodes[node.NextId].Position;
                    drawLineAndArrow(lineWidth, e.Graphics, from, to);
                }
                if (node.FalseId != -1)
                {
                    Point to = graph.Nodes[node.FalseId].Position;
                    drawLineAndArrow(lineWidth, e.Graphics, from, to);
                }
            }

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            foreach (Node node in graph.Nodes.Values)
            {
                Pen pen = new Pen(Color.Black, lineWidth);
                if (node.Id == graph.SelectedNodeId)
                    pen.DashPattern = new float[] { 1.5f, 1.5f };
                SolidBrush brush = new SolidBrush(Color.Black);
                Rectangle rect = new Rectangle((int)(node.Position.X - node.Rad), (int)(node.Position.Y - node.Rad), node.Rad * 2, node.Rad * 2);
                e.Graphics.FillEllipse(Brushes.White, rect);
                e.Graphics.DrawEllipse(pen, rect);
                e.Graphics.DrawString(node.Id.ToString(), new Font("Arial", 10), brush, rect, format);
            }
        }

        private void drawLineAndArrow(int lineWidth, Graphics g, Point from, Point to)
        {
            Pen connectPen = new Pen(Color.Black, lineWidth);
            connectPen.CustomEndCap = new AdjustableArrowCap(lineWidth * 3, lineWidth * 3);
            int shortenBy = 10;
            float dx = to.X - from.X;
            float dy = to.Y - from.Y;
            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length > shortenBy)
            {
                float factor = (float)(length - shortenBy) / (float)length;
                Point shortenedTo = new Point((int)(from.X + dx * factor), (int)(from.Y + dy * factor));
                g.DrawLine(connectPen, from, shortenedTo);
            }
            else
                g.DrawLine(connectPen, from, to);
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

        private void CorrectMove()
        {
            Node n = graphs[(int)numericUpDown1.Value].Nodes[graphs[(int)numericUpDown1.Value].SelectedNodeId];
            Point newPos = n.Position;
            if (newPos.X < 0)
                newPos.X = 0;
            else if (newPos.X > pictureBox1.Size.Width)
                newPos.X = pictureBox1.Size.Width;
            if (newPos.Y < 0)
                newPos.Y = 0;
            else if (newPos.Y > pictureBox1.Size.Height)
                newPos.Y = pictureBox1.Size.Height;
            n.Position = newPos;
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                if (isMoved)
                {
                    isMoved = false;
                    if (graphs[(int)numericUpDown1.Value].SelectedNodeId > -1)
                        CorrectMove();
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMoved)
            {
                if (graphs[(int)numericUpDown1.Value].SelectedNodeId > -1)
                {
                    Point vector = new Point(e.Location.X - prevPos.X, e.Location.Y - prevPos.Y);
                    prevPos = e.Location;
                    Node n = graphs[(int)numericUpDown1.Value].Nodes[graphs[(int)numericUpDown1.Value].SelectedNodeId];
                    vector.X += n.Position.X;
                    vector.Y += n.Position.Y;
                    n.Position = vector;
                    pictureBox1.Invalidate();
                }
                else
                    isMoved = false;
            }
        }
    }
}
