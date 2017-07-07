namespace ShortestPath2
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mnuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuPutNode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPutEdge = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFindShortestPath = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLoadGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPutNode,
            this.mnuPutEdge,
            this.mnuFindShortestPath,
            this.mnuSaveGraph,
            this.mnuLoadGraph});
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(175, 136);
            this.mnuMain.Opened += new System.EventHandler(this.mnuMain_Opened);
            // 
            // mnuPutNode
            // 
            this.mnuPutNode.Name = "mnuPutNode";
            this.mnuPutNode.Size = new System.Drawing.Size(174, 22);
            this.mnuPutNode.Text = "Put Node";
            this.mnuPutNode.Click += new System.EventHandler(this.mnuPutNode_Click);
            // 
            // mnuPutEdge
            // 
            this.mnuPutEdge.Name = "mnuPutEdge";
            this.mnuPutEdge.Size = new System.Drawing.Size(174, 22);
            this.mnuPutEdge.Text = "Put Edge";
            this.mnuPutEdge.Click += new System.EventHandler(this.mnuPutEdge_Click);
            // 
            // mnuFindShortestPath
            // 
            this.mnuFindShortestPath.Name = "mnuFindShortestPath";
            this.mnuFindShortestPath.Size = new System.Drawing.Size(174, 22);
            this.mnuFindShortestPath.Text = "Find Shortest Path";
            this.mnuFindShortestPath.Click += new System.EventHandler(this.mnuFindShortestPath_Click);
            // 
            // mnuSaveGraph
            // 
            this.mnuSaveGraph.Name = "mnuSaveGraph";
            this.mnuSaveGraph.Size = new System.Drawing.Size(174, 22);
            this.mnuSaveGraph.Text = "Save Graph";
            this.mnuSaveGraph.Click += new System.EventHandler(this.mnuSaveGraph_Click);
            // 
            // mnuLoadGraph
            // 
            this.mnuLoadGraph.Name = "mnuLoadGraph";
            this.mnuLoadGraph.Size = new System.Drawing.Size(174, 22);
            this.mnuLoadGraph.Text = "Load Graph";
            this.mnuLoadGraph.Click += new System.EventHandler(this.mnuLoadGraph_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 766);
            this.ContextMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shortest path using Dijkstra";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.Activated += new System.EventHandler(this.frmMain_Activated);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.mnuMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuPutNode;
        private System.Windows.Forms.ToolStripMenuItem mnuPutEdge;
        private System.Windows.Forms.ToolStripMenuItem mnuFindShortestPath;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveGraph;
        private System.Windows.Forms.ToolStripMenuItem mnuLoadGraph;
    }
}

