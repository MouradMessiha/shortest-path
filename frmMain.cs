using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShortestPath2
{
    public partial class frmMain : Form
    {
        private Bitmap mobjFormBitmap;
        private Graphics mobjBitmapGraphics;
        private int mintFormWidth;
        private int mintFormHeight;
        private Boolean mblnDoneOnce = false;
        private Boolean mblnMenuOpened = false;
        private int mintMode = 0;
        private List<clsNode> mlstNodes;
        private List<clsEdge> mlstEdges;
        private clsNode mobjEdgeStartNode;
        private clsNode mobjGraphStartNode;
        private List<clsEdge> mlstShortestPath;
        private bool mblnClosing = false;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            if (!mblnDoneOnce)
            {
                mblnDoneOnce = true;
                mintFormWidth = this.Width;
                mintFormHeight = this.Height;
                mobjFormBitmap = new Bitmap(mintFormWidth, mintFormHeight, this.CreateGraphics());
                mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap);

                mlstNodes = new List<clsNode>();
                mlstEdges = new List<clsEdge>();
                RefreshDisplay();
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                mintFormWidth = this.Width;
                mintFormHeight = this.Height;
                mobjFormBitmap = new Bitmap(mintFormWidth, mintFormHeight, this.CreateGraphics());
                mobjBitmapGraphics = Graphics.FromImage(mobjFormBitmap);
                RefreshDisplay();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do nothing
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(mobjFormBitmap, 0, 0);
        }

        private void RefreshDisplay()
        {
            Font objFont;
            mobjBitmapGraphics.FillRectangle(Brushes.White, 0, 0, mintFormWidth, mintFormHeight);

            if (!mblnMenuOpened)
            {
                objFont = new Font("MS Sans Serif", 10, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
                mobjBitmapGraphics.DrawString("Right click for menu", objFont, Brushes.Black,10,10);
            }
            
            foreach (clsEdge objEdge in mlstEdges)
            {
                mobjBitmapGraphics.DrawLine(new Pen(Brushes.DarkGray, (float)Math.Sqrt(objEdge.Speed)/2), objEdge.StartNode.X, objEdge.StartNode.Y, objEdge.EndNode.X, objEdge.EndNode.Y);
            }

            foreach (clsNode objNode in mlstNodes)
            {
                if (objNode == mobjEdgeStartNode )
                    mobjBitmapGraphics.FillEllipse(Brushes.Red, objNode.X - 5, objNode.Y - 5, 10, 10);
                else
                    if (objNode == mobjGraphStartNode)
                        mobjBitmapGraphics.FillEllipse(Brushes.Black, objNode.X - 5, objNode.Y - 5, 10, 10);
                    else
                        mobjBitmapGraphics.FillEllipse(Brushes.Green, objNode.X - 5, objNode.Y - 5, 10, 10);                    
            }

            this.Invalidate();
        }

        private void mnuMain_Opened(object sender, EventArgs e)
        {
            mblnMenuOpened = true;
        }

        private void mnuPutNode_Click(object sender, EventArgs e)
        {
            mintMode = 1;
        }

        private void mnuPutEdge_Click(object sender, EventArgs e)
        {
            mintMode = 2;
        }

        private void mnuFindShortestPath_Click(object sender, EventArgs e)
        {
            mintMode = 3;
        }

        private void mnuSaveGraph_Click(object sender, EventArgs e)
        {
            string strFileName = "";
            SaveFileDialog objDialog = new SaveFileDialog();
            objDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            objDialog.DefaultExt = ".gph"; 
            objDialog.Filter = "Graph file|*.gph";

            DialogResult result = objDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                strFileName = objDialog.FileName;
                Stream objStream = null;
                try
                {
                    IFormatter objFormatter = new BinaryFormatter();
                    objStream = new FileStream(strFileName, FileMode.Create, FileAccess.Write, FileShare.None);
                    objFormatter.Serialize(objStream, mlstNodes);
                    objFormatter.Serialize(objStream, mlstEdges);
                }
                catch
                {
                }
                finally
                {
                    if (objStream != null)
                        objStream.Close();
                }
            }
        }

        private void mnuLoadGraph_Click(object sender, EventArgs e)
        {
            string strFileName = "";
            OpenFileDialog objDialog = new OpenFileDialog();
            objDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            objDialog.DefaultExt = ".gph"; 
            objDialog.Filter = "Graph file|*.gph"; 

            DialogResult result = objDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                strFileName = objDialog.FileName;
                Stream objStream = null;
                try
                {
                    IFormatter objFormatter = new BinaryFormatter();
                    objStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    mlstNodes = (List<clsNode>)objFormatter.Deserialize(objStream);
                    mlstEdges = (List<clsEdge>)objFormatter.Deserialize(objStream);
                    RefreshDisplay();
                }
                catch
                {
                }
                finally
                {
                    if (objStream != null)
                        objStream.Close();
                }

            }
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            switch (mintMode)
            {
                case 1:
                    if (e.Button == MouseButtons.Left)
                    {
                        mlstNodes.Add(new clsNode(e.X, e.Y, mlstNodes.Count));
                        RefreshDisplay();
                    }                       
                    break;
                    
                case 2:
                    if (e.Button == MouseButtons.Left)
                        {
                        foreach (clsNode objNode in mlstNodes)
                            {
                                if (Math.Abs(e.X - objNode.X) + Math.Abs(e.Y - objNode.Y) < 20)
                                {
                                    if (mobjEdgeStartNode == null)
                                    {
                                        mobjEdgeStartNode = objNode;
                                    }
                                    else
                                    {
                                        Double dblSpeed = Convert.ToDouble(InputBox.Show("Enter speed in pixels/second", "Speed", "100").Text);
                                        Double dblCost = (Math.Sqrt(Math.Pow(mobjEdgeStartNode.X - objNode.X ,2)  + Math.Pow(mobjEdgeStartNode.Y - objNode.Y,2) )) / dblSpeed;
                                        mlstEdges.Add(new clsEdge(mobjEdgeStartNode, objNode, dblSpeed,dblCost));
                                        mobjEdgeStartNode = null;
                                    }
                                    RefreshDisplay();
                                    break;
                                }
                            }
                        }
                    break;

                case 3:
                    if (e.Button == MouseButtons.Left)
                        {
                        foreach (clsNode objNode in mlstNodes)
                            {
                                if (Math.Abs(e.X - objNode.X) + Math.Abs(e.Y - objNode.Y) < 20)
                                {
                                    if (mobjGraphStartNode == null)
                                    {
                                        mobjGraphStartNode = objNode;
                                        RefreshDisplay();
                                    }
                                    else
                                    {
                                        if (FindShortestPath(mobjGraphStartNode, objNode))
                                            AnimateShortestPath();
                                        else
                                        {
                                            MessageBox.Show("No Path between start and end nodes");
                                            mobjGraphStartNode = null;
                                            RefreshDisplay();
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    break;
            }
        }

        private void AnimateShortestPath()
        {
            Bitmap objSnapShotBitmap;
            bool blnFinished = false;
            double dblTime = 0;
            double dblEdgeStartTime;
            int intPathEdgeIndex;
            int intEdgeStartNodeIndex;
            int intEdgeEndNodeIndex;
            int intX1;
            int intY1;
            int intX2;
            int intY2;
            int intX;
            int intY;

            intEdgeStartNodeIndex = mobjGraphStartNode.Index;
            mobjGraphStartNode = null;
            RefreshDisplay();

            objSnapShotBitmap = new Bitmap(mobjFormBitmap);
            intPathEdgeIndex = mlstShortestPath.Count - 1;
            dblEdgeStartTime = 0;

            if (intEdgeStartNodeIndex == mlstShortestPath[intPathEdgeIndex].StartNode.Index)
            {
                intX1 = mlstShortestPath[intPathEdgeIndex].StartNode.X;
                intY1 = mlstShortestPath[intPathEdgeIndex].StartNode.Y;
                intX2 = mlstShortestPath[intPathEdgeIndex].EndNode.X;
                intY2 = mlstShortestPath[intPathEdgeIndex].EndNode.Y;
                intEdgeEndNodeIndex = mlstShortestPath[intPathEdgeIndex].EndNode.Index;
            }
            else
            {
                intX1 = mlstShortestPath[intPathEdgeIndex].EndNode.X;
                intY1 = mlstShortestPath[intPathEdgeIndex].EndNode.Y;
                intX2 = mlstShortestPath[intPathEdgeIndex].StartNode.X;
                intY2 = mlstShortestPath[intPathEdgeIndex].StartNode.Y;
                intEdgeEndNodeIndex = mlstShortestPath[intPathEdgeIndex].StartNode.Index;
            }
            
            while (!blnFinished && !mblnClosing)
            {
                intX = (int)((Double)intX1 + ((((Double)intX2 - (Double)intX1) * (dblTime - dblEdgeStartTime)) / mlstShortestPath[intPathEdgeIndex].Cost));
                intY = (int)((Double)intY1 + ((((Double)intY2 - (Double)intY1) * (dblTime - dblEdgeStartTime)) / mlstShortestPath[intPathEdgeIndex].Cost));

                mobjBitmapGraphics.DrawImage(objSnapShotBitmap, 0, 0);
                mobjBitmapGraphics.FillEllipse(Brushes.Black, intX - 10, intY - 10, 20, 20);
                this.Invalidate();
                Application.DoEvents();
                Thread.Sleep(10);

                dblTime += 0.01;

                while (dblEdgeStartTime + mlstShortestPath[intPathEdgeIndex].Cost < dblTime)
                {
                    dblEdgeStartTime += mlstShortestPath[intPathEdgeIndex].Cost;
                    if (intPathEdgeIndex > 0)
                    {
                        intPathEdgeIndex -= 1;
                        intEdgeStartNodeIndex = intEdgeEndNodeIndex;
                        if (intEdgeStartNodeIndex == mlstShortestPath[intPathEdgeIndex].StartNode.Index)
                        {
                            intX1 = mlstShortestPath[intPathEdgeIndex].StartNode.X;
                            intY1 = mlstShortestPath[intPathEdgeIndex].StartNode.Y;
                            intX2 = mlstShortestPath[intPathEdgeIndex].EndNode.X;
                            intY2 = mlstShortestPath[intPathEdgeIndex].EndNode.Y;
                            intEdgeEndNodeIndex = mlstShortestPath[intPathEdgeIndex].EndNode.Index;
                        }
                        else
                        {
                            intX1 = mlstShortestPath[intPathEdgeIndex].EndNode.X;
                            intY1 = mlstShortestPath[intPathEdgeIndex].EndNode.Y;
                            intX2 = mlstShortestPath[intPathEdgeIndex].StartNode.X;
                            intY2 = mlstShortestPath[intPathEdgeIndex].StartNode.Y;
                            intEdgeEndNodeIndex = mlstShortestPath[intPathEdgeIndex].StartNode.Index;
                        }
                    }
                    else
                        blnFinished = true;
                }
            }
        }

        private bool FindShortestPath(clsNode objStartNode, clsNode objEndNode)
        {
            Double[,] arrCosts;
            int[,] arrShortestPath;
            Double dblCost1;
            Double dblCost2;
            Double dblCostTotal;
            int intCurrentNodeIndex;

            arrCosts = new Double[mlstNodes.Count, mlstNodes.Count];
            arrShortestPath = new int[mlstNodes.Count, mlstNodes.Count];
            for (int intCount1 = 0; intCount1 < mlstNodes.Count; intCount1++)
                for (int intCount2 = 0; intCount2 < mlstNodes.Count; intCount2++)
                {
                    arrShortestPath[intCount1, intCount2] = intCount1;

                    if (intCount1 == intCount2)
                        arrCosts[intCount1, intCount2] = 0;
                    else
                        arrCosts[intCount1, intCount2] = -1;
                }
            foreach (clsEdge objEdge in mlstEdges)
            {
                arrCosts[objEdge.StartNode.Index, objEdge.EndNode.Index] = objEdge.Cost;
                arrCosts[objEdge.EndNode.Index, objEdge.StartNode.Index] = objEdge.Cost;
            }

            for (int intNodeThrough = 0; intNodeThrough < mlstNodes.Count; intNodeThrough++)
            {
                for (int intNodeFrom = 0; intNodeFrom < mlstNodes.Count; intNodeFrom++)
                {
                    dblCost1 = arrCosts[intNodeFrom, intNodeThrough];
                    if (dblCost1 != -1 && intNodeFrom != intNodeThrough)
                        for (int intNodeTo = 0; intNodeTo < mlstNodes.Count; intNodeTo++)
                        {
                            dblCost2 = arrCosts[intNodeThrough, intNodeTo];
                            dblCostTotal = arrCosts[intNodeFrom, intNodeTo];
                            if (dblCost2 != -1 && intNodeThrough != intNodeTo)
                                if ((dblCost1 + dblCost2 < dblCostTotal) | dblCostTotal == -1)
                                {
                                    arrCosts[intNodeFrom, intNodeTo] = dblCost1 + dblCost2;
                                    arrShortestPath[intNodeFrom, intNodeTo] = arrShortestPath[intNodeThrough, intNodeTo];
                                }
                        }
                }
            }
            
            mlstShortestPath = new List<clsEdge>();
            intCurrentNodeIndex = objEndNode.Index;
            while (true)
            {
                if (intCurrentNodeIndex == objStartNode.Index)
                    return true;
                else
                    if (arrCosts[objStartNode.Index, intCurrentNodeIndex] == -1)
                        return false;
                    else
                    {
                        mlstShortestPath.Add(FindEdge(intCurrentNodeIndex,arrShortestPath[objStartNode.Index, intCurrentNodeIndex]));
                        intCurrentNodeIndex = arrShortestPath[objStartNode.Index, intCurrentNodeIndex];
                    }
            }
        }


        private clsEdge FindEdge(int intNode1Index, int intNode2Index)
        {
            foreach (clsEdge objEdge in mlstEdges)
            {
                if ((objEdge.StartNode.Index == intNode1Index && objEdge.EndNode.Index == intNode2Index) |
                    (objEdge.StartNode.Index == intNode2Index && objEdge.EndNode.Index == intNode1Index))
                {
                    return objEdge;
                }
            }
            return null;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            mblnClosing = true;
        }

    }

    [Serializable]
    public class clsNode
    {
        private int mintX;
        private int mintY;
        private int mintIndex;
        
        public clsNode(int intX, int intY, int intIndex)
        {
            mintX = intX;
            mintY = intY;
            mintIndex = intIndex;
        }

        public int X
        {
            get 
            { 
                return mintX ; 
            }
        }

        public int Y
        {
            get 
            {
                return mintY;
            }
        }

        public int Index
        {
            get
            {
                return mintIndex;
            }
        }
    }

    [Serializable]
    public class clsEdge
    {
        private clsNode mobjStartNode;
        private clsNode mobjEndNode;
        private double mdblSpeed;
        private Double mdblCost;


        public clsEdge(clsNode objNode1, clsNode objNode2,Double dblSpeed, Double dblCost)
        {
            mobjStartNode = objNode1;
            mobjEndNode = objNode2;
            mdblSpeed = dblSpeed;
            mdblCost = dblCost;
        }

        public clsNode StartNode
        {
            get
            {
                return mobjStartNode;
            }
        }

        public clsNode EndNode
        {
            get
            {
                return mobjEndNode;
            }
        }

        public Double Speed
        {
            get
            {
                return mdblSpeed;
            }
        }

        public Double Cost
        {
            get
            {
                return mdblCost;
            }
        }

    }
    
}
