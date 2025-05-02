using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

const string IMAGE_PATH = "../../../image.jpg";

using var image = Image.Load<Rgba32>(IMAGE_PATH);
image.Mutate(x => x
    .Resize(
        new ResizeOptions
        {
            Size = new Size(100, 100),
            Sampler = KnownResamplers.NearestNeighbor
        }));

var colours = new Dictionary<Rgba32, int>();

for (var i = 0; i < image.Height; i++)
{
    for (var j = 0; j < image.Width; j++)
    {
        if (image[i, j].A == 0) continue; // Remove transparent
        if (image[i, j].R == 255 && image[i, j].G == 255 && image[i, j].B == 255) continue; // Remove white
        if (image[i, j].R == 0 && image[i, j].G == 0 && image[i, j].B == 0) continue; // Remove black
        
        if (colours.TryGetValue(image[i, j], out var value))
        {
            colours[image[i, j]] = ++value;
        }
        else
        {
            colours.Add(image[i, j], i);
        }
    }
}

var palette = colours.OrderByDescending(kvp => kvp.Value).ToList();

var primary = palette[0];
var secondary = palette[1];
Console.Write($"{primary.Key} {secondary.Key}");