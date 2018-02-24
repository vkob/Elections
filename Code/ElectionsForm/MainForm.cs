using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Elections;
using Elections.Utility;

namespace ElectionsForm
{
   public partial class MainForm : Form
   {
      #region Fields

      #endregion

      private ProcessExcel processExcel = new ProcessExcel();
      private Download download = new Download();
      
      private Thread thread;

      public MainForm()
      {
         InitializeComponent();
      }

      private void Form1_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (processExcel != null)
            processExcel.IsStopped = true;
         if (thread != null)
            thread.Join();
      }

      private void Form1_Load(object sender, EventArgs e)
      {
        // btnSortByDelta_Click(null, null);
         //btnProcessTxtFile_Click(null, null);
      }

      private void btnDownloadDuma2007_Click(object sender, EventArgs e)
      {
         download.Start(Consts.ElectionYear2003);
         //download.Start(Consts.ElectionYear2007);
         MessageBox.Show("Finished");
      }

      private void btnDownloadDuma2011_Click(object sender, EventArgs e)
      {
         download.Start(Consts.ElectionYear2011);
         MessageBox.Show("Finished");
      }


      private void btnDownloadPresident2008_Click(object sender, EventArgs e)
      {
         download.Start(Consts.ElectionYear2008);
         MessageBox.Show("Finished");
      }

      private void btnDownloadPresident2004_Click(object sender, EventArgs e)
      {
         download.Start(Consts.ElectionYear2004);
         MessageBox.Show("Finished");
      }

      private void btnDownloadPresident2012_Click(object sender, EventArgs e)
      {
         download.Start(Consts.ElectionYear2012);
         MessageBox.Show("Finished");
      }

      private void btnRename_Click(object sender, EventArgs e)
      {
         var electionDataRenamer = new ElectionDataRenamer(Consts.LocalPath);
         electionDataRenamer.Start();
      }

      private void btnDownloadXlsFilesDuma_Click(object sender, EventArgs e)
      {
         Download.FindFileForXlsExtraction(Consts.LocalPathDumaResults);
      }

      private void btnDownloadXlsFilesPres_Click(object sender, EventArgs e)
      {
         Download.FindFileForXlsExtraction(Consts.LocalPathPresidentResults);
      }

      private void btnSortByDelta_Click(object sender, EventArgs e)
      {
         var sortByDelta = new SortByDelta();
         sortByDelta.Start(true,  new[] { Consts.ElectionYear2007, Consts.ElectionYear2011 }, Consts.ElectionYear2007, true);
         Close();
      }

      private void btnDrawDiagramForTxt_Click(object sender, EventArgs e)
      {
         var fileName =
           // @"W:\VS2010\duma\Elections\ResultsDuma\Город Москва - Восточная\район Гольяново\СИЗКСРФ\район Гольяново 2011.txt";
           // @"W:\VS2010\duma\Elections\ResultsDuma\Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ 2011.txt";
            //@"W:\VS2010\duma\Elections\ResultsDuma\Территория за пределами РФ\СИЗКСРФ\Территория за пределами РФ 2007.txt";
            //@"W:\VS2010\duma\Elections\ResultsDuma\Забайкальский край\Тунгиро-Олекминская\СИЗКСРФ\тунгиро-олекминская 2011.txt";
            @"W:\VS2010\duma\Elections\ResultsDuma\Республика Саха (Якутия)\Оленекская\СИЗКСРФ\Оленекская 2007.txt";
         

         processExcel.DrawDiagramForTxtData(new FileInfo(fileName), Consts.ElectionYear2007, true);

         Close();
      }

      private void btnGenerateDiagrams_Click(object sender, EventArgs e)
      {
         thread  = new Thread(()=>
                                    processExcel.PrepareDrawAllDiagrams(new [] {Consts.ElectionYear2007, Consts.ElectionYear2011})
                                 );
         thread.Start();
      }

      private void btnGenerateMainGraphics_Click(object sender, EventArgs e)
      {
         {
            //ProcessExcel.GenerateMainGraphic(LocalPath, Consts.ElectionYear2011, ElectionType.Duma);
         }
         Close();
      }

      private void btnSaveXlsToTxt_Click(object sender, EventArgs e)
      {
         processExcel.SaveXlsToTxt(new FileInfo(Consts.LocalPath + @"\" + Consts.ResultsDuma + @"\Агинский Бурятский автономный округ\Агинская\СИЗКСРФ\Агинская 2007.xls"));
      }

      private void btnSaveXlsAsTxtDuma_Click(object sender, EventArgs e)
      {
         processExcel.ExportXls(Consts.LocalPath + @"\" + Consts.ResultsDuma, new[] { Consts.PatternExt2003Xls, Consts.PatternExt2007Xls, Consts.PatternExt2011Xls });
      }

      private void btnSaveXlsAsTxtPres_Click(object sender, EventArgs e)
      {
         processExcel.ExportXls(Consts.LocalPath + @"\" + Consts.ResultsPresident, new[] { Consts.PatternExt2004Xls, Consts.PatternExt2008Xls, Consts.PatternExt2012Xls });
      }

      private void btnProcessTxtFile_Click(object sender, EventArgs e)
      {
         var processTxt = new ProcessTxt();
         //processTxt.Start(LocalPath, Consts.ElectionYear2007);
         //processTxt.Start(LocalPath, Consts.ElectionYear2011);
         //processTxt.Start(LocalPath, Consts.ElectionYear2004);
         processTxt.Start(Consts.ElectionYear2008);
      }
   }
}
