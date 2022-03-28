using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace HWRDemo.View;

public partial class DrawingView : UserControl
{
    public static Action SaveImageAction;
    public static Action ClearCanvasAction;
    private Point currentPoint;

    public DrawingView()
    {
        InitializeComponent();
        SaveImageAction = () => ExportToDisk(PaintSurface);
        ClearCanvasAction = () => { PaintSurface.Children.Clear(); };
    }

    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            currentPoint = e.GetPosition(this);
    }

    private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var line = new Line
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 5,
                X1 = currentPoint.X,
                Y1 = currentPoint.Y,
                X2 = e.GetPosition(this).X,
                Y2 = e.GetPosition(this).Y
            };

            currentPoint = e.GetPosition(this);

            PaintSurface.Children.Add(line);
        }
    }

    public void ExportToDisk(Canvas surface)
    {
        // Save current canvas transform
        var transform = surface.LayoutTransform;
        // reset current transform (in case it is scaled or rotated)
        surface.LayoutTransform = null;

        // Get the size of canvas
        var size = new Size(surface.Width, surface.Height);
        // Measure and arrange the surface
        // VERY IMPORTANT
        surface.Measure(size);
        surface.Arrange(new Rect(size));

        // Create a render bitmap and push the surface to it
        var renderBitmap =
            new RenderTargetBitmap(
                (int) size.Width,
                (int) size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
        renderBitmap.Render(surface);
        // Create a file stream for saving image
        var path = Environment.CurrentDirectory + "\\temp.png";
        using (var ms = new MemoryStream())
        {
            // Use png encoder for our data
            var encoder = new PngBitmapEncoder();
            // push the rendered bitmap to it
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            // save the data to the stream
            encoder.Save(ms);
            var bitmap = new Bitmap(ms);
            bitmap = new Bitmap(bitmap, new System.Drawing.Size(256, 256));
            bitmap.Save(path);
        }

        // Restore previously saved layout
        surface.LayoutTransform = transform;
    }
}