using Bing.Maps;
using System;
using Windows.Foundation;
/// <summary>
/// 
/// </summary>
public class CustomPushPin
{
    /// <summary>
    /// Gets or sets the pin.
    /// </summary>
    /// <value>
    /// The pin.
    /// </value>
    public Pushpin Pin { get; set; }
    /// <summary>
    /// Gets or sets the location.
    /// </summary>
    /// <value>
    /// The location.
    /// </value>
    public Location Location { get; set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomPushPin"/> class.
    /// </summary>
    /// <param name="location">The location.</param>
    public CustomPushPin(Location location)
    {
        Pin = new Pushpin();
        MapLayer.SetPositionAnchor(Pin, new Point(25 / 2, 39));
        MapLayer.SetPosition(Pin, location);
        this.Location = location;
    }
}