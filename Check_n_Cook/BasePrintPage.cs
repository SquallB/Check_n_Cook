// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Graphics.Printing;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

namespace Check_n_Cook
{
    public class BasePrintPage : Page
    {
        #region Application Content Size Constants given in percents ( normalized )

        /// <summary>
        /// The percent of app's margin width, content is set at 85% (0.85) of the area's width
        /// </summary>
        protected const double ApplicationContentMarginLeft = 0.075;

        /// <summary>
        /// The percent of app's margin height, content is set at 94% (0.94) of tha area's height
        /// </summary>
        protected const double ApplicationContentMarginTop = 0.03;

        #endregion
        /// <summary>
        /// PrintDocument is used to prepare the pages for printing. 
        /// Prepare the pages to print in the handlers for the Paginate, GetPreviewPage, and AddPages events.
        /// </summary>
        protected PrintDocument printDocument = null;

        /// <summary>
        /// Marker interface for document source
        /// </summary>
        protected IPrintDocumentSource printDocumentSource = null;

        /// <summary>
        /// A list of UIElements used to store the print preview pages.  This gives easy access
        /// to any desired preview page.
        /// </summary>
        internal List<UIElement> printPreviewPages = null;

        public List<HubSection> sectionsToPrint { get; set; }

        public String title;

        /// <summary>
        /// Factory method for every scenario that will create/generate print content specific to each scenario
        /// For scenarios 1-5: it will create the first page from which content will flow
        /// Scenario 6 uses a different approach
        /// </summary>
        protected virtual void PreparePrintContent() {}

        public BasePrintPage()
        {
            printPreviewPages = new List<UIElement>();
            this.sectionsToPrint = new List<HubSection>();
        }

        /// <summary>
        ///  Printing root property on each input page.
        /// </summary>
        protected virtual Canvas PrintingRoot
        {
            get
            {
                return FindName("printingRoot") as Canvas;
            }
        }

        /// <summary>
        /// This is the event handler for PrintManager.PrintTaskRequested.
        /// </summary>
        /// <param name="sender">PrintManager</param>
        /// <param name="e">PrintTaskRequestedEventArgs </param>
        protected virtual void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("Check 'n' Cook Printing", sourceRequested =>
                {
                    // Print Task event handler is invoked when the print job is completed.
                    printTask.Completed += async (s, args) =>
                    {
                        // Notify the user when the print operation fails.
                        if (args.Completion == PrintTaskCompletion.Failed)
                        {
                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                            });
                        }
                    };

                    sourceRequested.SetSource(printDocumentSource);
                });
        } 

        /// <summary>
        /// This function registers the app for printing with Windows and sets up the necessary event handlers for the print process.
        /// </summary>
        protected virtual void RegisterForPrinting()
        {
            // Create the PrintDocument.
            printDocument = new PrintDocument();

            // Save the DocumentSource.
            printDocumentSource = printDocument.DocumentSource;

            // Add an event handler which creates preview pages.
            printDocument.Paginate += CreatePrintPreviewPages;

            // Add an event handler which provides a specified preview page.
            printDocument.GetPreviewPage += GetPrintPreviewPage;

            // Add an event handler which provides all final print pages.
            printDocument.AddPages += AddPrintPages;

            // Create a PrintManager and add a handler for printing initialization.
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;

            // Initialize print content for this scenario
            PreparePrintContent();
        }

        /// <summary>
        /// This function unregisters the app for printing with Windows.
        /// </summary>
        protected virtual void UnregisterForPrinting()
        {
            if (printDocument == null)
                return;

            printDocument.Paginate -= CreatePrintPreviewPages;
            printDocument.GetPreviewPage -= GetPrintPreviewPage;
            printDocument.AddPages -= AddPrintPages;

            // Remove the handler for printing initialization.
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested -= PrintTaskRequested;
        }

        /// <summary>
        /// This is the event handler for PrintDocument.Paginate. It creates print preview pages for the app.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Paginate Event Arguments</param>
        protected virtual void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            // Clear the cache of preview pages 
            printPreviewPages.Clear();

            PrintTaskOptions printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);
            PrintPageDescription printPageDescription = printingOptions.GetPageDescription(0);

            foreach (HubSection section in this.sectionsToPrint)
            {
                PrintPage page = new PrintPage(this.title, section);

                // Set "paper" width
                page.Width = printPageDescription.PageSize.Width;
                page.Height = printPageDescription.PageSize.Height;

                // Get the margins size
                // If the ImageableRect is smaller than the app provided margins use the ImageableRect
                double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * ApplicationContentMarginLeft * 2);
                double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * ApplicationContentMarginTop * 2);

                // Set-up "printable area" on the "paper"
                section.Width = page.Width - marginWidth;
                section.Height = page.Height - marginHeight;

                printPreviewPages.Add(page);
            }

            PrintDocument printDoc = (PrintDocument)sender;

            // Report the number of preview pages created
            printDoc.SetPreviewPageCount(printPreviewPages.Count, PreviewPageCountType.Intermediate);
        }

        /// <summary>
        /// This is the event handler for PrintDocument.GetPrintPreviewPage. It provides a specific print preview page,
        /// in the form of an UIElement, to an instance of PrintDocument. PrintDocument subsequently converts the UIElement
        /// into a page that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Arguments containing the preview requested page</param>
        protected virtual void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            PrintDocument printDoc = (PrintDocument)sender;

            printDoc.SetPreviewPage(e.PageNumber, printPreviewPages[e.PageNumber - 1]);
        }

        /// <summary>
        /// This is the event handler for PrintDocument.AddPages. It provides all pages to be printed, in the form of
        /// UIElements, to an instance of PrintDocument. PrintDocument subsequently converts the UIElements
        /// into a pages that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Add page event arguments containing a print task options reference</param>
        protected virtual void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            // Loop over all of the preview pages and add each one to  add each page to be printied
            for (int i = 0; i < printPreviewPages.Count; i++)
            {
                // We should have all pages ready at this point...
                printDocument.AddPage(printPreviewPages[i]);
            }

            PrintDocument printDoc = (PrintDocument)sender;
            
            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        #region Navigation
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // init printing 
            RegisterForPrinting();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UnregisterForPrinting();
        }
        #endregion
    }
}
