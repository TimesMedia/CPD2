using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Common;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Data;
using CPD2.Data;

namespace CPD2.Capture
{
      public partial class MainWindow : Window
      {
            Data.DataSet1TableAdapters.Survey2TableAdapter gSurveyAdapter = new Data.DataSet1TableAdapters.Survey2TableAdapter();
            Data.DataSet1TableAdapters.ModuleTableAdapter gModuleAdapter = new Data.DataSet1TableAdapters.ModuleTableAdapter();
            Data.DataSet1TableAdapters.ArticleTableAdapter gArticleAdapter = new Data.DataSet1TableAdapters.ArticleTableAdapter();
            Data.DataSet1TableAdapters.QuestionTableAdapter gQuestionAdapter = new Data.DataSet1TableAdapters.QuestionTableAdapter();
            Data.DataSet1TableAdapters.AnswerTableAdapter gAnswerAdapter = new Data.DataSet1TableAdapters.AnswerTableAdapter();

            Data.DataSet1? gDataSet1;
            CollectionViewSource gSurveyViewSource;
            CollectionViewSource gModuleViewSource;

            List<string> gFacility = new List<string>() { "CPD", "Read only" };
            public MainWindow()
            {
            InitializeComponent();
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            Settings.CPDConnectionString = config["CPDConnectionString"];
                
            gSurveyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("SurveyViewSource")));
            gModuleViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("ModuleViewSource")));

            string lStage = "Before try";


            try
            {
                // Set the Status-strip
                string[] myStatusMessages;
                char[] charSeparators = new char[] { ';' };
                myStatusMessages = Settings.CPDConnectionString.Split(charSeparators, 10, StringSplitOptions.RemoveEmptyEntries);
                string myServer = "";
                string myDataBase = "";
                string myVersion = "";

                lStage = "Before foreach";

                foreach (string myMember in myStatusMessages)
                {
                    if (myMember.StartsWith("Data Source"))
                    {
                        myServer = myMember.Substring(12);
                    }

                    if (myMember.StartsWith("Initial Catalog"))
                    {
                        myDataBase = myMember.Substring(16);
                    }
                }

                lStage = "Before Currentversion";
                //myVersion = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();

                lStage = "Before Title";

                this.Title = "CPD question capture facility " + myServer + " on database " + myDataBase + " Version "; // + myVersion;
            }

            catch (Exception ex)
            {
                //Display all the exceptions

                Exception CurrentException = ex;
                int ExceptionLevel = 0;
                do
                {
                    ExceptionLevel++;
                    ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "MainWindow", lStage);
                    CurrentException = CurrentException.InnerException;
                } while (CurrentException != null);

            }

            }

            private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                gDataSet1 = ((DataSet1)(this.FindResource("DataSet1")));

                try
                {
                    gSurveyAdapter.AttachConnection();
                    gModuleAdapter.AttachConnection();
                    gArticleAdapter.AttachConnection();
                    gQuestionAdapter.AttachConnection();
                    gAnswerAdapter.AttachConnection();
                    LoadAll();
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Window_Loaded", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Window_Loaded; " + ex.Message);
                }

            }

            private void LoadAll()
            {
                gDataSet1.Answer.Clear();
                gDataSet1.Question.Clear();
                gDataSet1.Article.Clear();
                gDataSet1.Module.Clear();
                gDataSet1.Survey2.Clear();

                gSurveyAdapter.Fill(gDataSet1.Survey2);
                gModuleAdapter.Fill(gDataSet1.Module);
                gArticleAdapter.Fill(gDataSet1.Article);
                gQuestionAdapter.Fill(gDataSet1.Question);
                gAnswerAdapter.Fill(gDataSet1.Answer);
                ListFacility.ItemsSource = gFacility;
            }

            private void ButtonExit_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }

            private void ButtonAddSurvey_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    DataSet1.Survey2Row lNewRow = gDataSet1.Survey2.NewSurvey2Row();
                    lNewRow.Naam = "ZZZ New Survey";
                    lNewRow.IssueId = 0;
                    gDataSet1.Survey2.AddSurvey2Row(lNewRow);
                    MessageBox.Show("OK a row has been added with the name ZZZ New Survey . Look at the bottom of the listing on the left.");
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonAddSurvey_Click", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Error in ButtonAddSurvey_Click: " + ex.Message);
                }

            }
            private void ButtonUpdateSurvey_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    gSurveyAdapter.Update(gDataSet1.Survey2);
                    LoadAll();

                    MessageBox.Show("Survey data updated successfully.");
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonUpdateSurvey_Click", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Update of Survey failed: " + ex.Message);
                }
            }

            private void SurveyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    DataRowView lRowView = (DataRowView)gSurveyViewSource.View.CurrentItem;

                    if (lRowView == null)
                    {
                        return;
                    }

                    DataSet1.Survey2Row lRow = (DataSet1.Survey2Row)lRowView.Row;

                    TextRange lTextRange;
                    System.IO.FileStream lFileStream;

                    if (!System.IO.File.Exists(lRow.XMLFileName))
                    {
                        return;
                    }

                    lTextRange = new TextRange(gRichTextBox.Document.ContentStart, gRichTextBox.Document.ContentEnd);
                    using (lFileStream = new System.IO.FileStream(lRow.XMLFileName, System.IO.FileMode.OpenOrCreate))
                    {
                        lTextRange.Load(lFileStream, System.Windows.DataFormats.Rtf);
                    }
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "SurveyDataGrid_SelectionChanged", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Error in SurveyDataGrid_SelectionChanged: " + ex.Message);
                }

            }

            private void ButtonUpdateModule_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    gModuleAdapter.Update(gDataSet1.Module);
                    gDataSet1.Answer.Clear();
                    gDataSet1.Question.Clear();
                    gDataSet1.Article.Clear();

                    gModuleAdapter.Fill(gDataSet1.Module);
                    gArticleAdapter.Fill(gDataSet1.Article);
                    gQuestionAdapter.Fill(gDataSet1.Question);
                    gAnswerAdapter.Fill(gDataSet1.Answer);

                    MessageBox.Show("Module data updated successfully.");
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonUpdateModule_Click", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Update of Module data failed: " + ex.Message);
                }
            }

            private void ButtonUpdateArticle_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    gArticleAdapter.Update(gDataSet1.Article);
                    gDataSet1.Answer.Clear();
                    gDataSet1.Question.Clear();

                    gArticleAdapter.Fill(gDataSet1.Article);
                    gQuestionAdapter.Fill(gDataSet1.Question);
                    gAnswerAdapter.Fill(gDataSet1.Answer);

                    MessageBox.Show("Article data updated successfully.");
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonUpdateArticle_Click", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Add article failed: " + ex.Message);
                }
            }

            private void ButtonUpdateQuestion_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    gQuestionAdapter.Update(gDataSet1.Question);
                    gDataSet1.Answer.Clear();
                    gQuestionAdapter.Fill(gDataSet1.Question);
                    gAnswerAdapter.Fill(gDataSet1.Answer);

                    MessageBox.Show("Question data updated successfully.");
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonUpdateQuestion_Click", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Add question failed: " + ex.Message);
                }
            }

            private void ButtonUpdateAnswer_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    this.Cursor = Cursors.Wait;

                    gAnswerAdapter.Update(gDataSet1.Answer);

                    // Consolidate the new answers
                    DataRowView lRowView = (DataRowView)gSurveyViewSource.View.CurrentItem;
                    CPD2.Data.DataSet1.Survey2Row lRow = (DataSet1.Survey2Row)lRowView.Row;

                    if (!Consolidate(lRow.SurveyId))
                    {
                        MessageBox.Show("There was a problem consolidating the new answers");
                        return;
                    }

                    MessageBox.Show("Answers successfully updated and consolidated for " + lRow.Naam);
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception? CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "ButtonUpdateAnswer_Click", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    MessageBox.Show("Add answer failed: " + ex.Message);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                }

            }

            private bool Consolidate(int pSurveyId)
            {
                this.Cursor = Cursors.Wait;

                try
                {
                    IEnumerable<int> lModuleIds = (IEnumerable<int>)gDataSet1.Module.Where(a => a.SurveyId == pSurveyId).Select(b => b.ModuleId);
                    IEnumerable<int> lArticleIds = (IEnumerable<int>)gDataSet1.Article.Where(a => lModuleIds.Contains(a.ModuleId)).Select(b => b.ArticleId);
                    IEnumerable<DataSet1.QuestionRow> lQuestions = (IEnumerable<DataSet1.QuestionRow>)gDataSet1.Question.Where(a => lArticleIds.Contains(a.ArticleId)).ToList();

                    foreach (DataSet1.QuestionRow lQuestion in lQuestions)
                    {
                        IEnumerable<DataSet1.AnswerRow> lHits = (IEnumerable<DataSet1.AnswerRow>)gDataSet1.Answer.Where(a => a.QuestionId == lQuestion.QuestionId);

                        if (lHits.Count() == 0)
                        {
                            // No answer was changed
                            continue;
                        }

                        // Encode the answer
                        int lEncodedAnswer = 0;
                        int i = 0;
                        IEnumerable<DataSet1.AnswerRow> lAnswers = (IEnumerable<DataSet1.AnswerRow>)gDataSet1.Answer.Where(a => a.QuestionId == lQuestion.QuestionId);

                        foreach (DataSet1.AnswerRow lAnswer in lAnswers)
                        {
                            if (lAnswer.Correct)
                            {
                                lEncodedAnswer += (int)Math.Pow(2, i);
                            }
                            i++;
                        }

                        lQuestion.CorrectAnswer = lEncodedAnswer;
                    }

                    gQuestionAdapter.Update(gDataSet1.Question);

                    return true;
                }

                catch (Exception ex)
                {
                    //Display all the exceptions

                    Exception? CurrentException = ex;
                    int ExceptionLevel = 0;
                    do
                    {
                        ExceptionLevel++;
                        ExceptionData.WriteException(1, ExceptionLevel.ToString() + " " + CurrentException.Message, this.ToString(), "Consolidate", "");
                        CurrentException = CurrentException.InnerException;
                    } while (CurrentException != null);

                    return false;
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                }
            }

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            try {

                List<AvailableModule> lModules = ModuleData.GetAvailableModules(2);
                MessageBox.Show(lModules.Count.ToString());



                List<AvailableSurvey> lSurveys = ModuleData.GetAvailableSurveys(120072);
            MessageBox.Show(lSurveys.Count.ToString()); 
            }
            catch(Exception ex)
            {
            MessageBox.Show(ex.Message);
            }
        }
    }
}
