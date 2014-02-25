using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace ToolBox.WPF
{
    /// <summary>
    /// Define new behaviors to TextBox : Select all text (on focus, on triple-click), ...
    /// Original source : http://gregandora.wordpress.com/2011/06/09/wpf-textbox-select-all-on-focus
    /// </summary>
    public static class TextBoxHelper
    {
        /// <summary>
        /// Add on Click event TextBox .
        /// On triple click, give Keyboard focus and select all text.
        /// </summary>
        public static void TextBox_SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // If its a triple click, select all text for the user.
            if (e.ClickCount == 3)
            {
                TextBox_SelectAllText(sender, new RoutedEventArgs());
                return;
            }

            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                if (parent is TextBox)
                {
                    var textBox = (TextBox)parent;
                    if (!textBox.IsKeyboardFocusWithin)
                    {
                        // If the text box is not yet focussed, give it the focus and
                        // stop further processing of this click event.
                        textBox.Focus();
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Add to event TextBox, to select all text on some event.
        /// </summary>
        public static void TextBox_SelectAllText(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
