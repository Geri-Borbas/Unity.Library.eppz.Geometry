# [eppz! `Geometry`](https://github.com/eppz/Unity.Library.eppz,Geometry)
> part of [**Unity.Library.eppz**](https://github.com/eppz/Unity.Library.eppz)

## Test scenes 

+ [Polygon-Point containment](#0-polygon-point-containment)
+ [Polygon-Segment intersection test](#1-polygon-segment-intersection)
+ [Polygon permiter-Point containment (Precise)](#2-polygon-permiter-point-containment-precise)
+ [Polygon permiter-Point containment (Default)](#3-polygon-permiter-point-containment-default)
+ [Polygon-Segment containment](#4-polygon-segment-containment)
+ [Polygon-Polygon containment](#5-polygon-polygon-containment)
+ [Vertex facing](#6-vertex-facing)
+ [Polygon area, Polygon winding](#7-polygon-area-polygon-winding)
+ [Segment-Segment intersection point](#8-segment-segment-intersection-point)
+ [Polygon offset](#9-polygon-offset)
+ [Multiple polygon centroid](#10-multiple-polygon-centroid)
+ [Polygon triangulation](#11-polygon-triangulation)

![Unity.Library.eppz.Geometry.Model.Poygon.Mesh.Triangulation](https://github.com/eppz/Unity.Library.eppz.Geometry/raw/Documentation/Documentation/Unity.Library.eppz.Geometry.Model.Poygon.Mesh.Triangulation.gif)

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
See [`Controller_0.cs`](Controllers/Controller_0.cs) for the full script context.

## 1. Polygon-Segment intersection

The star polygon drawn yellow when any of the two segments intersects any polygon edge.

+ Returns false when a segment endpoint appears to be on a polygon edge
+ Returns false when a segment endpoint is at a polygon vertex

```C#
bool test = polygon.IsIntersectingWithSegment(segment);
```
See [`Controller_1.cs`](Controllers/Controller_1.cs) for the full script context.

## 2. Polygon permiter-Point containment (Precise)

The star polygon drawn yellow when the point is contained by the polygon permiter. Accuracy means the line width of the polygon permiter (is `1.0f` by default).

+ Returns true even if the point appears to be on a polygon edge
+ Returns true even if the point is at a polygon vertex

```C#
bool test = polygon.PermiterContainsPoint(point, accuracy, Segment.ContainmentMethod.Precise);
```
See [`Controller_2.cs`](Controllers/Controller_2.cs) for the full script context.

**Points contained by a segment** (even edge or polygon permiter) should be calculated with a given **accuracy**. This accuracy is set to `1e-6f` by default, but **can be set to any value** per each containment test (like above).

Point containment tests has two **containment method**, `ContainmentMethod.Default` and `ContainmentMethod.Precise`. The former is less computation intensive than the latter. Depending on your ue case, you may trade precision over performance. The figure below summarizes the dissimilarities of the two method.

![Unity.Library.eppz.Geometry.Segment.ContainsPoint.ContainmentMethod](https://github.com/eppz/Unity.Library.eppz.Geometry/raw/Documentation/Documentation/Unity.Library.eppz.Geometry.Segment.ContainsPoint.ContainmentMethod.png)

## 3. Polygon permiter-Point containment (Default)

Actually the same as before, but a smaller accuracy is given (`0.1f`). The star polygon drawn yellow when the point appears to be on any polygon edge of at a polygon vertex.

+ Returns true even if the point appears to be on a polygon edge
+ Returns true even if the point is at a polygon vertex

```C#
float accuracy = 0.1f;	
bool test = polygon.PermiterContainsPoint(point, accuracy);
```
See [`Controller_3.cs`](Controllers/Controller_3.cs) for the full script context.

## 4. Polygon-Segment containment

The star drawn yellow when it contains both edge. This is a compund test of polygon-point containment, polygon permiter-point containment, polygon-segment intersection.

+ Returns true even if the point appears to be on a polygon edge (thanks to permiter test)
+ Returns true even if the point is at a polygon vertex (thanks to permiter test)

See [`Controller_4.cs`](Controllers/Controller_4.cs) for the full script context.

## 5. Polygon-Polygon containment

The star drawn yellow when it contain the rectangular polygon.  This is also a compund test of polygon-point containment, polygon-segment intersection, polygon permiter-point containment.

A polygon contains another polygon, when
+ other polygon vertices are contained by polygon
+ other polygon segments are not intersecting with polygon
+ other polygon vertices are not contained by polygon permiter

See [`Controller_5.cs`](Controllers/Controller_5.cs) for the full script context.

## 6. Vertex facing

The segments in ths example can think of imaginary polygon edges. The corner vertex normal segment drawn yellow when it faces inward the imaginary polygon, drawn white when facing outward.

It uses `Segment.IsPointLeft()` to create a compund test with both segments. It full calculation goes like below.

The two segments encompasses an acute angle if
+ the endpoint of the second segment lies on the left of the first segment

The vertex normal facing outward if
+ the neighbouring segment encompasses an acute angle
	+ and the normal point lies on the left of both segments
+ or the neighbouring segments encompass an obtuse angle
	+ and the normal point lies on the left of one of the segments

See [`Controller_6.cs`](Controllers/Controller_6.cs) for the full script context.

## 7. Polygon area, Polygon winding

The winding direction of a polygon comes to a good use when you want to validate the result of further polygon operations. The `winding` property of each `Polygon` instance gets calculated at construction time, and each time its topology gets updated (using `UpdatePointPositionsWithSource()`, `UpdatePointPositionsWithTransforms()`, `Scale()`, `Translate()`), or you can invoke `Calculate()` directly if you manipulated underlying point models directly.

In this scene you can see how area and winding of a polygon gets calculated. Just hit play and nudge some points around.

```C#
// After a polygon constructed, you can simply access values.
float area = polygon.area;
bool isCW = polygon.isCW;
```

See [`Model/Polygon.cs`](Model/Polygon.cs) source for the details.

## 8. Segment-Segment intersection point

Segment intersection has two parts actually. Get the **intersection point of the lines** defined by the segment, then look up if the point **resides on the segments**.

The first part has implemented as a generic `Geometry.IntersectionOfSegments()`.

The second part gets more tricky, uses `accuracy` like many tests above. Checks if bounds are overlap, after that it uses the segment-point containment for the endpoints, then checks if the segments has intersection point at all (using winding tests), then returns with the intersection point of lines.

> To avoid redundant test method calls, the method follows API style of `Physics.Raycast`, where you can query if there is intersection at all, then use point only if any.

```C#
// Output will have non-zero value only on having a valid intersection.
Vector2 intersectionPoint;
bool isIntersecting = a.IntersectionWithSegment(b, out intersectionPoint);
```

See [`Controller_8.cs`](Controllers/Controller_8.cs) for the full script context.

## 9. Polygon offset

Robust polygon offset (also known as polygon outline / polygon buffer) solution (using [Clipper](https://github.com/eppz/Clipper) by Angus Johnson).

```C#
float offset = 0.2f;
Polygon offsetPolygon = polygon.OffsetPolygon(offset);
```

See [`Controller_9.cs`](Controllers/Controller_9.cs) for the full script context.

## 10. Multiple polygon centroid

You can see how the compound centorid changes as you nudge polygons, vertices around. Algorithm uses [Geometric decomposition](https://en.wikipedia.org/wiki/Centroid#By_geometric_decomposition).

```C#
// Calculate compund centroid.
centroid.position = Geometry.CentroidOfPolygons(polygons));
```
See [`Controller_10.cs`](Controllers/Controller_10.cs) for the full script context.

## 11. Polygon triangulation

This scene uses a `Source.Mesh` component to simply **triangluate a polygon**. If both `Source.Polygon` and `UnityEngine.MeshFilter` component is present on a `GameObject`, you can use this setup. It uses an extension method `Polygon.Mesh()` that hooks up `Triangle.NET` mesh output into a `UnityEngine.MeshFilter` (with some additional issue resolved regarding self intersecting polygons).

```C#
// Assign trianglated mesh.
meshFilter.mesh = polygon.Mesh(color, triangulator);
```
See [`Source/Mesh.cs`](Source/Mesh.cs) source for the details.

## License

> Licensed under the [**MIT License**](https://en.wikipedia.org/wiki/MIT_License).