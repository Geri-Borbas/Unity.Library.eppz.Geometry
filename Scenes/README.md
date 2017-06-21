# eppz! `Geometry`
> part of [**Unity.Library.eppz**](https://github.com/eppz/Unity.Library.eppz)

## Test scenes

+ [Polygon-Point containment](#0-polygon-point-containment)
+ [Polygon-Segment intersection test](#1-polygon-segment-intersection)

These test scenes are designed to experience / proof the **eppz! Geometry** library features. Hit play, then manipulate the geometry in Scene window while in game mode (watch out to move the points directly instead their parent container). Every relevant code is in the corresponding `Controller_#.cs`, so you can see **how to use the API**.

You can define a polygon with simple `Vector2[]` array, but for sake of simplicity, test scenes uses some **polygon sourcing helper classes** ([`Components.PolygonSource`](../Components/PolygonSource.cs)) those take simple `GameObject` transforms as input. They also keep the polygon models updated on `GameObject` changes.

Another helper objects are **polygon line renderers** ([`Lines.PolygonLineRenderer`](../Lines/PolygonLineRenderer.cs)). These renderers are [`DirectLineRenderer`](https://github.com/eppz/Unity.Library.eppz.Lines/blob/master/DirectLineRenderer.cs) using fellow library [eppz! Lines](https://github.com/eppz/Unity.Library.eppz.Lines).

Beside these helper classes, you can easily construct `Polygon` / `Segment` instances using simple `Vector2` inputs as well. Having these test scenes, you can easily provision the mechanism of each geometry feature even with your own polygons.

## 0. Polygon-Point containment

The star polygon drawn yellow when it contains all three points.

+ When points appear to be on a polygon edge, test will return false
+ When point is at a polygon vertex, test will return false

Usage:
```C#
bool test = polygon.ContainsPoint(point);
```
See [`Controller_0.cs`](Controllers/Controller_0,cs) for the full script context.

## 1. Polygon-Segment intersection

The star polygon drawn yellow when any of the two segments intersects any polygon edge.

+ Returns false when a segment endpoint appears to be on a polygon edge
+ Returns false when a segment endpoint is at a polygon vertex

Usage:
```C#
bool test = polygon.IsIntersectingWithSegment(segment);
```
See [`Controller_1.cs`](Controllers/Controller_1,cs) for the full script context.


## License

> Licensed under the [**MIT License**](https://en.wikipedia.org/wiki/MIT_License).