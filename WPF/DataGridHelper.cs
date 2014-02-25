using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ToolBox.WPF
{
    /// <summary>
    /// Define new behaviors to DataGrid : auto-commit (cell mode, focus lost), hide column, ...
    /// </summary>
    public static class DataGridHelper
    {
        #region Auto Commit Module
        /// <summary>
        /// If auto commit is working.
        /// </summary>
        public static DependencyProperty IsAutoCommitEditProperty = 
            DependencyProperty.RegisterAttached("IsManualEditCommit",typeof(bool),typeof(DataGridHelper));

        public static void SetIsManualeditCommit(DataGrid element, Boolean value)
        {
            element.SetValue(IsAutoCommitEditProperty, value);
        }
        public static Boolean GetIsManualeditCommit(DataGrid element)
        {
            return (Boolean)element.GetValue(IsAutoCommitEditProperty);
        }

        /// <summary>
        /// To DataGrid.CellEditEnding event, the edition is committed.
        /// </summary>
        public static void CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                var datagrid = sender as DataGrid;
                if (datagrid == null)
                    return;
                if (!GetIsManualeditCommit(datagrid))
                {
                    SetIsManualeditCommit(datagrid,true);
                    DataGrid grid = (DataGrid)sender;
                    grid.CommitEdit(DataGridEditingUnit.Row, true);
                    grid.CancelEdit(); //Cancel modification if it is not valided.
                    SetIsManualeditCommit(datagrid, false);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// To DataGrid.MouseLeave event, the edition is committed.
        /// </summary>
        public static void MouseLeave(object sender, MouseEventArgs e)
        {
            var datagrid = sender as DataGrid;
            if (datagrid == null)
                return;
            TryCommitEndEdition(datagrid);
        }

        /// <summary>
        /// Try to commit the modifications. If it cannot, cancel the modification.
        /// DataGrid leave edition mode.
        /// </summary>
        public static void TryCommitEndEdition(DataGrid datagrid)
        {
            if (!GetIsManualeditCommit(datagrid))
            {
                SetIsManualeditCommit(datagrid, true);
                IEditableCollectionView collection = datagrid.Items as IEditableCollectionView;
                if (collection.IsEditingItem || collection.IsAddingNew)
                    datagrid.CommitEdit(DataGridEditingUnit.Row, true);
                SetIsManualeditCommit(datagrid, false);
            }
        }
        #endregion

        #region Hide option in menu header to hid column.
        /// <summary>
        /// To hide a column and add button, with Column's HeaderTemplate, in menu to display this column.
        /// </summary>
        /// <remarks>
        /// The last column is not hidden. Because after it is imposible to open context menu.
        /// Column need use HeaderTemplate. Because some bug with Hearder.
        /// </remarks>
        public static void HideDataGridColumn(DataGrid grid,DataGridColumn column,ContextMenu menu)
        {
            //Check if more one column is displayed.
            int nbVisible = 0;
            foreach(var item in grid.Columns)
                if(item.Visibility == Visibility.Visible)nbVisible++;
            if (nbVisible > 1)
            {
                column.Visibility = Visibility.Collapsed;
                var item = new MenuItem();
                item.HeaderTemplate = column.HeaderTemplate;
                item.Click += new RoutedEventHandler(delegate(object sender, RoutedEventArgs e)
                    {
                        column.Visibility = Visibility.Visible;
                        menu.Items.Remove(item);
                    });
                menu.Items.Add(item);
            }
        }
        #endregion

        #region Copy a image of DataGrid in Clipboard
        /// <summary>
        /// Add on MouseDoubleClick event.
        /// </summary>
        /// <param name="dataGrid">DataGrid aim</param>
        public static void InitPushInClipboardEvent(DataGrid dataGrid)
        {
            dataGrid.MouseDoubleClick += new MouseButtonEventHandler(DataGrid_MouseDoubleClickPushInClipboard);
        }

        /// <summary>
        /// If Left shift touch is pressed, then copy a image of DataGrid in Clipboard
        /// </summary>
        public static void DataGrid_MouseDoubleClickPushInClipboard(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift))
                PushInClipboard((DataGrid)sender);
        }

        /// <summary>
        /// Copy a image of DataGrid in ClipBoard.
        /// </summary>
        /// <param name="dataGrid">DataGrid aim</param>
        public static void PushInClipboard(DataGrid dataGrid)
        {
            //Save many attributs
            var tmpRenderSize = dataGrid.RenderSize;
            var tmpHorizontal = dataGrid.HorizontalScrollBarVisibility;
            var tmpVertical = dataGrid.VerticalScrollBarVisibility;
            //Modify the attributs to improve the display.
            dataGrid.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dataGrid.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            dataGrid.UnselectAll();
            dataGrid.Measure(new Size(dataGrid.DesiredSize.Width, dataGrid.DesiredSize.Height));
            dataGrid.Arrange(new Rect(dataGrid.DesiredSize));
            dataGrid.UpdateLayout();
            //Copy the visual in Clipboard
            var img = new RenderTargetBitmap((int)dataGrid.RenderSize.Width, (int)dataGrid.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
            img.Render(dataGrid);
            Clipboard.SetImage(img);
            //Restore the attributs
            dataGrid.HorizontalScrollBarVisibility = tmpHorizontal;
            dataGrid.VerticalScrollBarVisibility = tmpVertical;
            dataGrid.Measure(new Size(tmpRenderSize.Width, tmpRenderSize.Height));
            dataGrid.Arrange(new Rect(tmpRenderSize));
            dataGrid.UpdateLayout();
        }
        #endregion
    }
}
