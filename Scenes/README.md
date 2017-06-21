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

```C#
bool test = polygon.ContainsPoint(point);
```
See [`Controller_0.cs`](Controllers/Controller_0,cs) for the full script context.


## 1. Polygon-Segment intersection

The star polygon drawn yellow when any of the two segments intersects any polygon edge.

+ Returns false when a segment endpoint appears to be on a polygon edge
+ Returns false when a segment endpoint is at a polygon vertex

```C#
bool test = polygon.IsIntersectingWithSegment(segment);
```
See [`Controller_1.cs`](Controllers/Controller_1,cs) for the full script context.


## 2. Polygon permiter-Point containment (Precise)

The star polygon drawn yellow when the point is contained by the polygon permiter. Accuracy means the line width of the polygon permiter (is `1.0f` by default).

+ Returns true even if the point appears to be on a polygon edge
+ Returns true even if the point is at a polygon vertex

```C#
bool test = polygon.PermiterContainsPoint(point, accuracy, Segment.ContainmentMethod.Precise);
```
See [`Controller_2.cs`](Controllers/Controller_2,cs) for the full script context.

**Points contained by a segment** (even edge or polygon permiter) should be calculated with a given **accuracy**. This accuracy is set to `1e-6f` by default, but **can be set to any value** per each containment test (like above).

Point containment tests has two **containment method**, `ContainmentMethod.Default` and `ContainmentMethod.Precise`. The former is less computation intensive than the latter. Depending on your ue case, you may trade precision over performance. The figure below summarizes the dissimilarities of the two method.

![Unity.Library.eppz.Geometry.Segment.ContainsPoint.ContainmentMethod](https://github.com/eppz/Unity.Library.eppz.Geometry/raw/Documentation/Documentation/Unity.Library.eppz.Geometry.Segment.ContainsPoint.ContainmentMethod.png)

## 3. Polygon permiter-Point containment (Default)

Actually the same as before, but a smaller accuracy is given (`0.1f`). The star polygon drawn yellow when the point appears to be on any polygon edge of at a polygon vertex.

+ Returns true even if the point appears to be on a polygon edge
+ Returns true even if the point is at a polygon vertex

Usage:
```C#
float accuracy = 0.1f;	
bool test = polygon.PermiterContainsPoint(point, accuracy);
```
See [`Controller_3.cs`](Controllers/Controller_3,cs) for the full script context.

## 4. Polygon-Segment containment

The star drawn yellow when it contains both edge. This is a compund test of polygon-point containment, polygon permiter-point containment, polygon-segment intersection.

+ Returns true even if the point appears to be on a polygon edge (thanks to permiter test)
+ Returns true even if the point is at a polygon vertex (thanks to permiter test)

See [`Controller_4.cs`](Controllers/Controller_4,cs) for the full script context.

## 5. Polygon-Polygon containment

The star drawn yellow when it contain the rectangular polygon.  This is also a compund test of polygon-point containment, polygon-segment intersection, polygon permiter-point containment.

A polygon contains another polygon, when
+ other polygon vertices are contained by polygon
+ other polygon segments are not intersecting with polygon
+ other polygon vertices are not contained by polygon permiter

See [`Controller_5.cs`](Controllers/Controller_5,cs) for the full script context.

Among some other orientation normalizer stuff, this is the core of tangram! puzzle solving engine, so it is proven by millions of gameplay hours.



## License

> Licensed under the [**MIT License**](https://en.wikipedia.org/wiki/MIT_License).