# eppz! `Geometry`
> part of [**Unity.Library.eppz**](https://github.com/eppz/Unity.Library.eppz)

**📐 2D geometry for Unity.** Suited for everyday polygon hassle.

Polygon clipping, polygon winding direction, polygon area, polygon centroid, centroid of multiple polygons, line intersection, point-line distance, segment intersection, polygon-point containment, polygon triangulation, polygon Voronoi diagram, polygon offset, polygon outline, polygon buffer, polygon union, polygon substraction, polygon boolean operations, and more.

![Unity.Library.eppz.Geometry.Model.Poygon.Mesh.Triangulation](https://github.com/eppz/Unity.Library.eppz.Geometry/raw/Documentation/Documentation/Unity.Library.eppz.Geometry.Model.Poygon.Mesh.Triangulation.gif)

The library is **being used in production**. However, it comes with the disclaimed liability and warranty of [MIT License](https://en.wikipedia.org/wiki/MIT_License).

## Examples

If you prefer to read example code immediately, you can find example scenes in [`Scenes`](Scenes) folder.

+ [Polygon-Point containment](Scenes/README.md/#0-polygon-point-containment)
+ [Polygon-Segment intersection test](Scenes/README.md/#1-polygon-segment-intersection)
+ [Polygon permiter-Point containment (Precise)](Scenes/README.md/#2-polygon-permiter-point-containment-precise)
+ [Polygon permiter-Point containment (Default)](Scenes/README.md/#3-polygon-permiter-point-containment-default)
+ [Polygon-Segment containment](Scenes/README.md/#4-polygon-segment-containment)
+ [Polygon-Polygon containment](Scenes/README.md/#5-polygon-polygon-containment)
+ [Vertex facing](Scenes/README.md/#6-vertex-facing)
+ [Polygon area, Polygon winding](Scenes/README.md/#7-polygon-area-polygon-winding)
+ [Segment-Segment intersection point](Scenes/README.md/#8-segment-segment-intersection-point)
+ [Polygon offset](Scenes/README.md/#9-polygon-offset)
+ [Multiple polygon centroid](Scenes/README.md/#10-multiple-polygon-centroid)
+ [Polygon triangulation](Scenes/README.md/#11-polygon-triangulation)

## Model classes

* [`Vertex.cs`](Model/Vertex.cs)
	+ Basically a `Vector2` point, but is aware of the polygon context it resides (neighbours, segments, edges, polygon, bisector, normal).
* [`Segment.cs`](Model/Segment.cs)
	+ Segment of two `Vector2` point. Carries out basic geometry features (point distance, point containment, segment intersection).
* [`Edge.cs`](Model/Edge.cs)
	+ Edge of two `Vertex` in a polygon (a special `Segment` subclass). Likewise vertices, this model is also aware of the polygon context it resides (neighbours, segments, edges, polygon, perpendicular, normal).
* [`Polygon.cs`](Model/Edge.cs)
	+ The role player, it really **embodies mostly every feature of this library**. Basically a polygon made of vertices.
	+ Can be created with point array, transforms, [`Source.Polygon`](Source/Polygon.cs) components. Further polygons can be embedded into recursively. Vertices, edges, polygons can be enumerated (recursively).
	+ Area, winding direction, centroid are being calculated. Also carries the basic geometry features (point containment, line-, segment-, polygon intersection and more).
	+ Using library modules, it implements polygon offset (outline), union polygon (polygon clipping), basic mesh triangulation. It implements conversion to both [Clipper](https://github.com/eppz/Clipper) and [Triangle.NET](https://github.com/eppz/Triangle.NET), so you can implement further integration with those (awesome) libraries.

## [`Geometry.cs`](Geometry.cs)

Most of the basic 2D geometry algorithm collection is implemented in this static base class. You can (mostly) **use them with Unity `Vector2` types directly**, so (almost entirely) without the model classes introduced above.

* **Point**
	+ [**`ArePointsEqualWithAccuracy()`**](Geometry.cs#L24)
		+ Determine if points are equal with a given accuracy.
	+ [**`ArePointsCCW()`**](Geometry.cs#L30)
		+ Determine winding direction of three points.		
* **Rect / Bounds**
	+ [**`IsRectContainsRectSizeWithAccuracy()`**](Geometry.cs#L41)
		+ Determine if `rect2.size` fits into `rect1` (compare sizes only).
	+ [**`IsRectContainsRectWithAccuracy()`**](Geometry.cs#L56)
		+ Determine if `rect2` is contained by `rect1` (even if permiters are touching) with a given accuracy.
* **Line**
	+ [**`IntersectionPointOfLines()`**](Geometry.cs#L78)
		+ Returns intersection point of two lines (defined by segment endpoints). Returns zero, when segments have common points, or when a segment point lies on other.
	+ [**`PointDistanceFromLine()`**](Geometry.cs#L97)
		+ Determine point distance from line (defined by segment endpoints).
* **Segment**
	+ [**`PointIsLeftOfSegment()`**](Geometry.cs#L109)
		+ Determine if a given point lies on the left side of a segment (line beneath).
	+ [**`AreSegmentsEqualWithAccuracy()`**](Geometry.cs#L116)
		+ Determine if segments (defined by endpoints) are equal with a given accuracy.
	+ [**`HaveSegmentsCommonPointsWithAccuracy()`**](Geometry.cs#L125)
		+ Determine if segments (defined by endpoints) have common points with a given accuracy.
	+ [**`AreSegmentsIntersecting()`**](Geometry.cs#L141)
		+ Determine if two segments defined by endpoints are intersecting (defined by points). True when the two segments are intersecting. Not true when endpoints are equal, nor when a point is contained by other segment. Credits to [Bryce Boe](https://github.com/bboe) (@bboe) for his writeup [Line Segment Intersection Algorithm](http://bryceboe.com/2006/10/23/line-segment-intersection-algorithm).
* **Polygon** (using `EPPZ.Geometry.Polygon`)
	+ [**`IsPolygonContainsPoint()`**](Geometry.cs#L159)
		+ Test if a polygon contains the given point (checks for sub-polygons recursive). Uses the same Bryce boe algorithm above, so considerations are the same. See [Point in polygon](https://en.wikipedia.org/wiki/Point_in_polygon#Ray_casting_algorithm) for more.
	+ [**`CentroidOfPolygons()`**](Geometry.cs#L177)
		+ Returns the compound centroid of multiple polygon using [Geometric decomposition](https://en.wikipedia.org/wiki/Centroid#By_geometric_decomposition).

## Modules

For clipping, offsetting, triangulating the library use these brilliant third party `C#` libraries below.

* [Clipper](https://github.com/eppz/Clipper)

	+ Polygon and line clipping and offsetting library (C++, C#, Delphi) by Angus Johnson. See standalone project repository [Clipper](https://github.com/eppz/Clipper) for details.

* [Triangle.NET](https://github.com/eppz/Triangle.NET)

	+ Triangle.NET generates 2D (constrained) Delaunay triangulations and high-quality meshes of point sets or planar straight line graphs. It is a C# port by Christian Woltering of Jonathan Shewchuk's Triangle software. See standalone project repository [Triangle.NET](https://github.com/eppz/Triangle.NET) for details.

## Naming

The library uses namespaces heavily. I like to **name things as they are**. An edge in this library called `Edge`, a polygon is called `Polygon`. If it is a polygon model, it resides the `Model` namespace (`EPPZ.Geometry.Model` actually). Whether it is a source component for polygon, it resides in the `Source` namespace. It becomes nicely readable, as you declare polygons like `Model.Polygon`, or reference polygon sources as `Source.Polygon`.

> In addition, every class is **namespaced in the folder** it resides. If you look at a folder name, you can tell that classes are namespaced to the same as the folder name.

## Add-ons

* [`ClipperAddOns`](AddOns/ClipperAddOns.cs)

	+ Mainly `Polygon` extensions for easy conversion between **eppz! Geometry** and [Clipper](https://github.com/eppz/Clipper). It has a method to convert from generic `Vector2[]` array. **[Clipper](https://github.com/eppz/Clipper) works with integers**. So conversion involves a scale up (and a scale down), thus you'll need to pass a scale value to Clipper. (for example **eppz! Geometry** internals use `10e+5f` by default).
		+ `Polygon PolygonFromClipperPaths(Paths paths, float scale)`
		+ `Polygon PolygonFromClipperPath(Path path, float scale)`
		+ `Paths ClipperPaths(this Polygon this_, float scale)`
		+ `Path ClipperPath(this Polygon this_, float scale)`
		+ `Vector2[] PointsFromClipperPath(Path path, float scale)`

* [`TriangleNetAddOns`](AddOns/TriangleNetAddOns.cs)		

	+ Bridges the gap between library `Model.Polygon` objects and `Triangle.NET` models (meshes, voronoi diagrams).
		+ `TriangleNet.Geometry.Polygon TriangleNetPolygon(this Polygon this_)`
		+ `Rect Bounds(this TriangleNet.Voronoi.Legacy.SimpleVoronoi this_)`
		+ `Paths ClipperPathsFromVoronoiRegions(List<TriangleNet.Voronoi.Legacy.VoronoiRegion> voronoiRegions, float scale = 1.0f)`
		+ `Vector2 VectorFromPoint(TriangleNet.Geometry.Point point)`
		+ `Vector2[] PointsFromVertices(ICollection<TriangleNet.Geometry.Point> vertices)`

* [`UnityEngineAddOns`](AddOns/UnityEngineAddOns.cs)

	+ Contains a single `Model.Polygon` (yet enormously useful) extension that triangulates the corresponding polygon, and hooks up the result into a `UnityEngine.MeshFilter` component. This is the core functionality embedded into `Source.Mesh` component (see example scene [Polygon triangulation](Scenes/README.md/#11-polygon-triangulation) for more).
		+ `UnityEngine.Mesh Mesh(this EPPZ.Geometry.Model.Polygon this_, string name = "")`
		+ `UnityEngine.Mesh Mesh(this EPPZ.Geometry.Model.Polygon this_, TriangulatorType triangulator, string name = "")`
		+ `UnityEngine.Mesh Mesh(this EPPZ.Geometry.Model.Polygon this_, Color color, TriangulatorType triangulator, string name = "")`

## License

> Licensed under the [**MIT License**](https://en.wikipedia.org/wiki/MIT_License).
