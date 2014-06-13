using Bing.Maps;
using System;
using Windows.Foundation;
public class CustomPushPin
{
    public Pushpin Pin { get; set; }
    public Location Location { get; set; }
    public CustomPushPin(Location location)
    {
        Pin = new Pushpin();
        MapLayer.SetPositionAnchor(Pin, new Point(25 / 2, 39));
        MapLayer.SetPosition(Pin, location);
        this.Location = location;
    }
}